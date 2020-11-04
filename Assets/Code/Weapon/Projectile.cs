using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public float speed;
    public float TimeToLive = 5f;
    public float distance;
    public int damage;
    public GameObject HIT_FX;

    public LayerMask whatIsSolid;

    public void Start()
    {
        Invoke("DestroyProjectile", TimeToLive);
        // Destroy(gameObject, TimeToLive);
    }


    private void Update()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
        {
            if (hitinfo.collider != null)
            {
                if (hitinfo.collider.CompareTag("Enemy"))
                {
                    Debug.Log("ENEMY MUST TAKE DAMAGE !");
                    // hitinfo.collider.GetComponent<Enemy>().TakeDamage(damage);
                }

                Instantiate(HIT_FX, transform.position, Quaternion.Inverse(transform.rotation));
                DestroyProjectile();
            }
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);


    }

    void DestroyProjectile()
    {
        //Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}