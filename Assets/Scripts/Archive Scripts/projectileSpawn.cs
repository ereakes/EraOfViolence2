using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileSpawn : MonoBehaviour
{
    public GameObject projectile;
    public static projectileSpawn instance;
    private float StartTime = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }



    void OnTriggerEnter2D(Collider2D colEnter)
    {
        if (colEnter.gameObject.tag == "enemy" || colEnter.gameObject.tag == "enemyBase")
        {
            StartTime += Time.deltaTime;
            Debug.Log("spwan");
            while (StartTime > 1.0f)
            {
                Instantiate(projectile, instance.transform.position, Quaternion.identity);
                StartTime = 0.0f;
            }
        }
    }

}
