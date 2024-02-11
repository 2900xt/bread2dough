using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class GameManager : MonoBehaviour
{
    [Header ("Integers")]
    public int userCoins, userWheat, userEggs, userMilk, userBread, userShinyBread, userSolarBread, userNebulaBread, userTachyonBread, userPrestiegeCount;
    public int userGold, lightJars, cosmicShards, tachyonParticle;
    [Header ("Text References")]
    public TextMeshProUGUI goldText, wheatText, eggText, milkText, breadText, shinyBreadText, solarBreadText, nebulaBreadText, tachyonBreadText, prestiegePercent;
    public TextMeshProUGUI goldBarText, lightText, cosmicDust, tachyonParticleText;
    public TextMeshProUGUI wheatSell, milkSell, eggSell, breadSell, shinySell, solarSell, nebulaSell, tachyonSell;
    [Header ("References")]
    public CustomCursor buildingCursor, progressCursor;
    public GameObject gridParent, tilePrefab, progBar;
    public GameObject pShiny, pSolar, pNebula, pTachyon, pShiny2, pSolar2, pNebula2, pTachyon2, pShiny3, pSolar3, pNebula3, pTachyon3, pShiny4, pSolar4, pNebula4, pTachyon4;
    public Building buildingToPlace;
    public List<Tile> tiles;
    public int tileSize, tileStartX, tileStartY, gridXLength, gridYLength;
    public GameObject popup;
    [Header ("Prestige")]
    public int prestiegeLevel, prestiegeCountNeeded;
    public Slider prestiegeProgressBar;
    public GameObject prestiegeButton;
    public GameObject shop, ui;
    public int getSellMultiplier(int amount)
    {
        return Mathf.Max(1, amount / 10);
    }

    public void PlayClick()
    {
        GetComponent<AudioSource>().Play();
    }
    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tile nearestTile = getClosestTile(mousePosition);

        if(nearestTile != null)
        {
            if(Input.GetKeyDown(KeyCode.Escape) && buildingToPlace != null)
            {
                buildingToPlace = null;
                buildingCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                Tile.placing = false;
            }
            else if(Input.GetMouseButtonDown(0) && buildingToPlace != null && !nearestTile.isOccupied)
            {
                PlayClick();
                Building bd = Instantiate(buildingToPlace, nearestTile.transform.position + new Vector3(0, 0, 1), Quaternion.identity).GetComponent<Building>();
                nearestTile.isOccupied = true;
                nearestTile.building = bd;
                BoughtBuilding();
            }
            else if(nearestTile.isOccupied && buildingToPlace == null)
            {
                progBar.SetActive(true);
                float prog = nearestTile.building.GetProgress();
                progBar.GetComponent<ProgressBar>().SetProgress(prog);
                progBar.transform.position = new Vector3(mousePosition.x, mousePosition.y + 2, -3);
            }
            else
            {
                progBar.SetActive(false);
            }
        }
        else 
        {
            progBar.SetActive(false);
        }
        
        goldText.text = formatText(userCoins);
        breadText.text = formatText(userBread);
        milkText.text = formatText(userMilk);
        eggText.text = formatText(userEggs);
        wheatText.text = formatText(userWheat);
        shinyBreadText.text = formatText(userShinyBread);
        solarBreadText.text = formatText(userSolarBread);
        nebulaBreadText.text = formatText(userNebulaBread);
        tachyonBreadText.text = formatText(userTachyonBread);
        
        wheatSell.text = "Sell " + formatText(getSellMultiplier(userWheat)) + "x";
        milkSell.text = "Sell " + formatText(getSellMultiplier(userMilk)) + "x";
        eggSell.text = "Sell " + formatText(getSellMultiplier(userEggs)) + "x";
        breadSell.text = "Sell " + formatText(getSellMultiplier(userBread)) + "x";
        shinySell.text = "Sell " + formatText(getSellMultiplier(userShinyBread)) + "x";
        solarSell.text = "Sell " + formatText(getSellMultiplier(userSolarBread)) + "x";
        nebulaSell.text = "Sell " + formatText(getSellMultiplier(userNebulaBread)) + "x";
        tachyonSell.text = "Sell " + formatText(getSellMultiplier(userTachyonBread)) + "x";

        wheatSell.gameObject.transform.parent.gameObject.SetActive(userWheat != 0);
        milkSell.gameObject.transform.parent.gameObject.SetActive(userMilk != 0);
        eggSell.gameObject.transform.parent.gameObject.SetActive(userEggs != 0);
        breadSell.gameObject.transform.parent.gameObject.SetActive(userBread != 0);
        shinySell.gameObject.transform.parent.gameObject.SetActive(userShinyBread != 0);
        nebulaSell.gameObject.transform.parent.gameObject.SetActive(userNebulaBread != 0);
        tachyonSell.gameObject.transform.parent.gameObject.SetActive(userTachyonBread != 0);
        solarSell.gameObject.transform.parent.gameObject.SetActive(userSolarBread != 0);

        goldBarText.text = formatText(userGold);
        lightText.text = formatText(lightJars);
        cosmicDust.text = formatText(cosmicShards);
        tachyonParticleText.text = formatText(tachyonParticle);

        float val = (float)userPrestiegeCount / prestiegeCountNeeded;
        val = Mathf.Min(val, 1f);

        prestiegePercent.text = ((int)(val * 100)) + "%";
        prestiegeProgressBar.value = val;

        prestiegeButton.SetActive(val == 1f);
    }

    public Tile getClosestTile(Vector2 pos)
    {
        Tile nearestTile = null;
        float nearestDist = float.MaxValue;
        foreach(Tile tile in tiles)
        {
            float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(dist < nearestDist)
            {
                nearestDist = dist;
                nearestTile = tile;
            }
        }

        if(nearestDist > tileSize) return null;

        return nearestTile;
    }

    private string formatText(int num)
    {
        if(num > 1000)
        {
            float log = Mathf.Log10(num);
            int intNum = (int)(num / Mathf.Pow(10, log));
            return (num) + "e" + (int)log;
        }
        return "" + num;
    }

    public void DoPrestiege()
    {
        prestiegeLevel++;
        if(prestiegeLevel == 6)
        {
            GameObject.Find("Fader").GetComponent<DirectorSceneLoader>().LoadScene(7);
            return;
        }
         GameObject.Find("Fader").GetComponent<DirectorSceneLoader>().LoadScene(prestiegeLevel+1);
    }

    public void OpenShop()
    {
        PlayClick();
        shop.SetActive(true);
        ui.SetActive(false);
    }

    public void CloseShop()
    {
        PlayClick();
        shop.SetActive(false);
        ui.SetActive(true);
    }

    private void Start()
    {
        prestiegeLevel = PlayerPrefs.GetInt("Prestiege");
        if(prestiegeLevel >= 2)
        {
            pShiny.SetActive(false);
            pShiny2.SetActive(false);
            pShiny3.SetActive(false);
            pShiny4.SetActive(false);
        }
        if(prestiegeLevel >= 3)
        {
            pSolar.SetActive(false);
            pSolar2.SetActive(false);
            pSolar3.SetActive(false);
            pSolar4.SetActive(false);
        }
        if(prestiegeLevel >= 4)
        {
            pNebula.SetActive(false);
            pNebula2.SetActive(false);
            pNebula3.SetActive(false);
            pNebula4.SetActive(false);
        }
        if(prestiegeLevel >= 5)
        {
            pTachyon.SetActive(false);
            pTachyon2.SetActive(false);
            pTachyon3.SetActive(false);
            pTachyon4.SetActive(false);
        }

        prestiegeCountNeeded = (6 - prestiegeLevel) * 30;
        
        SpawnTiles();
    }

    public void SpawnTiles()
    {
        int endX = tileStartX + tileSize * gridXLength;
        int endY = tileStartY + tileSize * gridYLength;
        for(int x = tileStartX; x < endX; x += tileSize)
        {
            for(int y = tileStartY; y < endY; y += tileSize)
            {
                Transform t = Instantiate(tilePrefab, new Vector3(x, y, -1), Quaternion.identity).transform;
                t.parent = gridParent.transform;
                tiles.Add(t.gameObject.GetComponent<Tile>());
            }
        }
    }

    public void BuyBuilding(Building building)
    {
        PlayClick();
        if(userCoins < building.cost)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            popupText("Insufficient Funds", mousePosition, Color.red);
            return;
        }
        
        buildingToPlace = building;
        buildingCursor.gameObject.SetActive(true);
        buildingCursor.GetComponent<SpriteRenderer>().sprite = building.sprite;
        Cursor.visible = false;
        Tile.placing = true;
    }

    public void BoughtBuilding()
    {
        userCoins -= buildingToPlace.cost;
        buildingToPlace = null;
        buildingCursor.gameObject.SetActive(false);
        Cursor.visible = true;
        Tile.placing = false;
    }

    public void SellWheat()
    {
        PlayClick();
        int amount = getSellMultiplier(userWheat);
        userWheat -= amount;
        userCoins += amount;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupText("+ $" + (amount), mousePosition, new Color(0f, 1f, 0f, 1f));
    }

    public void SellMilk()
    {
        PlayClick();
        int amount = getSellMultiplier(userMilk);
        userMilk -= amount;
        userCoins += amount * 3;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupText("+ $" + (amount * 3), mousePosition, new Color(0f, 1f, 0f, 1f));
    }

    public void SellEggs()
    {
        PlayClick();
        int amount = getSellMultiplier(userEggs);
        userEggs -= amount;
        userCoins += amount * 3;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupText("+ $" + (amount * 3), mousePosition, new Color(0f, 1f, 0f, 1f));
    }
    public void SellBread()
    {
        PlayClick();
        int amount = getSellMultiplier(userBread);
        userBread -= amount;
        userCoins += amount * 8;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupText("+ $" + (amount * 8), mousePosition, new Color(0f, 1f, 0f, 1f));
    }
    public void SellShiny()
    {
        PlayClick();
        int amount = getSellMultiplier(userShinyBread);
        userShinyBread -= amount;
        userCoins += amount * 20;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupText("+ $" + (amount * 20), mousePosition, new Color(0f, 1f, 0f, 1f));
    }

    public void SellSolar()
    {
        PlayClick();
        int amount = getSellMultiplier(userSolarBread);
        userSolarBread -= amount;
        userCoins += amount * 65;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupText("+ $" + (amount * 65), mousePosition, new Color(0f, 1f, 0f, 1f));
    }

    public void SellNebula()
    {
        PlayClick();
        int amount = getSellMultiplier(userNebulaBread);
        userNebulaBread -= amount;
        userCoins += amount * 175;
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupText("+ $" + (amount * 175), mousePosition, new Color(0f, 1f, 0f, 1f));
    }

    public void SellTachyon()
    {
        PlayClick();
        int amount = getSellMultiplier(userTachyonBread);
        userTachyonBread -= amount;
        userCoins += amount * 450;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupText("+ $" + (amount * 450), mousePosition, new Color(0f, 1f, 0f, 1f));
    }

    public void popupText(string s, Vector2 v, Color c)
    {
        TextMeshPro g = Instantiate(popup, new Vector3(v.x, v.y, -5), Quaternion.identity).transform.GetChild(0).GetComponent<TextMeshPro>();
        g.color = c;
        g.text = s;
    }
}
