using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsSystem : MonoBehaviour
{
    private List<Building> buildings = new();

    public int Build(GameObject prefab, Vector3 position)
    {
        GameObject gameObject = Instantiate(prefab);
        gameObject.transform.position = position;
        Building building  = gameObject.GetComponent<Building>();
        buildings.Add(building);

        return buildings.Count-1;
    }

    public Building GetBuilding(int idx)
    {
        return buildings[idx];
    }

    public void RemoveAt(int idx)
    {
        if (buildings.Count < idx ||
            buildings[idx] == null)
            return;

        Destroy(buildings[idx]);
        buildings[idx] = null;
    }
}
