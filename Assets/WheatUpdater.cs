using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatUpdater : MonoBehaviour
{
    public SpriteRenderer rend;
    public Sprite stage1, stage2, stage3;
    void Update()
    {
        float prog = GetComponent<Building>().GetProgress();
        if(prog > 0.8)
        {
            rend.sprite = stage3;
        }
        else if(prog > 0.4)
        {
            rend.sprite = stage2;
        }
        else 
        {
            rend.sprite = stage1;
        }
    }
}
