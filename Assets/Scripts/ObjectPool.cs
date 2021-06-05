using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] int poolSize = 5;
    // Start is called before the first frame update

    GameObject[] pool;

    private void Awake()
    {
        PopulatePool();
    }

    // Create soldiers in object pool
    private void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(soldierPrefab, transform);
            pool[i].SetActive(false);
        }
    }


    // Get soldiers from object pool and set its activeness and position.
    public void GetSoldierFromPool()
    {
        Building selectedBuilding = FindObjectsOfType<Building>().FirstOrDefault(x => x.IsBuildingSelected());
        if (selectedBuilding != null)
        {
            var objectPool = this.gameObject;
            List<GameObject> allChilds = new List<GameObject>();
            foreach (Transform child in objectPool.transform)
            {
                allChilds.Add(child.gameObject);
            }


            // Check are there any unspawned soldier in object pool
            // if not, instante new soldiers in object pool.
            GameObject soldier = allChilds.FirstOrDefault(x => x.activeInHierarchy == false);

            if (soldier != null)
            {
                soldier.transform.position = new Vector3(selectedBuilding.transform.position.x - 4f, selectedBuilding.transform.position.y, -1f);

                soldier.SetActive(true);
            }
            else
            {
                PopulatePool();
                GetSoldierFromPool();
            }
        }

    }
}
