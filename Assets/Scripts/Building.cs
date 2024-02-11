using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int cost, buildingType;
    public const int TYPE_WHEAT_FARM = 1, TYPE_MILK_FARM = 2, TYPE_EGG_FARM = 3, TYPE_BREAD_FACTORY = 4;
    public const int TYPE_NEB_FACTORY = 5, TYPE_SOL_FACTORY = 6, TYPE_SHINY_FACTORY = 7, TYPE_TACHYON_FACTORY = 8;
    public const int TYPE_GOLD_MINE = 9, TYPE_SOLAR_PANEL = 10, TYPE_METEOR_MINE = 11, TYPE_ACCELERATOR = 12;
    public float timer;
    public static float wheatGenTime = 15, eggGenTime = 5, milkGenTime = 5, breadGenTime = 5, resourceGenTime = 5;
    public static GameManager gameMgr;
    public Sprite sprite;
    void Start()
    {
        gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
        wheatGenTime = 15.0f / gameMgr.prestiegeLevel;
        eggGenTime = 15.0f / gameMgr.prestiegeLevel;
        milkGenTime = 15.0f / gameMgr.prestiegeLevel;
        breadGenTime = 15.0f / gameMgr.prestiegeLevel;
        resourceGenTime = 15.0f / gameMgr.prestiegeLevel;
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
            case TYPE_NEB_FACTORY:
            case TYPE_SHINY_FACTORY:
            case TYPE_SOL_FACTORY:
            {
                denominator = breadGenTime;
                break;
            }
            case TYPE_GOLD_MINE:
            case TYPE_METEOR_MINE:
            case TYPE_SOLAR_PANEL:
            case TYPE_ACCELERATOR:
            {
                denominator = resourceGenTime;
                break;
            }
            default: break;
        }

        return Mathf.Min((denominator - timer) / denominator, 1f);
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
            case TYPE_GOLD_MINE:
            {
                if(timer < 0)
                {
                    timer = resourceGenTime;
                    gameMgr.userGold += 5;
                }
                break;
            }
            case TYPE_SOLAR_PANEL:
            {
                if(timer < 0)
                {
                    timer = resourceGenTime;
                    gameMgr.lightJars += 5;
                }
                break;
            }
            case TYPE_METEOR_MINE:
            {
                if(timer < 0)
                {
                    timer = resourceGenTime;
                    gameMgr.cosmicShards += 5;
                }
                break;
            }
            case TYPE_ACCELERATOR:
            {
                if(timer < 0)
                {
                    timer = resourceGenTime;
                    gameMgr.tachyonParticle += 5;
                }
                break;
            }
            case TYPE_BREAD_FACTORY:
            {
                if(timer < 0)
                {
                    timer += Time.deltaTime;
                    if(gameMgr.userEggs >= 1 && gameMgr.userMilk >= 1 && gameMgr.userWheat >= 1)
                    {
                        timer = breadGenTime;
                        gameMgr.userBread++;

                        gameMgr.userEggs -= 1;
                        gameMgr.userMilk -= 1;
                        gameMgr.userWheat -= 1;
                        if(gameMgr.prestiegeLevel == 1)
                        {
                            gameMgr.userPrestiegeCount++;
                        }
                    }
                }
                break;
            }
            case TYPE_SHINY_FACTORY:
            {
                if(timer < 0)
                {
                    timer += Time.deltaTime;
                    if(gameMgr.userBread >= 3 && gameMgr.userGold >= 1)
                    {
                        timer = breadGenTime;
                        gameMgr.userShinyBread++;
                        
                        gameMgr.userGold -= 1;
                        gameMgr.userBread -= 3;
                        if(gameMgr.prestiegeLevel == 2)
                        {
                            gameMgr.userPrestiegeCount++;
                        }
                    }
                }
                break;
            }
            case TYPE_SOL_FACTORY:
            {
                if(timer < 0)
                {
                    timer += Time.deltaTime;
                    if(gameMgr.userShinyBread >= 3 && gameMgr.lightJars >= 1)
                    {
                        timer = breadGenTime;
                        gameMgr.userSolarBread++;

                        gameMgr.lightJars -= 1;
                        gameMgr.userShinyBread -= 3;
                        if(gameMgr.prestiegeLevel == 3)
                        {
                            gameMgr.userPrestiegeCount++;
                        }
                    }
                }
                break;
            }
            case TYPE_NEB_FACTORY:
            {
                if(timer < 0)
                {
                    timer += Time.deltaTime;
                    if(gameMgr.userSolarBread >= 3 && gameMgr.cosmicShards >= 1)
                    {
                        timer = breadGenTime;
                        gameMgr.userNebulaBread++;
                        gameMgr.cosmicShards -= 1;

                        gameMgr.userSolarBread -= 3;
                        if(gameMgr.prestiegeLevel == 4)
                        {
                            gameMgr.userPrestiegeCount++;
                        }
                    }
                }
                break;
            }
            case TYPE_TACHYON_FACTORY:
            {
                if(timer < 0)
                {
                    timer += Time.deltaTime;
                    if(gameMgr.userNebulaBread >= 3 && gameMgr.tachyonParticle >= 1)
                    {
                        timer = breadGenTime;
                        gameMgr.userTachyonBread++;

                        gameMgr.userNebulaBread -= 3;
                        gameMgr.tachyonParticle -= 1;
                        if(gameMgr.prestiegeLevel == 5)
                        {
                            gameMgr.userPrestiegeCount++;
                        }
                    }
                }
                break;
            }
            default:
                break;
        }
    }
}
