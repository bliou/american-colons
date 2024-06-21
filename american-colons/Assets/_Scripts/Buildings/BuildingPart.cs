using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPart : MonoBehaviour
{
    public event EventHandler OnLastPartBuilt;

    [SerializeField] private bool isLastPart;
    [SerializeField] private BuildingPart nextPart;
    [SerializeField] private float constructionTime;
    [SerializeField] private Material buildingMaterial;
    
    private bool partBeingBuilt = false;
    private float constructionDuration = 0f;

    private Material material;

    private void Start()
    {
        gameObject.SetActive(false);
        material = GetComponent<Renderer>().materials[0];
    }

    private void Update()
    {
        UpdatePartConstruction();
    }

    private void UpdatePartConstruction()
    {
        // if the part is not under construction, do nothing
        if (!partBeingBuilt)
            return;


        // otherwise, wait consruction time before showing it entirely
        constructionDuration += Time.deltaTime;

        if (constructionDuration >= constructionTime)
        {
            EndConstruction();
            return;
        }

        Color color = material.color;
        color.a = constructionDuration / constructionTime;
        material.color = color;
    }

    public void EnableConstruction()
    {
        partBeingBuilt = true;
        gameObject.SetActive(true);
    }

    private void EndConstruction()
    {
        Color color = material.color;
        color.a = 1.0f;
        material.color = color;
        if (isLastPart)
        {
            OnLastPartBuilt?.Invoke(this, EventArgs.Empty);
            return;
        }

        nextPart.EnableConstruction();
        partBeingBuilt = false;

        return;
    }
}
