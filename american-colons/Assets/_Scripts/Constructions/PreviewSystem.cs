using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private bool showCellIndicator = false;

    // for placement
    private GameObject previewObject;
    private Vector2Int previewSize;

    // for removal
    private PlacedObject highlightPlacedObject;

    [SerializeField]
    private Material previewMaterialPrefab;

    public void StartPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        previewSize = size;
    }

    public void StopPlacementPreview()
    {
        Destroy(previewObject);
    }

    public void RotatePreview(float angle)
    {
        previewObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    public void UpdatePlacementPosition(Vector3 position, bool isPlacementValid)
    {
        Color c = isPlacementValid ? Color.green : Color.red;
        c.a = 0.5f;
        previewMaterialPrefab.color = c;
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);

        DrawCellIndicator(position, isPlacementValid);
    }

    public void HighlightPlacedObject(PlacedObject placedObject)
    {
        if (placedObject == null)
        {
            if (highlightPlacedObject != null)
                highlightPlacedObject.StopHighlighting();

            highlightPlacedObject = null;
            return;
        }
        placedObject.Highlight();
        if (highlightPlacedObject != null && highlightPlacedObject.UniqueId != placedObject.UniqueId)
            highlightPlacedObject.StopHighlighting();

        highlightPlacedObject = placedObject;
    }

    private void DrawCellIndicator(Vector3 position, bool isPlacementValid)
    {
        if (!showCellIndicator)
            return;

        Vector3 topLeftCorner = position;
        topLeftCorner.y = 0.1f;
        Vector3 topRightCorner = new Vector3(topLeftCorner.x + previewSize.x, topLeftCorner.y, topLeftCorner.z);
        Vector3 bottomRightCorner = new Vector3(topLeftCorner.x + previewSize.x, topLeftCorner.y, topLeftCorner.z + previewSize.y);
        Vector3 bottomLeftCorner = new Vector3(topLeftCorner.x, topLeftCorner.y, topLeftCorner.z + previewSize.y);

        Color color = isPlacementValid ? Color.green : Color.red;

        Debug.DrawLine(topLeftCorner, topRightCorner, color);
        Debug.DrawLine(topLeftCorner, bottomLeftCorner, color);
        Debug.DrawLine(topRightCorner, bottomRightCorner, color);
        Debug.DrawLine(bottomLeftCorner, bottomRightCorner, color);
    }

}
