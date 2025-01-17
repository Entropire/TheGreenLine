using UnityEngine;

public class UiInteraction : MonoBehaviour
{
  [SerializeField] private static GameObject activePanel;
  [SerializeField] private static GameObject activeGamePanel;
  [SerializeField] private GameObject chatBtn;
  [SerializeField] private GameObject chat;

  public static void ActivatePanel(GameObject panel)
  {
    activePanel?.SetActive(false);
    panel.SetActive(true);
    activePanel = panel;
  }

  public static void ActivateGamePanel(GameObject panel)
  {
    activeGamePanel?.SetActive(false);
    panel.SetActive(true);
    activeGamePanel = panel;
  }

  public void SetChatActive(bool active)
  {
    chatBtn?.SetActive(!active);
    chat.SetActive(active);
  }

  public void ExitProgram()
  {
    Application.Quit();
  }
}
