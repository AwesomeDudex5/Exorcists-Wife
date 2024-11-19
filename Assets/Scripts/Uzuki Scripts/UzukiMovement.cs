using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UzukiBehaviourMode { public bool followCaius, Standyby, Attack, Enabled; }

public class UzukiMovement : MonoBehaviour
{
    bool canMove; //to enable during transitons/animations/dialogue scenes
    public float followRange;
    public float followSpeed;
    public SpriteRenderer sr; //position sprite relative to Caius
    int spriteOrder;

    //player information
    Transform playerTransform;
    PlayerMovement _playerMovement;

    public UzukiBehaviourMode behaviour;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerMovement = playerTransform.gameObject.GetComponent<PlayerMovement>();
        spriteOrder = sr.sortingOrder;

        //set Uzuki's Behavious
        behaviour.followCaius = false;
        behaviour.Standyby = false;
        behaviour.Attack = false;
        behaviour.Enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        positionSprite();// position sprite relative to player
                         // EnableUzuki(_playerMovement.canPlay); //enable/disable Sprite during VN/Interaction
        if (_playerMovement.canPlay)
        {

            behaviour.Attack = _playerMovement.combatMode;
            ActOnBehaviour(behaviour);
        }
    }

    void ActOnBehaviour(UzukiBehaviourMode b)
    {
        if (b.followCaius)
        {
            followPlayer();
        }

    }

    void followPlayer()
    {
        float distance = Vector2.Distance(playerTransform.position, this.transform.position);
        if (distance > followRange)
        {
            this.transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
        }
    }

    void positionSprite()
    {
        float distanceY = this.transform.position.y - playerTransform.position.y;
        if (distanceY > 0)
        {
            sr.sortingOrder = spriteOrder - 1;
        }
        else
        {
            sr.sortingOrder = spriteOrder + 1;
        }
    }

    void EnableUzuki(bool enable)
    {
        if (enable)
        {
            if (sr.gameObject.activeInHierarchy == false)//check if it isn't already enabled to reduce redundancy
            {
                sr.gameObject.SetActive(true);
            }
        }
        else
        {
            sr.gameObject.SetActive(false);
        }
    }


}
