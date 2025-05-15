using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMeleeScript : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] float damage = 0.5f;

    [SerializeField] private float speed = 5f;
    private float StartTime = 0.0f;

    private bool moving;
    private bool attackCollision = false;

    [SerializeField] WorldSpaceHealthBar healthBar;

    void Start()
    {
        moving = true;
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
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
            while (StartTime > 2.0f)
            {
                TakeDamage();
                StartTime = 0.0f;
            }
        }
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<WorldSpaceHealthBar>();
    }

    private void Movement()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D colEnter)
    {
        if (colEnter.gameObject.tag == "enemy")
        {
            attackCollision = true;
            moving = false;
        }
        else if (colEnter.gameObject.tag == "enemyBase")
        {
            moving = false;
        }
        else if (colEnter.gameObject.tag == "player")
        {
            moving = false;
        }
    }

    void OnTriggerExit2D(Collider2D colExit)
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
            npcSpawn.instance.AddPointEnemyMelee();
        }
    }
}
