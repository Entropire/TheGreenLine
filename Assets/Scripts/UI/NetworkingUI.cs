using System.Net;

using Assets.Scripts.Networking;
using TMPro;
using UnityEngine;
using System.Threading;

namespace Assets.Scripts.UI
{
  internal class NetworkingUI : MonoBehaviour
  {
    [SerializeField] private TMP_Text WaitingLobbyIpText;
    [SerializeField] private GameObject gameUI;

    public void HostLobby(TMP_Dropdown dropdown)
    {
      int selectedIndex = dropdown.value;
      string selectedOptions = dropdown.options[selectedIndex].text;
      LobbyData lobbyData = new LobbyData(IPAddress.Parse(selectedOptions));

      Host host = new Host(lobbyData);

      SynchronizationContext mainThreadContext = SynchronizationContext.Current;
      host.onConnected += () =>
      {
        mainThreadContext?.Post(_ =>
        {
          UiInteraction.ActivatePanel(gameUI);
        }, null);
      };

      host.Start();
      PacketHandler.SetTcpConnection(host);
      WaitingLobbyIpText.text = lobbyData.ip.ToString();
    }

    public void StopTcpConnection()
    {
      PacketHandler.StopTcpConnection();
    } 

    public void JoinLobby(TMP_InputField lobbyIP)
    {
      LobbyData lobbyData = new LobbyData(IPAddress.Parse(lobbyIP.text));
      Client client = new Client(lobbyData);

      SynchronizationContext mainThreadContext = SynchronizationContext.Current;
      mainThreadContext?.Post(_ =>
      {
        UiInteraction.ActivatePanel(gameUI);
      }, null);

      client.Start();
      PacketHandler.SetTcpConnection(client);
    }
  }
}
