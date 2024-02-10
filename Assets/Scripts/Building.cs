using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int cost, buildingType;
    public const int TYPE_WHEAT_FARM = 1;

    void Update()
    {
        switch(buildingType)
        {
            case TYPE_WHEAT_FARM:
            {
                break;
            }
            default:
                break;
        }
    }
}
