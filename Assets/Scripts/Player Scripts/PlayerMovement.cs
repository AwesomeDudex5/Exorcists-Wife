using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //combat mode: false = tradtional walking
    //combat mode: true = walking, but face towards mouse
    [HideInInspector]
    public bool combatMode;

    public float moveSpeed;
    private float previousSpeed;
    private float defaultSpeed = 0.0f;

    private Camera cam;
    public Rigidbody2D rb;
    public Animator animator;

    [HideInInspector]
    public Vector2 movement;

    [HideInInspector]
    public bool canPlay;

    private void Start()
    {
        canPlay = true;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        previousSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        cam.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
        moveSpeed = defaultSpeed;
        if (canPlay)
        {

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (combatMode == false)
            {
                moveNormal();
            }
            else
            {
                moveShooting();
            }
        }
        else
        {
            moveSpeed = 0;
            animator.SetFloat("Speed", moveSpeed);
        }
    }

    void moveNormal()
    {
        movement.Normalize();

        if (movement != Vector2.zero)
        {
            moveSpeed = previousSpeed;
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        animator.SetFloat("Speed", moveSpeed);
    }

    //change movement so character faces mouse and shoots
    void moveShooting()
    {
        movement.Normalize();
        Vector2 movementDirection = getFaceDirection();

        if (movement == Vector2.zero)
        {
            moveSpeed = defaultSpeed;
        }
        else
        {
            moveSpeed = previousSpeed;
        }
        animator.SetFloat("Horizontal", movementDirection.x);
        animator.SetFloat("Vertical", movementDirection.y);
        animator.SetFloat("Speed", moveSpeed);
    }


    Vector2 getFaceDirection()
    {
        //calculate the difference in x and y positions
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);


        float deltaPosX = Mathf.Round(mousePos.x - transform.position.x);
        float deltaPosY = Mathf.Round(mousePos.y - transform.position.y);

        //offset the x position to allow horizontal facing
        //y value always has more precedence due to rounding
        if (Mathf.Abs(deltaPosX) + 2 > Mathf.Abs(deltaPosY))
        {
            deltaPosY = 0;
        }

        //check for zeroes
        //convert the positions into whole 1s
        if (deltaPosX == 0)
        {
            deltaPosX = 0;
        }
        else
        {
            deltaPosX = deltaPosX / (Mathf.Abs(deltaPosX));
        }
        if (deltaPosY == 0)
        {
            deltaPosY = 0;
        }
        else
        {
            deltaPosY = deltaPosY / (Mathf.Abs(deltaPosY));
        }

        return new Vector2(deltaPosX, deltaPosY);
    }


    void FixedUpdate()
    {
        // rb.velocity = movement * moveSpeed;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

}
