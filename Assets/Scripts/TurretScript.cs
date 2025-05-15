using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public float range = 10f;
    public float rotationSpeed = 5f;
    public Transform turretHead;

    private Transform target;

    public GameObject projectile;
    
    //private float startTimeProjectile = 0.0f;

    public Transform firePoint;
    public float projectileSpeed = 10f;

    private float fireCountdown = 0f;
    private float fireRate = 1f;

    private string targetTag;
    [SerializeField] private LayerMask layersToIgnore;

    // Update is called once per frame
    void Start()
    {
        if (gameObject.tag == "player")
        {
            targetTag = "enemy";
        }
        else 
        {
            targetTag = "player";
        }
    }
    
    void Update()
    {
        FindClosestTarget();

        if (target != null)
        {
            RotateTowardsTarget();
        }
    }

    void FindClosestTarget()
    {
        // Find all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;  // Reset countdown
            }
            fireCountdown -= Time.deltaTime;
        }
        else
        {
            target = null;  // No enemies in range
        }
    }

    void RotateTowardsTarget()
    {
        if ((layersToIgnore & (1 << target.gameObject.layer)) != 0)
        {
           return;
        }

        Vector2 direction = (gameObject.tag == "player") ?
            (target.position - transform.position) :
            (transform.position - target.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretHead.rotation = Quaternion.Lerp(turretHead.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    }

    void Shoot()
    {
        if ((layersToIgnore & (1 << target.gameObject.layer)) != 0)
        {
            return;
        }

        projectile.tag = gameObject.tag == "player" ? "player" : "enemy";

        GameObject projectileGO = Instantiate(projectile, firePoint.position, firePoint.rotation);
        ProjectileTurretScript proj = projectileGO.GetComponent<ProjectileTurretScript>();

        if (proj != null)
        {
            Vector2 directionToTarget = (target.position - firePoint.position).normalized;
            proj.SetSpeedAndDirection(directionToTarget, projectileSpeed);
        }
    }
}
