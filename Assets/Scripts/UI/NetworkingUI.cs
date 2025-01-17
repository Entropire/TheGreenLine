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
    [SerializeField] private GameObject game;

    public void HostLobby(TMP_Dropdown dropdown)
    {
      int selectedIndex = dropdown.value;
      string selectedOptions = dropdown.options[selectedIndex].text;
      LobbyData lobbyData = new LobbyData(IPAddress.Parse(selectedOptions));

      Host host = new Host(lobbyData);
      PacketHandler.playerCity = "Amsterdam";

      SynchronizationContext mainThreadContext = SynchronizationContext.Current;
      host.onConnected += () =>
      {
        mainThreadContext?.Post(_ =>
        {
          UiInteraction.ActivatePanel(game);

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
      PacketHandler.playerCity = "Stockholm";

      SynchronizationContext mainThreadContext = SynchronizationContext.Current;
      client.onConnected += () =>
      {
        mainThreadContext?.Post(_ =>
        {
          UiInteraction.ActivatePanel(game);
        }, null);
      };

      client.Start();
      PacketHandler.SetTcpConnection(client);
    }
  }
}
