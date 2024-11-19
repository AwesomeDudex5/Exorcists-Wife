using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class obstacleDepthParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AddObstacleDepthToChildren();
        this.GetComponent<SortingGroup>().enabled = false;
    }

    void AddObstacleDepthToChildren()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.AddComponent<obstacleDepth>();
        }
    }
}
