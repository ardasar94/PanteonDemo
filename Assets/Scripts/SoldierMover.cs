using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMover : MonoBehaviour
{
    bool isSoldierSelected = false;
    bool MouseOnObject = false;

    Vector3 startMultiSelectVector;
    Vector3 endMultiSelectVector;

    private void Update()
    {
        SelectAndUnselectSoldier();
        IsSoldierInMultiSelect();
    }

    // Check if any soldier in multiselect area. If there is, checked solider isSoldierSelected var true; 
    private void IsSoldierInMultiSelect()
    {
        if (startMultiSelectVector != null && endMultiSelectVector != null)
        {
            float minX = startMultiSelectVector.x < endMultiSelectVector.x ? startMultiSelectVector.x : endMultiSelectVector.x;
            float maxX = startMultiSelectVector.x > endMultiSelectVector.x ? startMultiSelectVector.x : endMultiSelectVector.x;
            float minY = startMultiSelectVector.y < endMultiSelectVector.y ? startMultiSelectVector.y : endMultiSelectVector.y;
            float maxY = startMultiSelectVector.y > endMultiSelectVector.y ? startMultiSelectVector.y : endMultiSelectVector.y;


            if (transform.position.x >= minX && transform.position.x <= maxX && transform.position.y >= minY && transform.position.y <= maxY)
            {
                isSoldierSelected = true;
            }
        }
    }

    // If player click to the soldier, set soldier selected. Else set soldier selected to false;
    private void SelectAndUnselectSoldier()
    {
        if (Input.GetMouseButton(0) && MouseOnObject)
        {
            isSoldierSelected = true;
        }
        else if (Input.GetMouseButton(0) && !MouseOnObject)
        {
            isSoldierSelected = false;
        }
    }

    // Start soldier move corotine but firstly stop all coroutines to prevent bugs.
    public void MoveSoldier(List<Tile> path)
    {
        StopAllCoroutines();
        StartCoroutine(MoveSoldierCoroutine(path));
    }

    // Move soldiers throughout to the tiles given by pathfinding scrip.t
    private IEnumerator MoveSoldierCoroutine(List<Tile> path)
    {
        Vector3 endPosition;
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            endPosition = new Vector3(path[i].transform.position.x, path[i].transform.position.y, -1);
            float travelPercent = 0f;
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * 4;  // speed is 4. It can be made a Seriliaze var.
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public bool GetIsSoldierSelected() => isSoldierSelected;

    //Check is mouse on the object.
    private void OnMouseEnter()
    {
        MouseOnObject = true;
    }

    private void OnMouseExit()
    {
        MouseOnObject = false;
    }

    public void SetStartMultiSelectVector(Vector3 startVector)
    {
        startMultiSelectVector = startVector;
    }

    public void SetEndMultiSelectVector(Vector3 endVector)
    {
        endMultiSelectVector = endVector;
    }

}
