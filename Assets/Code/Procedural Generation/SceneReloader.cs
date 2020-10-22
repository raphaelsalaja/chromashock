using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public Animator transisition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadNextLevel();
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