using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    BoxCollider2D _collider;
    Rigidbody2D _rigidBody;
    GameManager gm;
    public int health;
    public int knockbackForce;
    public SpriteRenderer sr;
    Color damageColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _collider = this.GetComponent<BoxCollider2D>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (gm.currentCondition == conditions.killAll)
            {
                gm.numberOfEnemies--;
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            _rigidBody.isKinematic = false;
            Vector2 knockbackDistance = this.transform.position - collision.transform.position;
            //knockbackDistance = new Vector2(transform.position.x + knockbackDistance.x, transform.position.y + knockbackDistance.y);
            _rigidBody.AddForce(knockbackDistance.normalized * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(knockTime(_rigidBody, 0.2f));
            StartCoroutine(takeDamage());
        }
    }

    IEnumerator takeDamage()
    {
        sr.color = damageColor;
        health--;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    public IEnumerator knockTime(Rigidbody2D rb, float knockbackTime)
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
}
