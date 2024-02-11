using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float after;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, after);
    }
}
