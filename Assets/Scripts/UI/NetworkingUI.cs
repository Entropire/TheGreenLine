using System;
using System.Net.Sockets;
using System.Net;

using Assets.Scripts.Networking;
using TMPro;
using UnityEngine;
using System.Threading;

namespace Assets.Scripts.UI
{
  internal class NetworkingUI : MonoBehaviour
  {
    public static NetworkingUI instance;

    [SerializeField] private TMP_Text WaitingLobbyIpText;
    [SerializeField] private GameObject gameUI;
    public TcpConnection tcpConnection;

    private void Start()
    {
      if (instance == null)
      {
        instance = this;
      }
    }

    public void HostLobby(TMP_Dropdown dropdown)
    {
      int selectedIndex = dropdown.value;
      string selectedOptions = dropdown.options[selectedIndex].text;
      LobbyData lobbyData = new LobbyData(IPAddress.Parse(selectedOptions));
      Debug.Log(lobbyData);
      Host host = new Host(lobbyData);
      host.onMessage += (msg) => Debug.Log(msg);
      host.onError += (msg) => Debug.LogError(msg);

      SynchronizationContext mainThreadContext = SynchronizationContext.Current;
      host.onConnected += () =>
      {
        mainThreadContext?.Post(_ =>
        {
          UiInteraction.ActivatePanel(gameUI);
        }, null);
      };
      host.Start();
      tcpConnection = host;
      WaitingLobbyIpText.text = lobbyData.ip.ToString();
    }

    public void StopTcpConnection()
    {
      tcpConnection.Stop();
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
      tcpConnection = client;
    }
  }
}
