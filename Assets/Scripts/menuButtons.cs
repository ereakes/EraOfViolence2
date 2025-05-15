using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButtons : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject unitMenuUI;
    private bool unitMenuUIOn;

    public GameObject turret;
    private int levelProgress;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        unitMenuUI.SetActive(false);
        levelProgress = PlayerPrefs.GetInt("LevelProgress", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused == true)
            {
                Resume();
            }
            else if (GameIsPaused == false)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (Time.timeScale == 0f)
        {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 1f)
        {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        }

    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
        Resume();
    }

    public void LoadLevel2()
    {
        //if (levelProgress >= 1)
        //{
            SceneManager.LoadScene(2);
            Resume();
        //}
    }

    public void LoadLevel3()
    {
        if (levelProgress >= 2)
        {
            SceneManager.LoadScene(3);
            Resume();
        }
    }

    public void LoadLevel4()
    {
        if (levelProgress >= 3)
        {
            SceneManager.LoadScene(4);
            Resume();
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Resume();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenUnitMenu()
    {
        if (unitMenuUIOn == true)
        {
            unitMenuUI.SetActive(false);
            unitMenuUIOn = false;
        }
        else
        {
            unitMenuUI.SetActive(true);
            unitMenuUIOn = true;
        }
    }

    public void RemoveTurret()
    {
        turret.SetActive(false);
    }

}
