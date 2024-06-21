using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fondation : MonoBehaviour
{
    [SerializeField] private BuildingPart firstPart;
    [SerializeField] private BuildingPart lastPart;

    // just a small delay time before starting the construction
    // TODO: to be removed later on
    [SerializeField] private float delayTime;

    private float delayDuration = 0f;
    private bool fondationReady = false;

    private void Start()
    {
        gameObject.SetActive(true);
        lastPart.OnLastPartBuilt += LastPart_OnLastPartBuilt;
    }

    private void LastPart_OnLastPartBuilt(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // only do the update when the fondation is not ready to 
        // being built
        if (fondationReady)
            return;

        delayDuration += Time.deltaTime;

        if (delayDuration > delayTime)
        {
            firstPart.EnableConstruction();
            fondationReady = true;
        }
    }
}
