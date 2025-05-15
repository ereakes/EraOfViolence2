using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class baseScript : MonoBehaviour
{

    [SerializeField] public float health, maxHealth = 50f;
    [SerializeField] WorldSpaceHealthBar healthBar;

    public GameObject victoryMenuUI;
    public GameObject defeatMenuUI;
    public TextMeshProUGUI healthText;

    private bool enemyCollision = false;

    private float StartTime = 0.0f;

    [SerializeField] private int progressInt;


    void Start()
    {
        healthBar.UpdateHealthBar(health, maxHealth);
        health = maxHealth;
        victoryMenuUI.SetActive(false);
        defeatMenuUI.SetActive(false);
    }

    public void Victory()
    {
        victoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        PlayerPrefs.SetInt("LevelProgress", progressInt);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("LevelProgress", 0));
    }

    public void Defeat()
    {
        defeatMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        CheckHealth();

        if (enemyCollision == true)
        {
            StartTime += Time.deltaTime;
            while (StartTime > 2.0f)
            {
                TakeDamage(0.5f);
                StartTime = 0.0f;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
    }

    public void CheckHealth()
    {
        healthBar.UpdateHealthBar(health, maxHealth);
        healthText.text = health.ToString();

        if (health <= 0)
        {
            Destroy(gameObject);

            if (gameObject.tag == "enemy")
            {
                Victory();
            }
            else
            {
                Defeat();
            }
        }
    }

    void OnCollisionStay2D(Collision2D colStay)
    {
        if (gameObject.tag == "player" && colStay.gameObject.tag == "enemy")
        {
            enemyCollision = true;
        }
        else if (gameObject.tag == "enemy" && colStay.gameObject.tag == "player")
        {
            enemyCollision = true;
        }
    }


    void OnCollisionExit2D(Collision2D colExit)
    {
        if (colExit.gameObject.tag != gameObject.tag)
        {
            enemyCollision = false;
            StartTime = 0.0f;
        }
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<WorldSpaceHealthBar>();
    }
}
