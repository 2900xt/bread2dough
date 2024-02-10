using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    public int userGold, userWheat, userEggs, userMilk, userBread, userShinyBread, userSolarBread, userNebulaBread, userTachyonBread, userPrestiegePoints;
    public TextMeshProUGUI goldText, wheatText, eggText, milkText, breadText, shinyBreadText, solarBreadText, nebulaBreadText, tachyonBreadText;
    public TextMeshProUGUI wheatSell, milkSell, eggSell, breadSell, shinySell, solarSell, nebulaSell, tachyonSell;
    public CustomCursor buildingCursor, progressCursor;
    public GameObject gridParent, tilePrefab, progBar;
    public Building buildingToPlace;
    public List<Tile> tiles;
    public int tileSize, tileStartX, tileStartY, gridXLength, gridYLength;
    public int prestiege;
    public int getSellMultiplier(int amount)
    {
        return Mathf.Max(1, amount / 10);
    }
    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tile nearestTile = getClosestTile(mousePosition);

        if(nearestTile != null)
        {
            if(Input.GetMouseButtonDown(0) && buildingToPlace != null && !nearestTile.isOccupied)
            {
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
        
        goldText.text = formatText(userGold);
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
            int intNum = (int)(num * 10) / 10;
            return (num) + "e" + (int)log;
        }
        return "" + num;
    }

    public void Prestiege()
    {
        PlayerPrefs.SetInt("Prestiege", prestiege + 1);
    }

    private void Start()
    {
        prestiege = PlayerPrefs.GetInt("Prestiege");
        if(prestiege >= 2)
        {
            
        }
        
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
        if(userGold < building.cost)
        {
            return;
        }
        
        userGold -= building.cost;
        buildingToPlace = building;
        buildingCursor.gameObject.SetActive(true);
        buildingCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
        Cursor.visible = false;
        Tile.placing = true;
    }

    public void BoughtBuilding()
    {
        buildingToPlace = null;
        buildingCursor.gameObject.SetActive(false);
        Cursor.visible = true;
        Tile.placing = false;
    }

    public void SellWheat()
    {
        int amount = getSellMultiplier(userWheat);
        userWheat -= amount;
        userGold += amount;
    }

    public void SellMilk()
    {
        int amount = getSellMultiplier(userMilk);
        userMilk -= amount;
        userGold += amount;
    }

    public void SellEggs()
    {
        int amount = getSellMultiplier(userEggs);
        userEggs -= amount;
        userGold += amount;
    }
    public void SellBread()
    {
        int amount = getSellMultiplier(userBread);
        userBread -= amount;
        userGold += amount * 10;
    }
    public void SellShiny()
    {
        int amount = getSellMultiplier(userShinyBread);
        userShinyBread -= amount;
        userGold += amount * 100;
    }

    public void SellSolar()
    {
        int amount = getSellMultiplier(userSolarBread);
        userSolarBread -= amount;
        userGold += amount * 1000;
    }

    public void SellNebula()
    {
        int amount = getSellMultiplier(userNebulaBread);
        userNebulaBread -= amount;
        userGold += amount * 10000;
    }

    public void SellTachyon()
    {
        int amount = getSellMultiplier(userTachyonBread);
        userTachyonBread -= amount;
        userGold += amount * 100000;
    }
}
