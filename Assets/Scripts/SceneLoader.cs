using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    public GameObject loadScreen;
    public Animator loadScreenAnim;

    [HideInInspector] public bool canLoadData;
    [HideInInspector] public PlayerData _playerData;
    [HideInInspector] public LevelData _levelData;

    private void Awake()
    {


        //assign a singleton reference
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //set camera
        loadScreen.GetComponent<Canvas>().worldCamera = GameObject.FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // currentActiveScene = SceneManager.GetActiveScene().buildIndex;

    }

    public void loadScene(int sceneIndex)
    {
        //SceneManager.LoadScene(sceneIndex);
        StartCoroutine(loadAsync(sceneIndex));
    }

    IEnumerator loadAsync(int sceneIndex)
    {
        loadScreen.SetActive(true);
        loadScreenAnim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(3f);


        if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            sceneIndex = 0;
        }


        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (operation.isDone == false)
        {
            yield return null;
        }
        if (operation.isDone)
        {
            loadScreenAnim.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
            loadScreen.SetActive(false);
        }
    }


    /*
    void loadNextScene()
    {
        if (sm.currentFlag >= sm.scenarios.Count)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentSceneIndex += 1;

            //if the next scene is not in build settings, return to main menu
            if (currentSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(currentSceneIndex);
            }
        }
    }
    */
}
