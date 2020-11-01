using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
   public float speed;
   public float lifeTime;
   public float distance;
   public int damage;
   public LayerMask whatIsSolid;

   private void Start()
   {
        Destroy(gameObject, lifeTime);
   } 
  
   private void Update()
   {
       RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
       {
           if(hitinfo.collider != null)
           {
                if (hitinfo.collider.CompareTag("Enemy"))
                {
                    Debug.Log("ENEMY MUST TAKE DAMAGE !");
                    hitinfo.collider.GetComponent<Enemy>().TakeDamage(damage);
                }
              //  Destroy(gameObject);
            }
       }
       transform.Translate(Vector2.right * speed * Time.deltaTime);
        
   }
}