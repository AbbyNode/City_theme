using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken_1 : MonoBehaviour
{
    public float fireRate = 0;
    public float Damage = 10;
    public LayerMask whatToHit;
    public GameObject shuriken;

    float timeToFire = 0;
    public Transform firePoint;

    // Use this for initialization
    void Awake()
    {
        //firePoint = transform.FindChild("FirePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ThrowShuriken();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                ThrowShuriken();
            }
        }
    }
    void ThrowShuriken()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        SpawnShuriken();
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
        }
    }
    void SpawnShuriken()
    {
        Instantiate(shuriken, firePoint.position, firePoint.rotation);
    }
}
