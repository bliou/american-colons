using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid<TGridObject>
{
    public event EventHandler<OnGridValueChangedArgs> OnGridValueChanged;
    public class OnGridValueChangedArgs : EventArgs
    {
        public int x;
        public int y;
        public TGridObject newValue;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;
    private TGridObject[,] gridArray;

    public CustomGrid(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
    }

    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetCoordinatesFromWorldPosition(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetCoordinatesFromWorldPosition(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    private TGridObject GetValue(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogFormat("out of range index: [{0}; {1}]", x, y);
            return default;
        }
        return gridArray[x, y];
    }

    private void SetValue(int x, int y, TGridObject value)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogFormat("out of range index: [{0}; {1}]", x, y);
            return;
        }

        gridArray[x, y] = value;
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedArgs {
            x = x,
            y = y,
            newValue = value
        });
    }

    private void GetCoordinatesFromWorldPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return (new Vector3(x, y)+origin) * cellSize;
    }
}
