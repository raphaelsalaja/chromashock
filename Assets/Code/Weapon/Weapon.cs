using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class Weapon : MonoBehaviour
{
    private float timeBtwShots;
    public float startTimeBtwShots;
    Vector3 mousePos, mouseVector;
    public GameObject Projectile;
    public GameObject MuzzleFlash;
    public Transform shotPoint;
    public int side;
    public Camera MyCamera;
    public HealthUI healthUI;
    public SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        healthUI = GameObject.FindGameObjectWithTag("UI").GetComponent<HealthUI>();
    }
    // Update is called once per frame
    private void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //position of cursor in world
        mousePos.z = transform.position.z; //keep the z position consistant, since we're in 2d
        mouseVector = (mousePos - transform.position).normalized; //normalized vector from player pointing to cursor
        if (mouseVector.x > 0)
        {
            side = 1;
            Flip(side);
        }
        if (mouseVector.x < 0)
        {
            side = -1;
            Flip(side);
        }

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 shakeVector = new Vector3(0.1f, 0.1f, 0);
                MyCamera.transform.DOShakePosition(1f, 0.25f, 5, 10f, false, true);
                Instantiate(MuzzleFlash, shotPoint.position, transform.rotation); 
                Instantiate(Projectile, shotPoint.position, transform.rotation);
                healthUI.ReduceAmmo();
                Debug.Log("Hello");
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    private void Flip(int side)
    {

        bool state = (side == 1) ? false : true;
        sr.flipY = state;
    }
}
