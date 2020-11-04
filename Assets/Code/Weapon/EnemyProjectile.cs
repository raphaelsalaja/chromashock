using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public Transform player;
    private Vector2 target;
    public LayerMask whatIsSolid;
    Rigidbody2D projectileRB;
    public int damage = 1;
    public HealthUI healthUI;

    public float distance;

    public void Start()
    {
        projectileRB = GetComponent<Rigidbody2D>();
        //healthUI = GetComponent<HealthUI>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthUI = GameObject.FindGameObjectWithTag("UI").GetComponent<HealthUI>();
        target = new Vector2(player.position.x, player.position.y);
        Vector2 moveDir = (player.transform.position - transform.position).normalized * speed;
        projectileRB.velocity = new Vector2(moveDir.x, moveDir.y);
    }


    private void Update()
    {

        // transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
        {
            if (hitinfo.collider != null)
            {
                if (hitinfo.collider.CompareTag("Player"))
                {
                    Debug.Log("ENEMY MUST TAKE DAMAGE !");
                    healthUI.TakeDamage(damage);
                    //hitinfo.collider.GetComponent<HealthUI>().TakeDamage(damage);
                }
                DestroyProjectile();
            }
        }
    }

    void DestroyProjectile()
    {
        //Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
