using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void PlayGame() {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        
        FindObjectOfType<Animator>().SetBool("StartMenuFade", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
