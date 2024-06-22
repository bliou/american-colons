using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingModels : ScriptableObject
{
    public List<BuildingModel> Models;
}

public enum BuildingDir
{
    Down = 0,
    Right = 1,
    Top = 2,
    Left = 3,
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
    public GameObject Preview { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set;}

    private BuildingDir direction;    

    public BuildingModel Clone()
    {
        return new BuildingModel
        {
            Name = Name,
            ID = ID,
            Size = Size,
            Preview = Preview,
            Prefab = Prefab,
            direction = BuildingDir.Down
        };
    }

    public void RotateLeft()
    {
        switch (direction)
        {
            case BuildingDir.Right:
                direction = BuildingDir.Top; break;
            case BuildingDir.Top:
                direction = BuildingDir.Left; break;
            case BuildingDir.Left:
                direction = BuildingDir.Down; break;
            case BuildingDir.Down:
                direction = BuildingDir.Right; break;
        }
    }

    public void RotateRight()
    {
        switch (direction)
        {
            case BuildingDir.Right:
                direction = BuildingDir.Down; break;
            case BuildingDir.Down:
                direction = BuildingDir.Left; break;
            case BuildingDir.Left:
                direction = BuildingDir.Top; break;
            case BuildingDir.Top:
                direction = BuildingDir.Right; break;
        }
    }

    public float GetRotationAngle()
    {
        switch (direction)
        {
            case BuildingDir.Down:
                return 0;
            case BuildingDir.Right:
                return 90;
            case BuildingDir.Top:
                return 180;
            case BuildingDir.Left:
                return 270;
        }

        return 0;
    }

    public Vector3 GetRotationOffset()
    {
        switch (direction)
        {
            case BuildingDir.Down:
                return Vector3.zero;
            case BuildingDir.Right:
                return new Vector3(0, 0, Size.x);
            case BuildingDir.Top:
                return new Vector3(Size.x, 0, Size.y);
            case BuildingDir.Left:
                return new Vector3(Size.y, 0, 0);
        }

        return Vector3.zero;
    }

    public Vector2Int GetSize()
    {
        switch (direction)
        {
            case BuildingDir.Left:
            case BuildingDir.Right:
                return new Vector2Int(Size.y, Size.x);

            case BuildingDir.Top:
            case BuildingDir.Down:
                return Size;
        }

        return Vector2Int.zero;
    }
}