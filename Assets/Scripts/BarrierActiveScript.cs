using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierActiveScript : MonoBehaviour
{
    public int flagToBeActive;
    BoxCollider2D bc;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bc = this.GetComponent<BoxCollider2D>();
        bc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.currentFlag == flagToBeActive)
        {
            if (bc.enabled == false)
                bc.enabled = true;
        }
        else
        {
            if (bc.enabled)
                bc.enabled = false;
        }
    }
}
