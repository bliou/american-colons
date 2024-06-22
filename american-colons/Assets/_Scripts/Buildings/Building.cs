using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public enum BuildingDir
{
    Down = 0,
    Right = 1,
    Top = 2,
    Left = 3,
}

public enum BuildingState
{
    Constructing = 0,
    Idle = 1,
    Working = 2,
    Destroying = 3,
} 

public class Building: PlacedObject
{
    private const string Fondation_Name = "Fondation";
    private const string Build_Name = "Build";

    // To detect redundant calls
    private bool _disposedValue;

    // a reference to the game object on the scene
    private GameObject gameObject;
    private BuildingModel model;
    private List<GameObject> buildSteps;

    // state of the building
    private BuildingState state;

    // list all the renderers of the child object
    private MeshRenderer[] childRenderers;
    private List<Color> childColors = new();

    public Building(GameObject gameObject, BuildingModel model, List<Cell> cells, Vector3Int gridPosition, Vector2Int size)
        : base(cells, gridPosition, size)
    {
        this.gameObject = gameObject;
        this.model = model;
        this.state = BuildingState.Constructing;

        // show the fondation game object
        gameObject.transform.Find(Fondation_Name).gameObject.SetActive(true);

        // add all the child game objects that start with Build
        for (int i = 0; i < gameObject.transform.childCount; i ++)
        {
            GameObject go = gameObject.transform.GetChild(i).gameObject;
            if (go.name.StartsWith(Build_Name))
                buildSteps.Add(go);
        }

        // get the child renderers (will be removed soon)
        childRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childRenderers)
        {
            childColors.Add(renderer.material.color);
        }
    }

    public override void Update(float deltaTime)
    {
        Debug.Log($"Update building: {this}");
    }

    public override string ToString()
    {
        return $"building [{gridPosition.x}; {gridPosition.y}] - unique ID: {UniqueId}";
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                if (gameObject != null)
                    GameSystem.Instance.DestroyGO(gameObject);
            }

            gameObject = null;
            _disposedValue = true;
        }

        base.Dispose(disposing);
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
