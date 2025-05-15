using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    private float speed = 10f;
    private float damage = 0.8f;
    private float lifetime = 3f;
    public Transform spriteTransform;

    [SerializeField] private LayerMask layersToIgnore;

    void Awake()
    {
        /*if (CompareTag("enemy"))
        {
            spriteTransform.transform.rotation = Quaternion.Euler(45, 45, 45);
        }*/

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (gameObject.tag == "player")
        {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.collider.gameObject.GetComponent<Unit>();
        baseScript basescript = collision.collider.gameObject.GetComponent<baseScript>();

        if (collision.collider.tag != gameObject.tag)
        {
            if ((layersToIgnore & (1 << collision.collider.gameObject.layer)) != 0)
            {
                return;
            }
            //check if unit script is returned
            if (unit!= null)
            {
                unit.health -= damage;
            }

            //check if base script is returned
            if (basescript != null)
            {
                Debug.Log("Base Script Got");
                basescript.health -= damage;
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
