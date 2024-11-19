using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject CombatText;
    public GameManager gm;
    private int currentFlag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentFlag = gm.currentFlag;
        if (currentFlag == 2)
        {
            CombatText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.C))
            {
                
                Destroy(this.gameObject);
            }
        }

    }
}
