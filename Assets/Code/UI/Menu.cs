using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator transisition;

    public void PlayGame()
    {
        StartCoroutine(LoadLevel(5));
    }
    public void Endless()
    {
        StartCoroutine(LoadLevel(7));
    }
    public void GoNext()
    {
        StartCoroutine(LoadLevel(6));
    }

    public void Story()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void Controls()
    {
        StartCoroutine(LoadLevel(2));
    }

    public void GoBack()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void Die()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void Win()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);

        yield return new WaitForSeconds(1);

        transisition.SetTrigger("Start");
    }
}
