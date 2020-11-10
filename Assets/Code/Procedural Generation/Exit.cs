using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

    public LevelGenerator levelGenerator;
    private SceneReloader sr;
    public Animator transisition;
    private HealthUI ui;
    private HealthUI ui_2;
    public float chanceWalkerSpawnEnemey = 0.05f;
    private void Awake()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();

    }
    private void Start()
    {
        GameObject ui = GameObject.Find("Canvas");
        HealthUI other = (HealthUI)ui.GetComponent(typeof(HealthUI));
        // other.World();
        ui_2 = GameObject.FindGameObjectWithTag("UI").GetComponent<HealthUI>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && ui_2.enemiesCount == 0)
        {
            ui_2.World();
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);

        transisition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
    }
}
