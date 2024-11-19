using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    public bool invertPosition;
    private Vector2 invertedVector = new Vector2(0f, 0f);
    private Animator playerAnimator;
    public GameObject nextWaypoint;
    Transform spawnPoint;
    AreaTransitionManager atm;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = nextWaypoint.transform.GetChild(0).transform;
        atm = GetComponentInParent<AreaTransitionManager>();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Reached");
        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "Waypoint")
        {
            StartCoroutine(moveToNextArea(collision.gameObject));
        }

    }

    IEnumerator moveToNextArea(GameObject player)
    {
        if (playerAnimator == null)
            playerAnimator = player.GetComponent<PlayerMovement>().animator;

        //disable player movement
        player.GetComponent<PlayerMovement>().canPlay = false;

        //play fade in animation
        atm.playFadeIn();

        //wait for cliplength + offset
        yield return new WaitForSeconds(atm.clipLength);

        //move player
        player.transform.position = spawnPoint.position;

        //enable playerMovement
        player.GetComponent<PlayerMovement>().canPlay = true;

        if (invertPosition)
        {
            invertedVector.x = playerAnimator.GetFloat("Horizontal");
            invertedVector.y = playerAnimator.GetFloat("Vertical");
            invertedVector *= -1f;
            playerAnimator.SetFloat("Horizontal", invertedVector.x);
            playerAnimator.SetFloat("Vertical", invertedVector.y);

        }

        //play fade out animation
        atm.playFadeOut();

    }
}
