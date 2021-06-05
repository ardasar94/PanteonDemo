using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;


    bool MouseOnObject = false;
    bool isBuildingSelected = false;

    public bool CreateTower(Building tower, Vector3 position)
    {
        Instantiate(tower.gameObject, position, Quaternion.identity);
        return true;
    }

    public int GetWidth() { return width;}
    public int GetHeight() { return height; }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0) && MouseOnObject)
        {
            isBuildingSelected = !isBuildingSelected;
        }
    }

    private void OnMouseEnter()
    {
        MouseOnObject = true;
    }

    private void OnMouseExit()
    {
        MouseOnObject = false;
    }


    public bool IsBuildingSelected() => isBuildingSelected;

    public void SetIsSelected(bool isSelect) { isBuildingSelected = isSelect; }
}
