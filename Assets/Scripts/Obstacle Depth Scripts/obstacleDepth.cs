using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class obstacleDepth : MonoBehaviour
{
    float yDistance;
    Transform playerTransform;
    int originalOrder, newOrder;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        originalOrder = this.GetComponentInParent<SortingGroup>().sortingOrder;
        newOrder = originalOrder + 2;
        //set the sprite order layer
        this.GetComponent<SpriteRenderer>().sortingLayerName = this.GetComponentInParent<SortingGroup>().sortingLayerName;
    }

    // Update is called once per frame
    void Update()
    {
        yDistance = playerTransform.position.y - this.transform.position.y;
        //Debug.Log(yDistance);
        if (yDistance > 0)
        {
            this.GetComponent<Renderer>().sortingOrder = newOrder;
        }
        else
        {
            this.GetComponent<Renderer>().sortingOrder = originalOrder;
        }

    }
}
