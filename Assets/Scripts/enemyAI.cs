using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    private testManager testManager;
    private int unitChoice;
    private bool firstSpawn;

    [SerializeField] private float barrierToSpawn;

    [SerializeField] private float timeToWaitEnd;

    [SerializeField] private bool enableMelee = true;
    [SerializeField] private bool enableArcher = true;
    [SerializeField] private bool enableSpecial = true;

    [SerializeField] private GameObject turret;
    private bool hasTurret;
    [SerializeField] private float turretDelay;

    public int enemyUnitCount;
    private int spawnCounter;
    // Start is called before the first frame update
    void Start()
    {
        testManager = FindObjectOfType<testManager>();

        firstSpawn = true;
        StartCoroutine(SpawnUnit());

        hasTurret = false;
        turret.SetActive(false);
        Invoke(nameof(spawnTurret), turretDelay);
    }

    /* Update is called once per frame
    void Update()
    {
        SpawnTurret();
    }*/


    IEnumerator SpawnUnit()
    {

        while (true)
        {
            if (firstSpawn == true)
            {
                yield return new WaitForSeconds(4f);
                firstSpawn = false;
            }

            float chanceToSpawn = Random.Range(0f, 1f);

            if (chanceToSpawn >= barrierToSpawn && enemyUnitCount < 6 && spawnCounter <= 1)
            {
                unitChoice = Random.Range(0, 100);

                if (unitChoice > 80 && enableMelee == true)
                {
                    testManager.spawnerEnemyMelee();
                    enemyUnitCount++;
                }
                else if (unitChoice <= 80 && unitChoice >= 45 && enableArcher == true)
                {
                    testManager.spawnerEnemyArcher();
                    enemyUnitCount++;
                }
                else if (enableSpecial == true)
                {
                    testManager.spawnerEnemySpecial();
                    enemyUnitCount++;
                }
            }
            yield return new WaitForSeconds(timeToWaitEnd);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            //Debug.Log("Enemy Spawn Busy");
            spawnCounter++;
            //Debug.Log(spawnCounter);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            //Debug.Log("Enemy Spawn Free");
            spawnCounter--;
            //Debug.Log(spawnCounter);
        }
    }

    void spawnTurret()
    {
        if (hasTurret == false)
        {
            hasTurret = true;
            turret.SetActive(true);
        }
    }
}