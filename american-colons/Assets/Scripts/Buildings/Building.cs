using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Building
{
    // actual game object
    private GameObject gameObject;

    private SelectedBuilding selectedBuilding;

    public int UniqueId {get; private set;}
    public static int currentUniqueID = 0;

    public Building(GameObject gameObject, Vector3 position)
    {
        this.gameObject = gameObject;
        this.selectedBuilding = gameObject.GetComponentInChildren<SelectedBuilding>();
        gameObject.transform.position = position;
        UniqueId = currentUniqueID++;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void ShowSelected()
    {
        selectedBuilding.ShowSelected();
    }

    public void HideSelected()
    {
        selectedBuilding.HideSelected();
    }
}
