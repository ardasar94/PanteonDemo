using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] Building building;

    Tile[] tiles;
    bool isSelected = false;

    // Update is called once per frame
    void Update()
    {
        ExitPlacePrefab();
    }


    public void SetBuildingsOnWaypoints()
    {
        ButtonController[] buttons = FindObjectsOfType<ButtonController>();
        foreach (ButtonController button in buttons)
        {
            if (button.gameObject != this.gameObject)
            {
                button.SetIsSelected(false);
            }
            else
            {
                button.SetIsSelected(true);
            }
        }
        //isSelected = !isSelected;
        tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            tile.SetBuilding(building);
            tile.ChangeIsSelected(isSelected);
        }
    }

    public void SetIsSelected(bool a)
    {
        isSelected = a;
    }

    public bool GetIsSeleceted() => isSelected;

    public Building GetBuildingPrefab() => building;

    void ExitPlacePrefab()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetIsSelected(false);
        }
    }

}
