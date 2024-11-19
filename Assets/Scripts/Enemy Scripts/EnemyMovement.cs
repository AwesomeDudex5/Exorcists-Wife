using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    //Transform playerTransform;
    public float detectionRange; //range to detect and move towards player
    public float movementSpeed;
    public float attackRange;
    public float wanderDistance;
    [HideInInspector] public bool canPlay;
    [HideInInspector]public float distance;

    // Start is called before the first frame update
    void Start()
    {
        canPlay = true;
    }


    public void followPlayer(Transform playerTransform)
    {
        if (checkInRange(playerTransform))
        {
            this.transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, movementSpeed * Time.deltaTime);
        }
    }

    public void wander(Transform playerTransform, Vector2 randomPosition)
    {
        if (checkInRange(playerTransform))
        {
            if (distance > attackRange)
            {
                this.transform.position = Vector2.MoveTowards(transform.position, randomPosition, movementSpeed * Time.deltaTime);
            }
        }
    }

    public bool checkInRange(Transform playerTransform)
    {
        distance = Vector2.Distance(playerTransform.position, this.transform.position);
        if (detectionRange > distance)
        {
            return true;
        }
        return false;
    }

}

