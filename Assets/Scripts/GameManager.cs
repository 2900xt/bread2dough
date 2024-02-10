using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    public int userGold, userWheat, userEggs, userMilk, userBread;
    public TextMeshProUGUI goldText, wheatText, eggText, milkText, breadText;
    public CustomCursor buildingCursor;
    public GameObject gridParent, tilePrefab;
    public Building buildingToPlace;
    public List<Tile> tiles;
    public int tileSize, tileStartX, tileStartY, gridLength;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && buildingToPlace != null)
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

            if(!nearestTile.isOccupied)
            {
                Instantiate(buildingToPlace, nearestTile.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
                nearestTile.isOccupied = true;
                BoughtBuilding();
            }
        }

        /*
        goldText.text = formatText(userGold);
        breadText.text = formatText(userBread);
        milkText.text = formatText(userMilk);
        eggText.text = formatText(userEggs);
        wheatText.text = formatText(userWheat);
        */
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
        int endX = tileStartX + tileSize * gridLength;
        int endY = tileStartY + tileSize * gridLength;
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
        gridParent.SetActive(true);
        buildingCursor.gameObject.SetActive(true);
        buildingCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
        Cursor.visible = false;
    }

    public void BoughtBuilding()
    {
        buildingToPlace = null;
        gridParent.SetActive(false);
        buildingCursor.gameObject.SetActive(false);
        Cursor.visible = true;
    }
}
