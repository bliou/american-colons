using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Building : MonoBehaviour
{
    // list all the renderers of the child object
    private MeshRenderer[] childRenderers;
    private List<Color> childColors = new();

    // the fondation of the building
    [SerializeField] private GameObject fondation;

    // reference to the related placed object
    private PlacedObject placedObject;

    private void Start()
    {
        childRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childRenderers)
        {
            childColors.Add(renderer.material.color);
        }
    }

    private void Update()
    {
        
    }

    public void SetPlacedObject(PlacedObject placedObject)
    {
        this.placedObject = placedObject;
        this.placedObject.OnHighlight += ShowSelection;
        this.placedObject.OnStopHighlighting += HideSelection;
    }

    public void StartConstruction()
    {
        fondation.SetActive(true);
    }

    private void EndConstruction()
    {
        fondation.SetActive(false);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
        this.placedObject.OnHighlight -= ShowSelection;
        this.placedObject.OnStopHighlighting -= HideSelection;
    }

    private void ShowSelection(object sender, EventArgs e)
    {
        foreach (MeshRenderer renderer in childRenderers)
        {
            renderer.material.color = Color.black; 
        }
    }

    private void HideSelection(object sender, EventArgs e)
    {
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.color = childColors[i];
        }
    }
}
