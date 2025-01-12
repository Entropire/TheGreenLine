using UnityEngine;

public class UiInteraction : MonoBehaviour
{
    [SerializeField] private static GameObject activePanel;

    public static void ActivatePanel(GameObject panel)
    {
        activePanel?.SetActive(false);
        panel.SetActive(true);
        activePanel = panel;
    }

    public void ExitProgram()
    {
        Debug.Log("program exited");
    }
}
