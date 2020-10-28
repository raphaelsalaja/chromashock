using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public Animator transisition;
    public HealthUI ui;
    private void Start()
    {
        GameObject ui = GameObject.Find("Canvas");
        HealthUI other = (HealthUI)ui.GetComponent(typeof(HealthUI));
        other.World();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadNextLevel();
            ui.World();
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);

        yield return new WaitForSeconds(1); transisition.SetTrigger("Start");

    }
}