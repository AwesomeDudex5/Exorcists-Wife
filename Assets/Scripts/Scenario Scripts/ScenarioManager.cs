using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[System.Serializable]
public class ScenarioManager : MonoBehaviour
{
    //Playable Director for Cutscenes
    PlayableDirector director;

    //VISUAL NOVEL UI
    [Header("Visual Novel UI")]
    public GameObject vnCanvas;
    public GameObject namePanel;
    // public LeanTweenType namePanelAnimation;
    Text nameText;
    public GameObject dialoguePanel;
    // public LeanTweenType dialoguePanelAnimation;
    Text dialogueText;
    public Text MissionText;
    public GameObject background;
    // public LeanTweenType backgroundAnimation;
    public Animator UIanim;
    private bool isUIAnimated;

    //Access to characters and list of scenes
    [Header("Character and Sprite Managers")]
    public SpriteRenderer[] gameSpritePositions;
    public CharacterManager cm;

    [HideInInspector] public int numberOfScenes;
    [HideInInspector] public List<SceneObj> scenarios;

    public int startingFlag = 0;
    [HideInInspector] public int currentFlag;

    //one click to process the dialogue, another to skip the processing and set the sentence
    //after sentence is done, reset click increment
    private int clickIncrement;
    private int dialogueIndex;

    [HideInInspector] public bool VNMode;
    private bool canProcessScene;
    private bool isDialogueFinished;

    [HideInInspector] public SceneObj currentScene;
    [HideInInspector] public Dialogue currentDialogue;

    float cutSceneLength;
    bool isAnimationPlayed;
    bool proceedWtihScene;

    private void Start()
    {
        currentFlag = startingFlag;
        //set text from panel objcts
        nameText = namePanel.GetComponentInChildren<Text>();
        dialogueText = dialoguePanel.GetComponentInChildren<Text>();

        VNMode = false;
        clickIncrement = 0;
        dialogueIndex = 0;

        canProcessScene = true;
        isDialogueFinished = true;

        //set the current scene to the first index
        currentScene = scenarios[currentFlag];

        //disable each spriterender
        for (int i = 0; i < gameSpritePositions.Length; i++)
        {
            gameSpritePositions[i].gameObject.SetActive(false);
        }
        //disable the VN UI
        enableUI(false);
        isUIAnimated = false;

        //set animation and director
        director = this.GetComponent<PlayableDirector>();
        isAnimationPlayed = false;
        proceedWtihScene = true;
    }

    private void Update()
    {
        displayMission();
        if (VNMode)
        {
            hideMission();
            StartCoroutine(playCutscene()); //play cutsene
            StartCoroutine(playUIAnimationStart()); //set up VN UI
            processScene(proceedWtihScene); //display dialogues in scene

        }// End of VN Mode
    }

    private void FixedUpdate()
    {

        // Debug.Log("Click Increment: " + clickIncrement);
        //  Debug.Log("Can Process: " + canProcessScene + " | isDialogue Finished " + isDialogueFinished + " | VN Mode " + VNMode + " | dialogue index " + dialogueIndex);
        //   Debug.Log("current flag: " + currentFlag);
    }


    IEnumerator playCutscene()
    {
        //as long as the animation clip is not null, otherwise just skip and go straight to processing
        if (isAnimationPlayed == false && scenarios[currentFlag]._animation != null)
        {
            isAnimationPlayed = true;
            proceedWtihScene = false;
            canProcessScene = false;

            // rebuild director for runtime playing and play cutscene
            director.playableAsset = scenarios[currentFlag]._animation;
            director.time = 0.0;
            director.Play();

            cutSceneLength = (float)scenarios[currentFlag]._animation.duration;

            yield return new WaitForSeconds(cutSceneLength);

            //reset playable director
            director.playableAsset = null;

            proceedWtihScene = true;
            canProcessScene = true;
            proceedWtihScene = true;
        }
    }

    IEnumerator playUIAnimationStart()
    {
        if (proceedWtihScene)
        {
            proceedWtihScene = false;
            if (isUIAnimated == false && VNMode == true)
            {
                isUIAnimated = true;
                proceedWtihScene = false;

                //set up UI
                enableUI(VNMode);

                //Animate UI
                UIanim.SetTrigger("VNMODE-ON");

                yield return new WaitForSeconds(0.75f);

            }
            proceedWtihScene = true;
        }
    }

    IEnumerator playUIAnimationEnd()
    {
        if (isUIAnimated == false && VNMode == false)
        {
            isUIAnimated = true;

            //animate UI
            UIanim.SetTrigger("VNMODE-OFF");
            yield return new WaitForSeconds(0.75f);

            isUIAnimated = false;
            proceedWtihScene = true;
            enableUI(VNMode);
        }
    }


    void processScene(bool canProceed)
    {
        if (canProceed)
        {
            enableUI(VNMode);
            if (currentFlag < scenarios.Count)
            {
                //get input from player while reading
                //increment the clicks to skip dialogue or move onto next scene
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    if (!isDialogueFinished)
                    {
                        clickIncrement++;
                    }
                    else
                    {
                        canProcessScene = true;
                    }

                }
                if (isDialogueFinished && canProcessScene)
                {
                    canProcessScene = false;
                    processDialogue(currentScene.dialogues.ToArray(), currentFlag);
                }
            }
        }
    }

    #region Mission UI Code
    void displayMission()
    {
        if (currentFlag < scenarios.Count)
            MissionText.text = currentScene.missionDescription;
        else
            MissionText.text = "";
    }

    void hideMission()
    {
        MissionText.text = "";
    }
    #endregion

    void processDialogue(Dialogue[] dialogues, int currentSceneIndex)
    {
        //  Debug.Log("Scene " + currentFlag + ": Number of Dialogues" + dialogues.Length);
        if (dialogueIndex < dialogues.Length)
        {
            currentDialogue = dialogues[dialogueIndex];
            int spritePosition = scenarios[currentSceneIndex].dialogues[dialogueIndex].spritePosition;
            nameText.text = currentDialogue.characterName; //set name
            gameSpritePositions[spritePosition].gameObject.SetActive(true); //enable sprite
            gameSpritePositions[spritePosition].sprite = dialogues[dialogueIndex].characterSprite; //set character sprite

            // StartCoroutine(playAnimation(currentDialogue.animationType, gameSpritePositions[spritePosition].gameObject));//play animation
            StartCoroutine(displaySentence(currentDialogue.sentences)); //set sentences
        }
        else //if at the end of the dialogue for the scene, reset the scene to prepare for the next scene
        {
            resetScene();
        }
    }

    IEnumerator displaySentence(string sentences)
    {
        isDialogueFinished = false;

        dialogueText.text = "";
        foreach (char c in sentences)
        {
            if (dialogueText.text.Length < sentences.Length)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(0.08f);
            }
            if (clickIncrement == 1)
            {
                clickIncrement = 0;
                dialogueText.text = sentences;
            }
        }

        clickIncrement = 0;
        dialogueIndex++;
        isDialogueFinished = true;
    }

    /* IEnumerator playAnimation(animationTypes anim, GameObject spriteObject)
     {
         //reset the scale so it doesnt mirror on normal animations
         spriteObject.transform.parent.localScale = new Vector3(1, 1, 1);
         Vector3 mirrorPosition = new Vector3(-1, 1, 1);

         Animator spriteAnim = spriteObject.GetComponent<Animator>();
         spriteAnim.StopPlayback();
         spriteAnim.SetBool("canPlayAnim", true);

         switch (anim)
         {
             case animationTypes.jump:
                 spriteAnim.Play("Jump");
                 break;
             default:
                 break;
         }
         yield return new WaitForSeconds(0.15f);
         // spriteObject.transform.parent.position = spriteObject.transform.localPosition;
         spriteAnim.SetBool("canPlayAnim", false);

     }
     */

    void resetScene()
    {
        //set all sprite positions to null
        for (int i = 0; i < gameSpritePositions.Length; i++)
        {
            gameSpritePositions[i].sprite = null;
        }

        //reset the texts
        dialogueText.text = "";
        nameText.text = "";

        canProcessScene = true;
        clickIncrement = 0;
        dialogueIndex = 0;


        //end of scene process

        //increment to next scene
        currentFlag++;
        if (currentFlag < scenarios.Count)
        {
            currentScene = scenarios[currentFlag];
        }

        VNMode = false;

        //UI Animate OUT
        isUIAnimated = false;
        isAnimationPlayed = false;

        StartCoroutine(playUIAnimationEnd());
    }

    void enableUI(bool canEnable)
    {
        if (canEnable)
            vnCanvas.SetActive(true);
        else
            vnCanvas.SetActive(false);
    }


}
