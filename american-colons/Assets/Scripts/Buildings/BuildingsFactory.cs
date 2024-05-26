using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsFactory : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buildings = new();

    public int Build(GameObject prefab, Vector3 position)
    {
        GameObject building = Instantiate(prefab);
        building.transform.position = position;
        buildings.Add(building);

        return buildings.Count-1;
    }
}
