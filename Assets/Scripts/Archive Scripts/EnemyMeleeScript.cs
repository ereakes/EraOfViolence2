using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeScript : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] float damage = 0.5f;

    private float speed = 5f;
    private float StartTime = 0.0f;

    private bool moving;
    private bool attackCollision = false;

    [SerializeField] WorldSpaceHealthBar healthBar;

    void Start()
    {
        moving = true;
        healthBar.UpdateHealthBar(health, maxHealth);
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (moving)
        {
            Movement();
        }

        if (attackCollision == true)
        {
            moving = false;
            StartTime += Time.deltaTime;
            Debug.Log(StartTime);
            while (StartTime > 2.0f)
            {
                TakeDamage();
                StartTime = 0f;
            }
        }
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<WorldSpaceHealthBar>();
    }

    private void Movement()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D colEnter)
    {
        if (colEnter.gameObject.tag == "player")
        {
            attackCollision = true;
            moving = false;
        }
        else if (colEnter.gameObject.tag == "playerBase")
        {
            moving = false;
        }
        else if (colEnter.gameObject.tag == "enemy")
        {
            moving = false;
        }
    }

    void OnTriggerExit2D(Collider2D col2Exit)
    {
        attackCollision = false;
        moving = true;
        StartTime = 0.0f;
    }

    public void TakeDamage()
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Destroy(gameObject);
            npcSpawn.instance.AddPointPlayerMelee();
        }
    }
 
}
