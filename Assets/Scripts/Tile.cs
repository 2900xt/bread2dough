using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public bool isOccupied;
    public Building building;
    public Color availableColor, unavailiableColor;
    public SpriteRenderer rend;
    private void Update()
    {
        if(isOccupied)
        {
            rend.color = unavailiableColor;
        }
        else 
        {
            rend.color = availableColor;
        }
    }
}
