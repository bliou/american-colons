using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingDatabaseSO : ScriptableObject
{
    public List<BuildingData> Buildings;
}

[Serializable]
public class BuildingData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField] 
    public GameObject Preview { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set;}
}