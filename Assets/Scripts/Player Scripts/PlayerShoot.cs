using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject crosshair;
    private Camera cam;


    [HideInInspector]
    public Vector2 mousePos;

    //bullet info
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce;
    public float fireRate;
    private bool canFire;
    private bool combatMode;
    [HideInInspector]public bool canInteract;

    PlayerMovement pm;


    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
        combatMode = false;
        crosshair.SetActive(combatMode);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        pm = this.GetComponent<PlayerMovement>();
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.canPlay)
        {
            if (Input.GetKeyDown(KeyCode.C)) //toggle combat mode
            {
                if (combatMode == true)
                {
                    this.gameObject.transform.rotation = Quaternion.identity;
                    enableCombatMode(false);
                }
                else
                {
                    enableCombatMode(true);
                }
            }
        }
        else
        {
            combatMode = false;
            enableCombatMode(false);
        }
        pm.combatMode = combatMode; //set combat mode data to player
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("Mouse Pos: " + mousePos);

        if (combatMode) //if combat mode enable, fire bullets
        {
            if (Input.GetButtonDown("Fire1") && canFire)
            {
                StartCoroutine(Shoot());
            }
        }
    }
    private void FixedUpdate()
    {
        if (combatMode)
            Aim();
    }

    void Aim()
    {
        //crosshair.transform.position = mousePos;
        //calculate rotatio angle for firepoint, have it move along with mouse
        Vector2 firepointPos = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 lookDir = mousePos - firepointPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        firePoint.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator Shoot()
    {
        canFire = false;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    void enableCombatMode(bool toggle)
    {
        canInteract =!toggle; //disable interactions in combat mode
        crosshair.SetActive(toggle);
        combatMode = toggle;
    }

}
