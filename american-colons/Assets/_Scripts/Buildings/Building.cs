using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;


public class Building: PlacedObject
{
    private const string Fondation_Tag = "Fondation";


    // a reference to the game object on the scene
    GameObject gameObject;

    // list all the renderers of the child object
    private MeshRenderer[] childRenderers;
    private List<Color> childColors = new();

    // the fondation of the building
    private GameObject fondation;

    public Building(GameObject gameObject, List<Cell> cells, Vector3Int gridPosition, Vector2Int size)
        : base(cells, gridPosition, size)
    {
        this.gameObject = gameObject;
        
        childRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childRenderers)
        {
            childColors.Add(renderer.material.color);
        }

        fondation = gameObject.transform.Find(Fondation_Tag).gameObject;
    }

    public override string ToString()
    {
        return $"building [{gridPosition.x}; {gridPosition.y}] - unique ID: {UniqueId}";
    }

    public override void RemovePlacedObject()
    {
        base.RemovePlacedObject();
        EndConstruction();
        GameSystem.Instance.DestroyGO(gameObject);
    }

    public void StartConstruction()
    {
        fondation.SetActive(true);
    }

    private void EndConstruction()
    {
        fondation.SetActive(false);
    }

    public void ShowSelection()
    {
        foreach (MeshRenderer renderer in childRenderers)
        {
            renderer.material.color = Color.black; 
        }
    }

    public void HideSelection()
    {
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.color = childColors[i];
        }
    }
}
