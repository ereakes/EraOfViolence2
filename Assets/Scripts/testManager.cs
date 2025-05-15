using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class testManager : MonoBehaviour
{
    public GameObject unitPrefab;
    [SerializeField] GameObject spawnPointPlayer;
    [SerializeField] GameObject spawnPointEnemy;
    [SerializeField] private GameObject playerTurret;
    [SerializeField] private GameObject nefText;
    [SerializeField] private GameObject spawnOccText;

    private bool cooldownPlayerActive = false;
    public float cooldownTimer;
    private int spawnCounter;

    private bool hasTurret;
    [SerializeField] private int turretCost;

    public void spawnerPlayerInfantry()
    {
        if(!cooldownPlayerActive)
        { 
            if (money < unitCostInfantry){
            StartCoroutine(NotEnoughFunds());
            }
            else if (money >= unitCostInfantry && money != 0 && spawnCounter <= 1)
            {
                RemovePointInfantry();
                cooldownTimer = 1f;
                SpawnUnit(UnitType.Infantry, spawnPointPlayer, "player");
                StartCoroutine(CooldownPlayer());
            }
        }

        if (spawnCounter != 1)
        {
            StartCoroutine(spawnOccupied());
        }
    }

    public void spawnerPlayerArcher()
    {
        if (!cooldownPlayerActive)
        {
            if (money < unitCostArcher){
                StartCoroutine(NotEnoughFunds());
            }
            else if (money >= unitCostArcher && money != 0 && spawnCounter <= 1)
            {
                RemovePointArcher();
                cooldownTimer = 2f;
                SpawnUnit(UnitType.Archer, spawnPointPlayer, "player");
                StartCoroutine(CooldownPlayer());
            }
        }
        
        if (spawnCounter != 1)
        {
            StartCoroutine(spawnOccupied());
        }
    }

    public void spawnerPlayerSpecial()
    {
        if (!cooldownPlayerActive)
        {
            if (money < unitCostSpecial){
                StartCoroutine(NotEnoughFunds());
            }
            else if (money >= unitCostSpecial && money != 0 && spawnCounter <= 1)
            {
                RemovePointSpecial();
                cooldownTimer = 3f;
                SpawnUnit(UnitType.Special, spawnPointPlayer, "player");
                StartCoroutine(CooldownPlayer());
            }
        }

        if (spawnCounter != 1)
        {
            StartCoroutine(spawnOccupied());        
        }
    }

    public void spawnerPlayerTurret()
    {
        if (money < turretCost){
            StartCoroutine(NotEnoughFunds());
        }
        else if (money >= turretCost && money != 0 && hasTurret == false)
        {
            money -= turretCost;
            scoreText.text = money.ToString();
            hasTurret = true;
            playerTurret.SetActive(true);
        }
    }


    public void SpawnUnit(UnitType unitTypeToSpawn, GameObject spawnPoint, string tag)
    {
        // Instantiate the unit
        GameObject spawnedUnit = Instantiate(unitPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Get the Unit script component
        Unit unitScript = spawnedUnit.GetComponent<Unit>();

        // Set the unit type
        unitScript.unitType = unitTypeToSpawn;
        spawnedUnit.tag = tag;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            //Debug.Log("Spawn Busy");
            spawnCounter++;
            //Debug.Log(spawnCounter);
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            //Debug.Log("Spawn Free");
            spawnCounter--;
            //Debug.Log(spawnCounter);
        }
    }

    public void spawnerEnemyMelee()
    {
        SpawnUnit(UnitType.Infantry, spawnPointEnemy, "enemy");
    }

    public void spawnerEnemyArcher()
    {
        SpawnUnit(UnitType.Archer, spawnPointEnemy, "enemy");
    }

    public void spawnerEnemySpecial()
    {
        SpawnUnit(UnitType.Special, spawnPointEnemy, "enemy");
    }

    private IEnumerator CooldownPlayer()
    {
        cooldownPlayerActive = true;
        yield return new WaitForSeconds(cooldownTimer);
        cooldownPlayerActive = false;
    }

    private IEnumerator NotEnoughFunds(){
        Debug.Log("NEF Start");
        nefText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        nefText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        nefText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        nefText.SetActive(false);
    }

    private IEnumerator spawnOccupied(){
        spawnOccText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        spawnOccText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        spawnOccText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        spawnOccText.SetActive(false);
    }

    //Money Manager Script
    
    public TextMeshProUGUI scoreText;
    public int money = 0;
    private int unitCostInfantry = 150;
    private int unitCostArcher = 250;
    private int unitCostSpecial = 1000;

    private int unitGetInfantry = 200;
    private int unitGetArcher = 330;
    private int unitGetSpecial = 1200;

    void Start()
    {
        money = 1750;
        scoreText.text = money.ToString();
        hasTurret = false;
    }

    public void AddPointInfantry()
    {
        money += unitGetInfantry;
        scoreText.text = money.ToString();
    }

    public void AddPointArcher()
    {
        money += unitGetArcher;
        scoreText.text = money.ToString();
    }

    public void AddPointSpecial()
    {
        money += unitGetSpecial;
        scoreText.text = money.ToString();
    }

    public void RemovePointInfantry()
    {
        money -= unitCostInfantry;
        scoreText.text = money.ToString();
    }

    public void RemovePointArcher()
    {
        money -= unitCostArcher;
        scoreText.text = money.ToString();
    }

    public void RemovePointSpecial()
    {
        money -= unitCostSpecial;
        scoreText.text = money.ToString();
    }

}