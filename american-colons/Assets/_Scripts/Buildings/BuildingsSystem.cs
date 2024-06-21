using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsSystem : MonoBehaviour
{
    private List<Building> buildings = new();

    public int Build(GameObject prefab, Vector3 position, float angle)
    {
        GameObject gameObject = Instantiate(prefab);
        gameObject.transform.position = position;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        Building building  = gameObject.GetComponent<Building>();
        building.StartConstruction();
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
