using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsSystem : MonoBehaviour
{
    private Dictionary<int, Building> buildings = new();

    public void Build(GameObject prefab, PlacedObject placedObject, Vector3 position, float angle)
    {
        GameObject gameObject = Instantiate(prefab);
        gameObject.transform.position = position;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        Building building  = gameObject.GetComponent<Building>();
        building.SetPlacedObject(placedObject);
        building.StartConstruction();
        buildings.Add(placedObject.UniqueId, building);
    }

    public Building GetBuilding(int uniqueId)
    {
        return buildings[uniqueId];
    }

    public void RemoveBuilding(int uniqueId)
    {
        if (!buildings.ContainsKey(uniqueId))
            return;

        Destroy(buildings[uniqueId]);
        buildings[uniqueId] = null;
    }
}
