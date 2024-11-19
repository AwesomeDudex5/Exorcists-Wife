using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ScenarioManager scenarioManager;
    public GameObject PauseUI;
    public GameObject savedDataPopup;
    SceneLoader sceneLoader;

    //Enemy Spawners
    public EnemySpawner[] enemySpawners;
    public EnemySpawner bossSpawner;
    private int numberOfEnemiesToSpawn;
    private bool enemiesSpawned;
    private bool bossSpawned;

    private PlayerMovement playerMovement;
    [HideInInspector] public conditions currentCondition;
    private conditions previousCondition; //for the interact types;

    //trigger stats
    [HideInInspector] public bool currentlyInteracting;
    [HideInInspector] public bool isConditionMet;
    [HideInInspector] public bool collisionTrigger;
    [HideInInspector] public bool interactTrigger;
    [HideInInspector] public bool interactCountTrigger;
    [HideInInspector] public int interactableCount = 0;
    [HideInInspector] public int numberOfEnemies;
    [HideInInspector] public int numberOfBosses;
    [HideInInspector] public int currentFlag;

    int currentSceneIndex;
    bool canLoad;
    bool canProceed;



    // Start is called before the first frame update
    void Awake()
    {
        previousCondition = conditions.None;
        canLoad = true;
        canProceed = true;

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        isConditionMet = false;
        currentlyInteracting = false;
        PauseUI.SetActive(false);
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        //load data from file
        loadData();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!currentlyInteracting && scenarioManager.VNMode == false)
            {

                PauseUI.SetActive(!PauseUI.activeInHierarchy);
                savedDataPopup.SetActive(false);

                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                else
                    Time.timeScale = 0;
            }
        }

        //only get the condition as long as the current flag is not pass the total amount of scenes
        if (scenarioManager.currentFlag < scenarioManager.scenarios.Count)
            getCurrentCondition();
        else
            currentCondition = conditions.None;

        if (isConditionMet)
        {
            isConditionMet = false;
            disablePlayer();
            scenarioManager.VNMode = true;
        }
        if (scenarioManager.VNMode == false && currentlyInteracting == false)
        {
            resetPlayer();

            if (canLoad)
            {
                loadNextLevel();
            }

        }
    }

    private void FixedUpdate()
    {
        //  Debug.Log("Current Condition: " + currentCondition + " | currentFlag: " + currentFlag + 
        //   " | numberofinteracts: " + interactableCount + " | interactsNeeded: " + scenarioManager.currentScene.interactCount);
        // Debug.Log("Can Player Interact: " + playerMovement.transform.GetComponent<PlayerShoot>().canInteract);
        // Debug.Log("Current Flag: " + currentFlag + " | playermove: " + playerMovement.canPlay);
        // Debug.Log("Current Flag: " + currentFlag + " | Current Condition: " + currentCondition + " | CONDITIONMET " + isConditionMet);

    }

    void getCurrentCondition()
    {
        currentCondition = scenarioManager.currentScene.condition;
        currentFlag = scenarioManager.currentFlag;

        //check if Condition is met
        conditionCheck(currentCondition);
    }

    void conditionCheck(conditions condition)
    {
        // previousCondition = condition;
        switch (condition)
        {
            case conditions.story:

                /*      if (canProceed && previousCondition == conditions.story && currentCondition == conditions.story)
                      {
                          canProceed = false;
                          StartCoroutine(proceedWithStoryCondition(conditions.None));
                      }
                      */

                if(canProceed && previousCondition == conditions.None)
                {
                    canProceed = false;
                    isConditionMet = true;
                    StartCoroutine(proceedWithStoryCondition(condition));
                }

                if (canProceed && previousCondition != conditions.story)
                {
                    canProceed = false;
                    StartCoroutine(proceedWithStoryCondition(condition));
                }

                break;
            case conditions.killAll:
                spawnEnemies();
                if (numberOfEnemies <= 0)
                {
                    isConditionMet = true;
                    previousCondition = condition;
                }
                break;
            case conditions.onCollision:
                if (collisionTrigger)
                {
                    collisionTrigger = false;
                    isConditionMet = true;
                    previousCondition = condition;
                }
                break;
            case conditions.pressToInteract:
                if (interactTrigger && !currentlyInteracting)
                {
                    interactTrigger = false;
                    isConditionMet = true;
                    previousCondition = condition;
                }
                break;
            case conditions.interactWithAll:
                if (interactCountTrigger && !currentlyInteracting)
                {
                    interactCountTrigger = false;
                    interactableCount++;
                }
                // Debug.Log("Interactable Count: " + interactableCount + " | interacts needed: " + scenarioManager.currentScene.interactCount);
                if (interactableCount == scenarioManager.currentScene.interactCount)
                {
                    interactableCount = 0;
                    isConditionMet = true;
                    previousCondition = condition;
                }
                break;
            default:
                break;

        }
    }
    void disablePlayer()
    {
        playerMovement.canPlay = false;
        playerMovement.transform.GetComponent<PlayerShoot>().canInteract = false;
        // playerMovement.gameObject.SetActive(false);
    }
    void resetPlayer()
    {
        playerMovement.canPlay = true;
        playerMovement.gameObject.SetActive(true);
        if (previousCondition == conditions.pressToInteract || previousCondition == conditions.interactWithAll)
        {
            playerMovement.transform.GetComponent<PlayerShoot>().canInteract = true;
        }
    }

    void spawnEnemies()
    {
        if (previousCondition != conditions.killAll)
        {
            //spawn the number of enemies needed
            numberOfEnemiesToSpawn = scenarioManager.currentScene.enemiesToKill;
            previousCondition = conditions.killAll;
            for (int i = 0; i < numberOfEnemiesToSpawn; i++)
            {
                int randomIndex = Random.Range(0, enemySpawners.Length);
                enemySpawners[randomIndex].spawnEnemy();
            }
            numberOfEnemiesToSpawn = 0;
            numberOfEnemies = getNumberOfEnemiesInScene();
        }
    }

    int getNumberOfEnemiesInScene()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length;
    }

    //spawn the boss then set the condition to kill all;
    void spawnBoss()
    {
        if (previousCondition != conditions.killBoss)
        {
            previousCondition = conditions.killAll;
            bossSpawner.spawnBoss();

        }
    }

    void loadNextLevel()
    {
        if (scenarioManager.currentFlag >= scenarioManager.scenarios.Count)
        {
            currentSceneIndex++;
            // Debug.Log("LOADING: " + currentSceneIndex);
            sceneLoader.loadScene(currentSceneIndex);
            canLoad = false;
        }
    }

    IEnumerator proceedWithStoryCondition(conditions condition)
    {

        yield return new WaitForSeconds(1f);
        isConditionMet = true;
        canProceed = true;
        previousCondition = condition;

    }

    public void quitToMainMenu()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(!PauseUI.activeInHierarchy);
        sceneLoader.loadScene(0);
    }

    public void SaveGame()
    {
        PlayerData pd = new PlayerData(playerMovement.gameObject);
        LevelData ld = new LevelData(currentSceneIndex, currentFlag);
        Debug.Log("Saved Data");
        SaveSystem.SavePlayerData(pd, ld);
        savedDataPopup.SetActive(true);

        Debug.Log("Level Data: " + ld.levelIndex + " | curretn flag " + ld.curretnScenarioIndex);
    }



    void loadData()
    {
        if (sceneLoader.canLoadData)
        {
            sceneLoader.canLoadData = false;
            PlayerData pd = sceneLoader._playerData;
            LevelData ld = sceneLoader._levelData;

            //set player data
            Vector3 newPlayerPosition = new Vector3(pd.caiusPosition[0], pd.caiusPosition[1], pd.caiusPosition[2]);
            playerMovement.gameObject.transform.position = newPlayerPosition;
            playerMovement.GetComponent<PlayerInfo>().playerHealth = pd.health;

            //set level data
            scenarioManager.startingFlag = ld.curretnScenarioIndex;
            scenarioManager.currentFlag = ld.curretnScenarioIndex;
            currentFlag = ld.curretnScenarioIndex;

            currentCondition = scenarioManager.scenarios[currentFlag].condition;
            Debug.Log("Current Flag: " + currentFlag + " | Current Condition: " + currentCondition);


        }
    }


}
