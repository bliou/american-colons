using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : IDisposable
{
    private const string Preview_Tag = "Preview";
    private float previewYOffset = 0.06f;

    // To detect redundant calls
    private bool _disposedValue;

    private GameObject gameObject;
    private Vector2Int size;
    private BuildingDir direction;
    private Renderer[] previewRenderers;

    public BuildingPreview(BuildingModel model)
    {
        size = model.Size;
        gameObject = GameSystem.Instance.InstantiateGO(model.Prefab);
        gameObject.SetActive(true);

        // disable all the child of the game object
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        // only enable the preview game object
        GameObject previewObject = gameObject.transform.Find(Preview_Tag).gameObject;
        previewObject.SetActive(true);
        previewRenderers = previewObject.GetComponentsInChildren<Renderer>();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                if (gameObject != null)
                    GameSystem.Instance.DestroyGO(gameObject);
            }

            gameObject = null;
            previewRenderers = null;
            _disposedValue = true;
        }
    }

    // return the previewed game object and set it to null
    // so the buildingPreview can be garbage collected
    public GameObject DumpPreview()
    {
        GameObject dump = gameObject;
        // while dumping the game object, stop showing the preview
        gameObject.transform.Find(Preview_Tag).gameObject.SetActive(false);
        gameObject = null;

        return dump;
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

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, GetRotationAngle(), 0));
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

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, GetRotationAngle(), 0));
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
                return new Vector3(0, 0, size.x);
            case BuildingDir.Top:
                return new Vector3(size.x, 0, size.y);
            case BuildingDir.Left:
                return new Vector3(size.y, 0, 0);
        }

        return Vector3.zero;
    }

    public Vector2Int GetSize()
    {
        switch (direction)
        {
            case BuildingDir.Left:
            case BuildingDir.Right:
                return new Vector2Int(size.y, size.x);

            case BuildingDir.Top:
            case BuildingDir.Down:
                return size;
        }

        return Vector2Int.zero;
    }

    public void UpdatePlacementPosition(Vector3 position, bool isPlacementValid)
    {
        Color c = isPlacementValid ? Color.green : Color.red;
        c.a = 0.5f;

        foreach (var renderer in previewRenderers)
        {
            foreach (var material in renderer.materials)
            {
                material.color = c;
            }
        }

        gameObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }
}
