using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Building : MonoBehaviour
{
    // list all the renderers of the child object
    private MeshRenderer[] childRenderers;
    private List<Color> childColors = new();

    public int UniqueId { get; private set; }
    private static int uniqueId = 0;

    private void Start()
    {
        childRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childRenderers)
        {
            childColors.Add(renderer.material.color);
        }
        UniqueId = uniqueId++;
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
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
