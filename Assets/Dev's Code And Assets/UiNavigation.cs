using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UiNavigation : MonoBehaviour
{
    // Code made by Patryk.
    // This code will control buttons that modify UIs and scenes. This means that this will switch between scenes and or open panels.
    // Mostly public values for interchangeability.

    private void Start()
    {

    }
    public void ButtonFunct(bool Buttonversion)
    {
        GameObject Panel = gameObject;
        if (Buttonversion == true) //Checks if the button is either used for scenechanges or gameobject management.
        {
            if (Panel.activeInHierarchy == false)
            {
                Panel.SetActive(true);
            }
            else if (Panel.activeInHierarchy == true)
            {
                Panel.SetActive(false);
            }
            else
            {
                Debug.Log("Unexpected Error in Panelctrl True. Else called, undefined values?");
                Panel.SetActive(false); //If error encountered, set panel to false to restart function.
            }
        }
        else if (Buttonversion == false) //Loads the scene. Displays in Print how many scenes loaded for debugging.
        {
            SceneManager.LoadScene("TestSceneX");
            print("Scenes Loaded = " + SceneManager.loadedSceneCount);
        }
        else
        {
            Debug.Log("Error, undefined values and or undefined bool in ButtonFunct");
            Buttonversion = true; //If error encountered, set function to true, announce error.
        }
    }
}
