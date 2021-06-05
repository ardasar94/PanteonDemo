using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Tile[] AllTiles;
    bool isEnd = false;

    void Start()
    {
        AllTiles = FindObjectsOfType<Tile>(); 
    }

    // return tiles path for soldier
    public List<Tile> FindPath(Tile startTile, Tile endTile)
    {
        List<Tile> neighborTiles = new List<Tile>();
        List<Tile> checkedTiles = new List<Tile>();
        int z = 0;
        checkedTiles.Add(startTile);
        neighborTiles = FindNeighbors(startTile);
        Tile minDistanceTile = null;

        // Finds the closest neighbor tiles to the end tiles untill find the end tile and store them in a list.
        while (!isEnd)
        {
            neighborTiles = neighborTiles.Where(x => x.IsPlaceable == true).ToList();

            foreach (Tile tile in neighborTiles)
            {
                if (minDistanceTile == null)
                {
                    minDistanceTile = tile;
                }
                else
                {
                    minDistanceTile = neighborTiles.Aggregate((x, y) => Vector3.Distance(x.transform.position, endTile.transform.position) < Vector3.Distance(y.transform.position, endTile.transform.position) ? x : y);
                }
            }
            neighborTiles.Remove(minDistanceTile);
            checkedTiles.Add(minDistanceTile);
            List<Tile> newNeighbors = FindNeighbors(minDistanceTile);

            for (int i = 0; i < newNeighbors.Count; i++)
            {
                if (checkedTiles.Contains(newNeighbors[i]))
                {
                    newNeighbors.Remove(newNeighbors[i]);
                }
            }

            for (int i = 0; i < neighborTiles.Count; i++)
            {
                if (checkedTiles.Contains(neighborTiles[i]))
                {
                    newNeighbors.Remove(neighborTiles[i]);
                }
            }

            neighborTiles.AddRange(newNeighbors);

            foreach (Tile tile in neighborTiles)
            {
                if (tile == endTile)
                {
                    isEnd = true;
                }
            }

            z++;
        }
        checkedTiles.Add(endTile);
        isEnd = false;
        return checkedTiles;
    }


    // Finds all neighbor tiles of given tile.
    private List<Tile> FindNeighbors(Tile mainTile)
    {
        List<Tile> tileNeighbors = new List<Tile>();
        foreach (Tile tile in AllTiles)
        {
            if ((tile.transform.position.x + 1 == mainTile.transform.position.x && tile.transform.position.y + 1 == mainTile.transform.position.y) ||
                (tile.transform.position.x + 1 == mainTile.transform.position.x && tile.transform.position.y == mainTile.transform.position.y) ||
                (tile.transform.position.x + 1 == mainTile.transform.position.x && tile.transform.position.y - 1 == mainTile.transform.position.y) ||
                (tile.transform.position.x - 1 == mainTile.transform.position.x && tile.transform.position.y + 1 == mainTile.transform.position.y) ||
                (tile.transform.position.x + 0 == mainTile.transform.position.x && tile.transform.position.y + 1 == mainTile.transform.position.y) ||
                (tile.transform.position.x - 1 == mainTile.transform.position.x && tile.transform.position.y + 0 == mainTile.transform.position.y) ||
                (tile.transform.position.x - 1 == mainTile.transform.position.x && tile.transform.position.y - 1 == mainTile.transform.position.y) ||
                (tile.transform.position.x + 0 == mainTile.transform.position.x && tile.transform.position.y - 1 == mainTile.transform.position.y))
            {
                tileNeighbors.Add(tile);
            }
        }

        return tileNeighbors;
    }
}
