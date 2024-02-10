using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public static bool placing = false;
    public bool isOccupied;
    public Building building;
    public Color normalColor, placedColor;
    public SpriteRenderer rend;
    private void Update()
    {
        if(isOccupied)
        {
            rend.color = placedColor;
        }
        else rend.color = normalColor;
    }
}
