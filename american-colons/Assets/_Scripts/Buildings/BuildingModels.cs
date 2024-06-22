using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingModels : ScriptableObject
{
    public List<BuildingModel> Models;
}

[Serializable]
public class BuildingModel
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set;}

    [field: SerializeField]
    public List<BuildingStep> BuildingSteps { get ; private set; }
}

// BuildingStep represent the step a building must take to be built
[Serializable]
public class BuildingStep
{
    [field: SerializeField]
    public float ConstructionTime { get; private set; }

    // TODO: add the necessary resources for this part and the number of worker required
}