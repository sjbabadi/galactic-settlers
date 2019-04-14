using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {

        foreach (Animator anim in FindObjectsOfType<Animator>())
        {
            if (anim.name == "Image")
            {
                anim.SetBool("StartFadeOut", true);
            }
        }
        FindObjectOfType<MusicController>().FadeOutMusic();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
