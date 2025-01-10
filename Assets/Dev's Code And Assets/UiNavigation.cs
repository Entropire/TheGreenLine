using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UiNavigation : MonoBehaviour
{
    // Code made by Patryk.
    // This code will control buttons that modify UIs. This means that this will switch between UIs and Panels.
    // Mostly public values for interchangeability.

    public void ToggleUI()
    {
        GameObject uiParent = gameObject; // Defines the UIparent being the model the script is under.

        if (uiParent == null) // If uiParent is undefined or null
        {
            Debug.Log("Error - Gameobject uiParent Undefined");
            return; // Return if the uiParent is null to restart the function.
        }

        uiParent.SetActive(!uiParent.activeInHierarchy); //Toggle the uiParent between its active and inactive state.
    }
}
