using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    GameObject infoScreen;

    bool shouldShown = false;

    void Start()
    {
        infoScreen = GameObject.FindGameObjectWithTag("InfoScreen");
    }

    void Update()
    {
        infoScreen.transform.GetChild(0).gameObject.SetActive(shouldShown);
    }

    private void OnMouseDown()
    {
        bool IsBuildingSelected = FindObjectsOfType<Building>().Any(x => x.IsBuildingSelected());
        shouldShown = IsBuildingSelected;
    }
}
