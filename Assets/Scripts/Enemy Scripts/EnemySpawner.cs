using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] listOfEnemies;

    public void spawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, listOfEnemies.Length);
        float randOffsetX = Random.Range(0, 3f) + this.transform.position.x;
        float randOffsetY = Random.Range(0, 3f) + this.transform.position.y;
        Vector3 newPosition = new Vector3(randOffsetX, randOffsetY, 0);
        Instantiate(listOfEnemies[randomEnemyIndex], newPosition, Quaternion.identity);
    }

    public void spawnBoss()
    {
        Instantiate(listOfEnemies[0], this.transform);
    }
}
