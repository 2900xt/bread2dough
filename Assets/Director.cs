using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Director : MonoBehaviour
{

    public PlayableDirector dir;

    public PlayableAsset[] cutscenes; 

    void PlayCutscene(int scene)
    {
        dir.playableAsset = cutscenes[scene];
        dir.Play();
    }
}
