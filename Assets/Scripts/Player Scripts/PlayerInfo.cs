using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int playerHealth;
    public float knockTime; //duration of damage frames
    public float knockbackDistance;
    public SpriteRenderer sr;
    Rigidbody2D rb;
    bool canTakeDamage;
    Color damageColor, originalColor;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        //for damage input
        canTakeDamage = true;
        damageColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        originalColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "EnemyBullet")
        {
            if (canTakeDamage)
            {
                //apply knock back
                Vector2 knockDistance = this.transform.position - collision.transform.position;
                knockDistance = knockDistance * knockbackDistance;
                this.transform.position = new Vector2(transform.position.x + knockDistance.x, transform.position.y + knockDistance.y);
                StartCoroutine(takeDamage());
            }
        }
    }


    IEnumerator takeDamage()
    {
        canTakeDamage = false;
        playerHealth--;
        for (int i = 0; i < 5; i++)
        {

            sr.color = damageColor;
            yield return new WaitForSeconds(knockTime);
            sr.color = originalColor;
            yield return new WaitForSeconds(knockTime);
        }
        yield return new WaitForSeconds(0.5f); //short invicibilty frame
        canTakeDamage = true;
    }

}
