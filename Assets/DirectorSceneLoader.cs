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

    void OnEnable()
    {
        if(cutscene){director.stopped += OnPlayableDirectorStopped;}
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if(director == aDirector){
            PlayerPrefs.SetInt("Presteige", PresteigetoLoad);
            SceneManager.LoadScene(2);
        }
    }

    void OnDisable()
    {
        if(cutscene){director.stopped -= OnPlayableDirectorStopped;}
    }

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
