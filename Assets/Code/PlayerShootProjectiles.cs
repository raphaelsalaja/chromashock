using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectiles : MonoBehaviour
{
    [SerializeField] private Transform pfbullet;

    private void Awake()
    {
        GetComponent<PlayerAimWeapon>().OnShoot += PlayerShootProjectiles_OnShoot;
    }

    private void PlayerShootProjectiles_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void PlayerShootProjectlies_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        //shoot
       Transform bulletTransform =  Instantiate(pfbullet, e.gunEndPointPosition, Quaternion.identity);

        Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target target = collider.GetComponent<Target>();
        if(target != null)
        {
            target.Damage();
            Destroy(gameObject);
        }
    }*/
    // This is the code for bullet collision (replace "target" with enemy)
}
