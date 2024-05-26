using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedBuilding : MonoBehaviour
{
    // selected game object
    [SerializeField] private GameObject selected;

    public void ShowSelected()
    {
        selected.SetActive(true);
    }

    public void HideSelected()
    {
        selected.SetActive(false);
    }
}
