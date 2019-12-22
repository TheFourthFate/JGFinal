using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public float tHP = 100f;

    public float tRange = 5f;

    public float tSF = 10f;

    public float tFR = 1f;

    private float tCD = 0f;

    public Transform target;

    public Transform tFP;

    public GameObject redPointLight;

    public GameObject explosionParticle;

    public GameObject bulletPrefab;

    void Start()
    {
        InvokeRepeating("SearchForTarget", 0f, 0.5f);
    }

    void Update()
    {
        TurretTarget();
    }

    public void ShowPointLight()
    {
        StartCoroutine(Shine());
    }

    public IEnumerator Shine()
    {
        redPointLight.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        redPointLight.SetActive(false);
    }

    public void SearchForTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");

        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;

                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= tRange)
        {
            target = nearestEnemy.transform;
        }

        else
        {
            target = null;
        }
    }

    public void TurretTarget()
    {
        if (target == null)
        {
            return;
        }

        if (Player.pAttacking == true)
        {
            tHP -= 1f;
        }

        if (tCD <= 0f)
        {
            TurretShoot();

            tCD = 1f / tFR;
        }

        tCD -= Time.deltaTime;
    }

    public void TurretShoot()
    {
        GameObject bomb = Instantiate(bulletPrefab, tFP.position, Quaternion.identity);

        Rigidbody rb = bomb.gameObject.GetComponent<Rigidbody>();

        rb.AddForce(transform.position - target.position * tSF, ForceMode.Impulse);
    }

    public void Explosion()
    {
        if (tHP <= 0)
        {
            Destroy(this.gameObject);

            Instantiate(explosionParticle, transform.position + (transform.up * 0.2f), Quaternion.identity);
        }  
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Explosion();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tRange);
    }


}
