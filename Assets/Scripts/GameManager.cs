using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    public int userGold, userWheat, userEggs, userMilk, userBread, userShinyBread, userSolarBread, userNebulaBread, userCosmicBread, userTachyonBread;
    public TextMeshProUGUI goldText, wheatText, eggText, milkText, breadText;
    public CustomCursor buildingCursor, progressCursor;
    public GameObject gridParent, tilePrefab, progBar;
    public Building buildingToPlace;
    public List<Tile> tiles;
    public int tileSize, tileStartX, tileStartY, gridXLength, gridYLength;
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
        if(num > 100)
        {
            float log = Mathf.Log10(num);
            return (num/log) + "e" + (int)log;
        }
        return "" + num;
    }

    private void Start()
    {
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
}
