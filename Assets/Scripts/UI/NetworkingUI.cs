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
    [SerializeField] TMP_Text WaitingLobbyIpText;
    [SerializeField] GameObject gameUI;
    TcpConnection tcpConnection;

    public void HostLobby(TMP_Dropdown dropdown)
    {
      LobbyData lobbyData = new LobbyData(IPAddress.Parse(""));
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
