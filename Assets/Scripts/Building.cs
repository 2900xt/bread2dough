using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int cost, buildingType;
    public const int TYPE_WHEAT_FARM = 1, TYPE_MILK_FARM = 2, TYPE_EGG_FARM = 3, TYPE_BREAD_FACTORY = 4;
    public float timer;
    public static float wheatGenTime = 15, eggGenTime = 5, milkGenTime = 5, breadGenTime = 5;
    public static GameManager gameMgr;
    void Start()
    {
        gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public float GetProgress()
    {
        float denominator = 0;
        timer -= Time.deltaTime;

        switch(buildingType)
        {
            case TYPE_WHEAT_FARM:
            {
                denominator = wheatGenTime;
                break;
            }
            case TYPE_MILK_FARM:
            {
                denominator = milkGenTime;
                break;
            }
            case TYPE_EGG_FARM:
            {
                denominator = eggGenTime;
                break;
            }
            case TYPE_BREAD_FACTORY:
            {
                denominator = breadGenTime;
                break;
            }
            default: break;
        }

        return (denominator - timer) / denominator;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        switch(buildingType)
        {
            case TYPE_WHEAT_FARM:
            {
                if(timer < 0)
                {
                    timer = wheatGenTime;
                    gameMgr.userWheat += 5;
                }
                break;
            }
            case TYPE_MILK_FARM:
            {
                if(timer < 0)
                {
                    timer = milkGenTime;
                    gameMgr.userMilk += 5;
                }
                break;
            }
            case TYPE_EGG_FARM:
            {
                if(timer < 0)
                {
                    timer = eggGenTime;
                    gameMgr.userEggs += 5;
                }
                break;
            }
            case TYPE_BREAD_FACTORY:
            {
                if(timer < 0)
                {
                    timer += Time.deltaTime;
                    if(gameMgr.userEggs >= 2 && gameMgr.userMilk >= 1 && gameMgr.userWheat >= 3)
                    {
                        timer = breadGenTime;
                        gameMgr.userBread++;

                        gameMgr.userEggs -= 2;
                        gameMgr.userMilk -= 1;
                        gameMgr.userWheat -= 3;
                    }
                }
                break;
            }
            default:
                break;
        }
    }
}
