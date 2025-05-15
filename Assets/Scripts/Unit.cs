using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Unit : MonoBehaviour
{

    private float speed;
    [SerializeField] private float speedRanged = 1f;
    [SerializeField] private float baseLocation = 13f;
    [SerializeField] private float baseLocationForEnemy = -13f;

    public float health;
    [SerializeField] private float speedDefault = 2f;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage = 0.5f;


    private float attackTimer = 0.0f;
    [SerializeField] private float attackInterval = 2.0f;

    public GameObject projectile;
    public GameObject projPos;
    private float projectileCooldownTimer = 0f;
    [SerializeField] private float projectileCooldown = 1.0f;

    private bool isMoving;
    private bool atBase;
    [SerializeField] private float closeRange = 2f;
    [SerializeField] private float farRange = 8f;

    private Vector2 direction;
    private float distanceThreshold;

    [SerializeField] WorldSpaceHealthBar healthBar;

    public testManager testManager;
    public damageManager damageManager;
    public UnitType unitType;
    public enemyAI enemyAI;

    public Transform spriteTransform;
    [SerializeField] private SpriteRenderer unitSprite;

    [SerializeField] private Sprite infantrySprite;
    [SerializeField] private Sprite archerSprite;
    [SerializeField] private Sprite specialSprite;

    //Archer units wont fire when moving behind a friendly unit when an enemy is in range.


    void Start()
    {
        //Set the default parameters
        isMoving = true;
        atBase = false;
        speed = speedDefault;
        direction = gameObject.CompareTag("enemy") ? Vector2.left : Vector2.right;
        RotateEnemySprite();

        //Initialise the health bar
        healthBar = GetComponentInChildren<WorldSpaceHealthBar>();
        healthBar.UpdateHealthBar(health, maxHealth);
        testManager = FindObjectOfType<testManager>();
        enemyAI = FindObjectOfType<enemyAI>();


        // Assign Damage manager
        if (damageManager == null)
        {
            damageManager = FindObjectOfType<damageManager>();
        }

        damage = damageManager.GetDamageForUnit(unitType);
        health = damageManager.GetHealthForUnit(unitType);
        maxHealth = damageManager.GetHealthForUnit(unitType);

        if (unitType == UnitType.Infantry)
        {
            unitSprite.sprite = infantrySprite;
        }
        else if (unitType == UnitType.Archer)
        {
            unitSprite.sprite = archerSprite;
        }
        else
        {
            unitSprite.sprite = specialSprite;
        }
    }

    void Update()
    {
        HandleMovement();
        CheckBaseCollision();
        HealthCheck();
    }


    private void HandleMovement()
    {
        if (atBase)
        {
            isMoving = false;
            return;
        }

        //Raycast logic
        RaycastHit2D unitHit = Physics2D.Raycast(transform.position, direction, farRange);

        if (unitHit.collider != null)
        {
            distanceThreshold = Vector2.Distance(gameObject.transform.position, unitHit.transform.position);

            if (distanceThreshold <= closeRange)
            {
                Debug.DrawRay(transform.position, direction * closeRange, Color.yellow); //DEBUGGING
                isMoving = false;

                if (unitHit.collider.tag != gameObject.tag)
                {
                    HandleCombat(unitHit.collider);
                    if (unitHit.collider.gameObject.GetComponent<Unit>().health <= 0)
                    {
                        isMoving = true;
                    }
                }
            }
            else if (unitType != UnitType.Archer)
            {
                isMoving = true;
            }
            
            if (unitType == UnitType.Archer)
            {
                if (unitHit.collider.CompareTag(gameObject.tag))
                {
                    HandleRangedCombat();
                    Debug.Log("Tags Same");
                }
                else if (distanceThreshold <= farRange && unitHit.collider.tag != gameObject.tag)
                {
                    SpawnProjectile();
                    speed = speedRanged;
                    Debug.Log("Tags Diff FIRE");
                }
                else
                isMoving = true;
                Debug.Log("Else Speed ranged and moving");
            }
        }
        else
        {
            speed = speedDefault;
            isMoving = true;
        }



        if (isMoving)
        {
            MoveUnit();
        }
    }

    private void HandleCombat(Collider2D target)
    {
        if (target.CompareTag("player") || target.CompareTag("enemy"))
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
            {
                Unit targetUnit = target.GetComponent<Unit>();
                if (targetUnit != null)
                {
                    targetUnit.TakeDamage(damage);
                    if(targetUnit.health <= 0)
                    {
                        attackTimer = 0f;
                        isMoving = true;
                    }
                }
                attackTimer = 0f;
            }
        }
    }

    private void HandleRangedCombat()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, farRange);
        Debug.DrawRay(transform.position, direction * farRange, Color.red); // DEBUGGING

        bool foundPlayer = false;
        bool foundEnemyAfterPlayer = false;

        GameObject enemyTarget = null;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null) continue;


            if (hit.collider.tag == gameObject.tag)
            {
                foundPlayer = true;
            }

            if (hit.collider.tag != gameObject.tag && foundPlayer)
            {
                foundEnemyAfterPlayer = true;
                enemyTarget = hit.collider.gameObject;
                break;
            }
        }

        if (foundPlayer && foundEnemyAfterPlayer && enemyTarget != null)
        {
            SpawnProjectile();
            speed = speedRanged;
        }
        else if (enemyTarget == null)
        {
            speed = speedDefault;
            isMoving = true;
        }
    }

    public void SpawnProjectile()
    {
        projectileCooldownTimer += Time.deltaTime;
        
        if (projectileCooldownTimer >= projectileCooldown)
        {
            GameObject proj = Instantiate(projectile, projPos.transform.position, Quaternion.identity);
            proj.tag = gameObject.tag == "player" ? "player" : "enemy";

            projectileCooldownTimer = 0f;
        }
    }

    private void MoveUnit()
    {
        float movementSpeed = speed * Time.deltaTime;
        gameObject.transform.Translate(direction * movementSpeed);
    }


    private void CheckBaseCollision()
    {
        if (gameObject.tag == "enemy" && gameObject.transform.position.x <= baseLocationForEnemy)
        {
            atBase = true;
        }
        else if (gameObject.tag == "player" && gameObject.transform.position.x >= baseLocation)
        {
            atBase = true;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
    }



    private void HealthCheck()
    {
        healthBar.UpdateHealthBar(health, maxHealth);

        if (health <= 0)
        {
            Destroy(gameObject);

            if (gameObject.tag == "enemy")
            {
                AddPoints();
                enemyAI.enemyUnitCount--;
            }
        }

    }

    public void AddPoints()
    {
        if (unitType == UnitType.Infantry)
        {
            testManager.AddPointInfantry();
        }

        if (unitType == UnitType.Archer)
        {
            testManager.AddPointArcher();
        }

        if (unitType == UnitType.Special)
        {
            testManager.AddPointSpecial();
        }

    }

    private void RotateEnemySprite()
    {
        if (gameObject.tag == "enemy")
        {
            spriteTransform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
}