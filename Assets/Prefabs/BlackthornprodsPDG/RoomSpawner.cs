using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [Header("1 -> Bottom Door | 2 -> Top Door | 3 -> Left Door | 4 -> Right Door")]
    public int openingDirection;
    /*
     * 1 -> Bottom Door
     * 2 -> Top Door
     * 3 -> Left Door
     * 4 -> Right Door
     */
    private int rand;
    private RoomTemplate templates;
    private bool spawned = false;
    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        Invoke("Spawn",.1f);
    }


    void Spawn()
    {
        if(spawned == false)
        {
            if (openingDirection == 1)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {

                rand = Random.Range(0, templates.topRoooms.Length);
                Instantiate(templates.topRoooms[rand], transform.position, templates.topRoooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {

                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {

                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            spawned = true;
        }
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spawn Point"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                
            }
            Destroy(gameObject);
        }
    }

}