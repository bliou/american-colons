using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;
using MEC;

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

    private const string Coroutine_Tag = "constructBuilding";

    // To detect redundant calls
    private bool _disposedValue;

    // a reference to the game object on the scene
    private GameObject gameObject;
    private BuildingModel model;
    private List<GameObject> buildSteps = new();

    // state of the building
    private BuildingState state;
    private int currentBuildStep;
    // temp variable that will later be replaced with resources
    // and effort amount used to build the step
    private float currentBuildStepTime;

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

        // Start the construction of the building in a dedicated coroutine
        Timing.RunCoroutine(_ConstructBuilding(), Coroutine_Tag);

        // throw an exception if the build steps found in the prefab
        // do not match the build steps from the model
        if (buildSteps.Count != model.BuildSteps.Count)
            throw new Exception($"prefab build steps {buildSteps.Count} do not match model build steps {model.BuildSteps.Count}");

        // get the child renderers (will be removed soon)
        childRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childRenderers)
        {
            childColors.Add(renderer.material.color);
        }
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

                Timing.KillCoroutines(Coroutine_Tag);
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

    private IEnumerator<float> _ConstructBuilding()
    {
        while (currentBuildStep < model.BuildSteps.Count)
        {
            BuildStep buildStep = model.BuildSteps[currentBuildStep];
            currentBuildStepTime += Timing.DeltaTime;
            if (currentBuildStepTime > buildStep.ConstructionTime)
            {
                currentBuildStepTime = 0f;
                MoveToNextStep();
            }
            yield return Timing.WaitForOneFrame;
        }
    }


    // MoveToNextStep will hide the current build step
    // and show the next build step.
    // If we are at the last step, switch the building state
    // to idle and stop showing the fondation
    private void MoveToNextStep()
    {
        if (currentBuildStep > 0)
        {
            buildSteps[currentBuildStep-1].SetActive(false);
        }
        buildSteps[currentBuildStep].SetActive(true);
        currentBuildStep++;

        if (currentBuildStep >= model.BuildSteps.Count)
        {
            state = BuildingState.Idle;
            gameObject.transform.Find(Fondation_Name).gameObject.SetActive(false);
            return;
        }
    }
}
