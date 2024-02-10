using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    public int userGold, userWheat;
    public TextMeshProUGUI goldText, wheatText;
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
                Instantiate(buildingToPlace, nearestTile.transform.position, Quaternion.identity);
                BoughtBuilding();
            }
        }
        //goldText.text = userGold + "";
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
