using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class DirectorSceneLoader : MonoBehaviour
{

    public bool cutscene = false;
    public int PresteigetoLoad;
    public PlayableDirector director;

    public Animator faderAnim;
    public GameObject fader;

    public int lvlToLoad;


    void OnEnable()
    {
        if(cutscene){director.stopped += OnPlayableDirectorStopped;}
        fader =  GameObject.Find("Fader");
        faderAnim = fader.transform.GetChild(0).GetComponent<Animator>();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if(director == aDirector){
            PlayerPrefs.SetInt("Prestiege", PresteigetoLoad);
            SceneManager.LoadScene(2);
        }
    }

    void OnDisable()
    {
        if(cutscene){director.stopped -= OnPlayableDirectorStopped;}
    }

    public void LoadScene(int i)
    {
        faderAnim.SetTrigger("FadeOut");
        lvlToLoad = i;
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(lvlToLoad);
    }
}
