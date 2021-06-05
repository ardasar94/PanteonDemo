using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Building buildingPrefab;
    [SerializeField] GameObject NoPlace;
    [SerializeField] bool isPlaceable = true;

    ButtonController[] allButtons;
    ButtonController selectedButton;
    Building preBuild;
    List<Tile> noPlaceableTiles;
    Pathfinder pathfinder;
    Tile[] allTiles;
    bool isSelected = false;
    public bool IsPlaceable { get { return isPlaceable; } }

    private void Start()
    {
        allButtons = FindObjectsOfType<ButtonController>();
        pathfinder = FindObjectOfType<Pathfinder>();
        allTiles = FindObjectsOfType<Tile>();
    }

    private void Update()
    {
        CheckSelectedButton();
    }

    // check if any buttons are checked
    private void CheckSelectedButton()
    {
        foreach (ButtonController button in allButtons)
        {
            if (button.GetIsSeleceted())
            {
                selectedButton = button;
            }
        }
        if (allButtons.All(x => x.GetIsSeleceted() == false))
        {
            selectedButton = null;
        }
    }

    private void OnMouseOver()
    {
        // Turns off the preview when right clicked on tiles. If there is a selected soldier, moves the soldier to the selected tile.
        if (Input.GetMouseButtonDown(1))
        {
            ExitPlacePrefab();

            var selectedSoldiers = FindObjectsOfType<SoldierMover>().Where(x => x.GetIsSoldierSelected());

            if (selectedSoldiers != null)
            {
                foreach (var soldier in selectedSoldiers)
                {
                    Tile startTile = allTiles.FirstOrDefault(x => x.transform.position == new Vector3(Mathf.Round(soldier.transform.position.x), Mathf.Round(soldier.transform.position.y), transform.position.z));
                    soldier.MoveSoldier(pathfinder.FindPath(startTile, this));
                }
            }
        }

        // Soldier Multi Select start pos
        else if (Input.GetMouseButtonDown(0))
        {
            List<SoldierMover> soldiers = FindObjectsOfType<SoldierMover>().ToList();
            foreach (var item in soldiers)
            {
                item.SetStartMultiSelectVector(transform.position);
            }
        }
        // Soldier Multi Select end pos
        if (Input.GetMouseButtonUp(0))
        {
            List<SoldierMover> soldiers = FindObjectsOfType<SoldierMover>().ToList();
            foreach (var item in soldiers)
            {
                item.SetEndMultiSelectVector(transform.position);
            }
        }
    }

    private void OnMouseDown()
    {
        Vector3 instantiateSpot = new Vector3(0, 0, 0);
        Tile[] tiles = FindObjectsOfType<Tile>();
        // Find instantiate spot of the building when clicked on tiles
        if (buildingPrefab)
        {
            // We interfere the position because of to fit exactly the buildings to the tiles.
            instantiateSpot = new Vector3(
                buildingPrefab.GetWidth() % 2 == 0 ? transform.position.x - 0.5f : transform.position.x,
                buildingPrefab.GetHeight() % 2 == 0 ? transform.position.y - 0.5f : transform.position.y, transform.position.z - 1f);

        }

        // Checked is button selected, are tiles placable.
        // "isPlacable" in first if is unnecessary now. It can be removed optionally
        if (isPlaceable && isSelected && FindAllBuildingTiles().All(x => x.isPlaceable == true))
        {
            bool isPlaced = buildingPrefab.CreateTower(buildingPrefab, instantiateSpot);
            isPlaceable = !isPlaced;

            foreach (var item in tiles)
            {
                item.ChangeIsSelected(false);
            }
            foreach (Tile tile in FindAllBuildingTiles())
            {
                tile.isPlaceable = false;
            }
        }

        // if there are some unplacable tiles give a warning to the user. 
        else if (FindAllBuildingTiles().Any(x => x.isPlaceable == false && isSelected))
        {
            noPlaceableTiles = FindAllBuildingTiles().FindAll(x => x.isPlaceable == false);

            foreach (Tile tile in noPlaceableTiles)
            {
                var noPlaceBuilding = Instantiate(NoPlace, new Vector3(tile.transform.position.x, tile.transform.position.y, -2), Quaternion.identity);
                Destroy(noPlaceBuilding.gameObject, 0.5f);
            }
        }

        if (selectedButton != null)
        {
            selectedButton.SetIsSelected(false);
            Destroy(preBuild.gameObject);
            selectedButton = null;
        }
    }

    // Instantiate the preview of a building
    private void OnMouseEnter()
    {
        if (selectedButton)
        {
            preBuild = Instantiate(selectedButton.GetBuildingPrefab(), new Vector3(transform.position.x - 0.5f, transform.position.y - 0.5f, -2f), Quaternion.identity);
            if (preBuild.GetComponent<BoxCollider2D>() != null)
            {
                preBuild.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    // Destroy the preview if button is not selected or preview is null.
    private void OnMouseExit()
    {
        if (selectedButton && preBuild)
        {
            Destroy(preBuild.gameObject);
        }
    }

    public void SetBuilding(Building NewBuildingPrefab)
    {
        buildingPrefab = NewBuildingPrefab;
    }

    public void ChangeIsSelected(bool _isSelected)
    {
        isSelected = _isSelected;
    }

    // Find the other tiles that the building will hover over.
    private List<Tile> FindAllBuildingTiles()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        List<Tile> buildingsTile = new List<Tile>();
        if (buildingPrefab != null)
        {
            buildingsTile = tiles.Where(x =>
                        x.transform.position.x >= transform.position.x - (float)buildingPrefab.GetWidth() / 2 &&
                        x.transform.position.x <= transform.position.x + (float)buildingPrefab.GetWidth() / 2 &&
                        x.transform.position.y >= transform.position.y - (float)buildingPrefab.GetHeight() / 2 &&
                        x.transform.position.y <= transform.position.y + (float)buildingPrefab.GetHeight() / 2).ToList();
        }

        return buildingsTile;
    }

    void ExitPlacePrefab()
    {
        if (preBuild != null)
        {
            Destroy(preBuild.gameObject);
        }
        ChangeIsSelected(false);
        preBuild = null;
        selectedButton = null;
    }
}
