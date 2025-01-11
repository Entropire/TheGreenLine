using UnityEngine;

public class UiNavigation : MonoBehaviour
{
    // Code made by Patryk.
    // This code will control buttons that modify UIs. This means that this will switch between UIs and Panels.
    // Mostly public values for interchangeability.
    
    
    public void ToggleUI(GameObject uiParent)
    {
        uiParent.SetActive(!uiParent.activeInHierarchy); //Toggle the uiParent between its active and inactive state.
    }
}
