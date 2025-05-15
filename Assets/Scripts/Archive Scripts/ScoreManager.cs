using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreTextPlayer;
    public TextMeshProUGUI scoreTextEnemy;

    int scorePlayer = 1;
    int scoreEnemy = 0;

    int scorePlayerCost = 1;
    int scoreEnemyCost = 1;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreTextPlayer.text = scorePlayer.ToString();
        scoreTextEnemy.text = scoreEnemy.ToString();
    }

    public void AddPointPlayerMelee()
    {
        scorePlayer += 1;
        scoreTextPlayer.text = scorePlayer.ToString();
    }

    public void AddPointEnemyMelee()
    {
        scoreEnemy += 1;
        scoreTextEnemy.text = scoreEnemy.ToString();
    }

    public void RemovePointPlayerMelee()
    {
        if (scorePlayer >= scorePlayerCost && scorePlayer != 0)
        {
            scorePlayer -= scorePlayerCost;
            scoreTextPlayer.text = scorePlayer.ToString();
        }
        
    }

    public void RemovePointEnemyMelee()
    {
        scoreEnemy -= scoreEnemyCost;
        scoreTextEnemy.text = scoreEnemy.ToString();
    }
}
