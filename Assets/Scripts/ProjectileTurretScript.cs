using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTurretScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage = 1f;
    private Vector2 direction;
    [SerializeField] private Transform spriteTransform;

    [SerializeField] private LayerMask layersToIgnore;

    private float lifetime = 3f;

    void Awake()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetSpeedAndDirection(Vector2 fireDirection, float projectileSpeed)
    {
        direction = fireDirection.normalized;
        speed = projectileSpeed;
        RotateProjectileToDirection();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
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
            if (unit != null)
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

    private void RotateProjectileToDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        /*if (gameObject.tag == "enemy") 
        {
            angle = angle - 180f;
        }*/

        spriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
