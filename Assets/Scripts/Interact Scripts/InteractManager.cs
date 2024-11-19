
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : MonoBehaviour
{
    public Sprite sealPrefab;
    public GameObject[] sealSpawners;
    [HideInInspector] public GameManager gm;

    public GameObject interactPanel;
    public GameObject interactButton;
    Text interactText;

    GameObject[] interactables;
    [HideInInspector] public string[] sentenceToParse;
    [HideInInspector] public string[] staticSentences;
    [HideInInspector] public string[] missionSentences;
    [HideInInspector] public bool inRange;
    [HideInInspector] public string objectToParseName;
    [HideInInspector] public bool triggeredScene;
    [HideInInspector] public bool triggeredCount;
    [HideInInspector] public bool isSeal;
    [HideInInspector] public int flagBeingPassed;

    private bool canInteract; //enables/disables interact button
    private bool isProcessing;
    private bool isDialogueFinished;
    private int clickIncrement;
    private int currentSentence = 0;
    private GameObject _player; //disable player control while processing sentences


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        // interactables = GameObject.FindGameObjectsWithTag("Interactables");

        //get all interactable children
        interactables = GameObject.FindGameObjectsWithTag("Interactable");

        //get access to gameManager stats to send data
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        interactText = interactPanel.transform.GetComponentInChildren<Text>();
        inRange = false;
        canInteract = true;
        isProcessing = false;
        isDialogueFinished = true;
        interactButton.SetActive(false);
        interactPanel.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Debug.Log("INTERACT MANAGER Current Flag: " + gm.currentFlag + " | objectToParse Name: " + objectToParseName + " | triggered Scene: " + triggeredScene);

    }

    // Update is called once per frame
    void Update()
    {
        interactButton.SetActive(false);

        //do not do anyof this when VN MODE is on or player is in Combat Mode
        if (inRange && _player.GetComponent<PlayerShoot>().canInteract && gm.scenarioManager.VNMode == false)
        {

            //only process dialogue in range
            if (isProcessing && isDialogueFinished)
            {
                canInteract = false;
                isProcessing = false;
                StartCoroutine(processSentences());
            }

            if (canInteract) //set interact button
            {
                interactButton.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                sentenceToParse = staticSentences;
                //disable player movement
                gm.currentlyInteracting = true;
                _player.GetComponent<PlayerMovement>().canPlay = false;


                if (!isDialogueFinished) //click to skip dialogue
                    clickIncrement++;
                else //begin processing dialogue
                {
                    isProcessing = true;
                }

                //mission related triggers
                if (verifyFlag())
                {
                    if (triggeredScene)
                    {
                        triggeredScene = false;
                        disableMission();
                        gm.interactTrigger = true;
                        _player.GetComponent<PlayerMovement>().canPlay = false;
                    }
                    if (triggeredCount)
                    {
                        triggeredCount = false;
                        disableMission();
                        gm.interactCountTrigger = true;
                    }
                    if (isSeal)
                    {
                        disableSeal();
                        placeSeal();
                    }
                }

            }
        } //end of inRange
    }

    #region enable/disable interactives 
    public void disableOtherInteractives()
    {
        foreach (GameObject go in interactables)
        {
            if (go.name != objectToParseName)
            {
                // go.SetActive(false);
                go.GetComponent<InteractScript>().enabled = false;
            }
        }
    }

    public void enableInteractives()
    {
        foreach (GameObject go in interactables)
        {
            //  go.SetActive(true);
            go.GetComponent<InteractScript>().enabled = true;
        }
    }

    public void disableMission()
    {
        foreach (GameObject go in interactables)
        {
            if (go.name == objectToParseName)
            {
                go.GetComponent<InteractScript>().isMission = false;
                go.GetComponent<InteractScript>().interactTriggerScene = false;
                go.GetComponent<InteractScript>().interactCount = false;
                go.GetComponent<InteractScript>().staticSentences = missionSentences;
            }
        }
    }

    public void disableSeal()
    {
        foreach (GameObject go in interactables)
        {
            if (go.name == objectToParseName)
            {
                go.GetComponent<InteractScript>().isSeal = false;
            }
        }
    }

    #endregion

    #region seal scripts
    public void spawnSeal()
    {
        for (int i = 0; i < sealSpawners.Length; i++)
        {
            sealSpawners[i].SetActive(true);
        }
    }

    public void placeSeal()
    {
        foreach (GameObject go in interactables)
        {
            if (go.name == objectToParseName)
            {
                go.GetComponent<SpriteRenderer>().sprite = sealPrefab;
            }
        }
    }

    #endregion

    #region processing sentences
    IEnumerator processSentences()
    {
        if (currentSentence < sentenceToParse.Length) //if theres still sentences left to be processed
        {
            string sentences = sentenceToParse[currentSentence];
            //enables appropiate ui
            interactButton.SetActive(false);
            interactPanel.SetActive(true);
            interactText.text = "";
            isDialogueFinished = false;
            foreach (char c in sentences)
            {
                if (interactText.text.Length < sentences.Length)
                {
                    interactText.text += c;
                    yield return new WaitForSeconds(0.05f);
                }
                if (clickIncrement == 1)
                {
                    clickIncrement = 0;
                    interactText.text = sentences;
                }
            }
            currentSentence++;
        }
        else
        {
            interactPanel.SetActive(false);
            canInteract = true;
            currentSentence = 0;
            isProcessing = false;
            //end of interact dialogue
            gm.currentlyInteracting = false;
            _player.GetComponent<PlayerMovement>().canPlay = true;
        }
        clickIncrement = 0;
        isDialogueFinished = true;
    }
    #endregion

    bool verifyFlag()
    {
        if (flagBeingPassed == gm.currentFlag)
        {
            sentenceToParse = missionSentences;
            return true;
        }
        else
        {
            return false;
        }
    }
}
