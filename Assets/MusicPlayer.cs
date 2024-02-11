using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioManager audioMgr;
    void Start()
    {
        audioMgr.Play("bg_music");
    }
}
