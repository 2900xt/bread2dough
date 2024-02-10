using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public GameObject pivot;
    public void SetProgress(float val)
    {
        pivot.transform.localScale = new Vector3(val, 1, 1);
    }
}
