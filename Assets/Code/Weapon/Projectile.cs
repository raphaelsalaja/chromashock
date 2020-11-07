using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [Space]
    public float speed;
    public float TimeToLive = 5f;
    public float distance;
    public int damage;

    [Header("FX")]
    [Space]
    public GameObject HIT_FX;

    [Header("Collisions")]
    [Space]
    public LayerMask whatIsSolid;

    [Header("Enemy")]
    [Space]
    public EnemyAI enemyAI;


    public void Start()
    {
        enemyAI = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();
        Invoke("DestroyProjectile", TimeToLive);
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
                    hitinfo.collider.GetComponent<EnemyAI>().TakeDamage(damage);

                }
                Instantiate(HIT_FX, transform.position, Quaternion.Inverse(transform.rotation));
                DestroyProjectile();
            }
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        ChooseBulletHit();
        Destroy(gameObject);
    }

    void ChooseBulletHit()
    {
        float RandomValue = UnityEngine.Random.value;
        if (RandomValue <= 0.2)
        {
            FindObjectOfType<AudioManager>().Play("BulletHit");
        }
        else if (RandomValue >= 0.2 && RandomValue < 0.4)
        {
            FindObjectOfType<AudioManager>().Play("BulletHit2");
        }
        else if (RandomValue >= 0.4 && RandomValue < 0.8)
        {
            FindObjectOfType<AudioManager>().Play("BulletHit3");
        }
        else if (RandomValue >= 0.8 && RandomValue <= 1)
        {
            FindObjectOfType<AudioManager>().Play("BulletHit4");
        }
    }
}