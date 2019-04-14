using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum MusicState
{
    Start,
    Battle,
    Enemy,
    End
}

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip startMenu;
    [SerializeField] AudioClip battleScene;
    //[SerializeField] AudioClip enemyTurn;
    //[SerializeField] AudioClip gameEnd;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            PlayMusic(MusicState.Start);
        }
        else
        {
            PlayMusic(MusicState.Battle);
        }

        foreach (Animator anim in FindObjectsOfType<Animator>())
        {
            if (anim.name == "Music")
            {
                animator = anim;
            }
        }
    }

    public void PlayMusic(MusicState audioState)
    {
        AudioSource audio = GetComponent<AudioSource>();
        AudioClip startingClip = audio.clip;
        switch (audioState)
        {
            case MusicState.Start:
                Debug.Log(MusicState.Start);
                audio.clip = startMenu;
                break;
            case MusicState.Battle:
                Debug.Log(MusicState.Battle);
                audio.clip = battleScene;
                break;
            //case MusicState.Enemy:
            //    Debug.Log(MusicState.Enemy);
            //    audio.clip = enemyTurn;
            //    break;
            //case MusicState.End:
            //    Debug.Log(MusicState.End);
            //    audio.clip = gameEnd;
            //    break;
        }
        audio.Play();
    }

    public void FadeOutMusic()
    {
        animator.SetBool("MusicFadeOut", true);
    }
}
