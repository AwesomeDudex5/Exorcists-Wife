using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    public bool playerBullet;
    public bool enemyBullet;

    public float timerToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyTimer(timerToDestroy));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(playerBullet)
        {
            if(collision.tag == "Enemy")
            {
                StopAllCoroutines();
                Destroy(this.gameObject);
            }
        }

        if(enemyBullet)
        {
            if(collision.tag == "Player")
            {
                StopAllCoroutines();
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator DestroyTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
