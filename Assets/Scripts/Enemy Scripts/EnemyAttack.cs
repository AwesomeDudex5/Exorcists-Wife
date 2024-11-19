

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyMovement
{
    public bool contactAttack;
    public bool rangeAttack;

    [Header("Range Attack Attributes")]
    public Animator anim;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce;
    public float shotDelay;
    bool canShoot;
    bool combatMode;
    //public float attackRange;

    Transform playerTansform;
    PlayerMovement pm;
    Vector2 randomPosition;
    bool canGenerate;

    // Start is called before the first frame update
    void Start()
    {
        playerTansform = GameObject.FindGameObjectWithTag("Player").transform;
        pm = playerTansform.GetComponent<PlayerMovement>();
        canGenerate = true;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        canPlay = pm.canPlay;
        if (canPlay)
        {
            if (contactAttack)
            {
                physcialAttack();
            }

            if (rangeAttack)
            {
                if (checkInRange(playerTansform))
                {
                    combatMode = true;
                }
                else
                    combatMode = false;

                wanderAndShoot();
            }
        }
    }


    void physcialAttack()
    {
        followPlayer(playerTansform);
    }

    #region Shooter Enemies Code

    void wanderAndShoot()
    {
        if (combatMode)
        {
            Aim();
            if (canGenerate)
                StartCoroutine(generateRandomPosition());
            wander(playerTansform, randomPosition);

            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator generateRandomPosition()
    {
        canGenerate = false;
        float disanceFromPlayer = distance;
        float randomX = 0;
        float randomY = 0;
        // randomPosition = new Vector2(randomX, randomY);

        //generate coordiantes relative to its position to the characteter
        if (this.transform.position.x > playerTansform.position.x)
            randomX = Random.Range(-wanderDistance, 0) - this.transform.position.x;
        if (this.transform.position.x < playerTansform.position.x)
            randomX = Random.Range(0, wanderDistance) + this.transform.position.x;

        if (this.transform.position.y > playerTansform.position.y)
            randomY = Random.Range(-wanderDistance, 0) - this.transform.position.y;
        if (this.transform.position.y < playerTansform.position.y)
            randomY = Random.Range(0, wanderDistance) + this.transform.position.y;

        randomPosition = new Vector2(randomX, randomY);


        yield return new WaitForSeconds(3f);
        canGenerate = true;
    }

    void Aim()
    {
        Vector2 aimPosition = playerTansform.position;
        //calculate rotatio angle for firepoint, have it move along with mouse
        Vector2 firepointPos = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 lookDir = aimPosition - firepointPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        firePoint.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        anim.SetTrigger("ShootTrigger");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
    

    #endregion



}
