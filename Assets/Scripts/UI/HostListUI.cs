using Assets.Scripts.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostListUI : MonoBehaviour
{
  [SerializeField] private Transform Contents;
  [SerializeField] private GameObject HostPrefab;
  private Broadcaster broadcaster;

  public HostListUI()
  {
    broadcaster = new Broadcaster();
    broadcaster.onLobiesUpdate += (lobbyData) =>
    {
      TMP_Text tmpText = HostPrefab.GetComponentInChildren<TMP_Text>();
      tmpText.text = lobbyData.name;

      Button tmpButton = HostPrefab.GetComponentInChildren<Button>();
      tmpButton.onClick.RemoveAllListeners();
      tmpButton.onClick.AddListener(() =>
      {
        Client client = new Client(lobbyData);
        client.onConnected += () => UiInteraction.ActivatePanel(GameObject.FindGameObjectsWithTag("Game")[0]);
        client.Start();
      });

      tmpButton.transform.SetParent(Contents);
      Instantiate(tmpButton);
    };
  }

  private void OnEnable()
  {
    Debug.Log("Host list enabled");
    broadcaster.ListenForBroadCast();
  }

  private void OnDisable()
  {
    Debug.Log("Host list disabled");
    broadcaster.CancelOperations();
    broadcaster.RefreshLobbies();
  }

  public void RefreshLobbies()
  {
    broadcaster.RefreshLobbies();
  }
}
