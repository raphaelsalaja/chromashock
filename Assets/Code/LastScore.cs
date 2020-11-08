using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LastScore : MonoBehaviour
{
    public GameController gc;
    [Header("Stats")]
    [Space]
    public Text Last_Stage_Text;
    public int last_stage = 1;
    public int last_world = 1;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("Game Controller").GetComponent<GameController>();
        last_stage = gc.last_stage;
        last_world = gc.last_world;
    }
    private void Update()
    {
        Last_Stage_Text.text = "Your Last Score Was: " + last_world + " - " + last_stage;
    }
}
