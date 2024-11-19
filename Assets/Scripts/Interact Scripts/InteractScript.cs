using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum interactType { basic, storyTrigger }

public class InteractScript : MonoBehaviour
{
    InteractManager im;
    public bool isMission;
    public int flagToTrigger; int currentFlag;
    public bool interactTriggerScene, interactCount;
    public bool isSeal; //will be in conjunction with an interact type

    //basic player stats
    Transform playerPosition;
    float distanceToPlayer;
    public int interactRange;

    //data to send to Interact Manager
    string gameObjectName;
    public string[] staticSentences;
    public string[] missionSentences; //send to IM when it is the current flag and isMission = true

    //if is mission and mission is fulfilled, send mission sentences
    // else if it isnotmission, send the static sentences
    private void Start()
    {
        gameObjectName = this.gameObject.name;
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        im = this.transform.parent.GetComponent<InteractManager>();

        if (staticSentences.Length == 0)
        {
            staticSentences = missionSentences;
        }
        if (missionSentences.Length == 0)
        {
            missionSentences = staticSentences;
        }
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(playerPosition.position, this.transform.position);
        if (distanceToPlayer <= interactRange)
        {
            im.inRange = true;
            im.objectToParseName = gameObjectName;
            if (isMission)
            {
                im.flagBeingPassed = flagToTrigger;

                // send appropiate flags to IM
                if (interactTriggerScene)
                {
                    im.triggeredScene = true;

                }
                if (interactCount)
                {
                    im.triggeredCount = true;
                }
                if (isSeal)
                {
                    im.isSeal = true;
                }
            }

            //send up sentences, have IM handle it
            im.missionSentences = missionSentences;
            im.staticSentences = staticSentences;

            //disbale other interactives
            im.disableOtherInteractives();
        }
        else
        {
            //enable all interactives
            im.enableInteractives();
            im.inRange = false;

            //reset all values
            im.triggeredScene = false;
            im.triggeredCount = false;
            im.isSeal = false;
            im.flagBeingPassed = -1;
        }
    }


    bool getCurrentFlag()
    {
        if (flagToTrigger == im.gm.currentFlag)
        {
            return true;
        }
        return false;
    }
}
