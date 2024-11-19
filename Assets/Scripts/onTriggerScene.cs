using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTriggerScene : MonoBehaviour
{
    public int flagToTrigger;
    int currentFlag;
    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentFlag = gm.currentFlag;
        if (collision.tag == "Player")
        {
            if (flagToTrigger == currentFlag)
            {
              
                gm.collisionTrigger = true;
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        currentFlag = gm.currentFlag;
        if (collision.tag == "Player")
        {
            if (flagToTrigger == currentFlag)
            {
               
                gm.collisionTrigger = true;
                this.gameObject.SetActive(false);
            }
        }
    }
}
