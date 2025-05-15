using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class npcSpawn : MonoBehaviour
{
    //Spawner Code
    public GameObject meleePlayer;
    public GameObject meleeEnemy;

    [SerializeField] GameObject spawnPointPlayer;
    [SerializeField] GameObject spawnPointEnemy;
    private bool CooldownEnemyActive = false;
    private bool CooldownPlayerActive = false;


    //scoreManager Code
    public static npcSpawn instance;

    public TextMeshProUGUI scoreTextPlayer;
    public TextMeshProUGUI scoreTextEnemy;

    [SerializeField] int scorePlayer = 5;
    [SerializeField] int scoreEnemy = 3;

    int scorePlayerCost = 1;
    int scoreEnemyCost = 1;


    //scoreManager Code
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreTextPlayer.text = scorePlayer.ToString();
        scoreTextEnemy.text = scoreEnemy.ToString();
    }

    //Spawner Code with score remove points
    public void spawnerMeleePlayer()
    {
        if (scorePlayer >= scorePlayerCost && scorePlayer != 0 && CooldownPlayerActive == false)
        {
            StartCoroutine(CooldownPlayer());
            Instantiate(meleePlayer, spawnPointPlayer.transform.position, Quaternion.identity);
            scorePlayer -= scorePlayerCost;
            scoreTextPlayer.text = scorePlayer.ToString();
        }
    }

    public void spawnerMeleeEnemy()
    {
        if (scoreEnemy >= scoreEnemyCost && scoreEnemy != 0 && CooldownEnemyActive == false)
        {
            StartCoroutine(CooldownEnemy());
            Instantiate(meleeEnemy, spawnPointEnemy.transform.position, Quaternion.identity);
            scoreEnemy -= scoreEnemyCost;
            scoreTextEnemy.text = scoreEnemy.ToString();
        }
    }

    private IEnumerator CooldownEnemy()
    {
        CooldownEnemyActive = true;
        yield return new WaitForSeconds(3);
        CooldownEnemyActive = false;
        yield return null;
    }

    private IEnumerator CooldownPlayer()
    {
        Debug.Log("COROUTINE ON");
        CooldownPlayerActive = true;
        yield return new WaitForSeconds(3);
        CooldownPlayerActive = false;
        yield return null;
    }

    //For adding points on prefab destroy
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
}
