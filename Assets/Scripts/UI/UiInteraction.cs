using UnityEngine;

public class UiInteraction : MonoBehaviour
{
    //Code originally written by Patryk, tweaked & rewritten by Quinten.

    [SerializeField] private static GameObject activePanel;

    public static void ActivatePanel(GameObject panel)
    {
        activePanel?.SetActive(false);
        panel.SetActive(true);
        activePanel = panel;
    }

    public void ExitProgram()
    {
        Application.Quit();
    }
}
