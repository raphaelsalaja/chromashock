using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]

    float xInput = 0, yInput = 0, speed = 5;
    bool mouseLeft, canShoot;
    Vector3 mousePos, mouseVector;
    public Transform gunSprite, gunTip;
    public SpriteRenderer gunRend;
    public GameObject bulletPrefab;
    private Vector2 playerInput;
    private Rigidbody2D rb;

    public float moveSpeed = 5;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // rb.velocity = playerInput.normalized * moveSpeed;
        GetInput();
        GetMouseInput();
        Movement();
    }

    void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");                                 //capture wasd and arrow controls
        GetMouseInput();
    }
    void GetMouseInput()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     //position of cursor in world
        mousePos.z = transform.position.z;                                  //keep the z position consistant, since we're in 2d
        mouseVector = (mousePos - transform.position).normalized;           //normalized vector from player pointing to cursor
        mouseLeft = Input.GetMouseButton(0);                                //check left mouse button
    }
    void Movement()
    {
        Vector3 tempPos = transform.position;
        tempPos += new Vector3(xInput, yInput, 0) * speed * Time.deltaTime; //move the player based on inpupt captures
        transform.position = tempPos;
    }
}