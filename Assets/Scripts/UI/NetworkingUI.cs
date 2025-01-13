using System;
using System.Net.Sockets;
using System.Net;

using Assets.Scripts.Networking;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
  internal class NetworkingUI : MonoBehaviour
  {
    TcpConnection tcpConnection;

    public void HostLobby(TMP_InputField lobbyName)
    {
      LobbyData lobbyData = new LobbyData(lobbyName.text, GetLocalIPAddress());
      Host host = new Host(lobbyData);
      //host.onMessage += (msg) => Debug.Log(msg);
      host.onConnected += () =>
      {
        UiInteraction.ActivatePanel(GameObject.FindGameObjectsWithTag("Game")[0]);
      };
      host.Start();
      tcpConnection = host;
    }

    public void StopHostingLobby()
    {
      tcpConnection.Stop();
    }

    public void GetOpenLobbies()
    {

    }

    public void JoinLobby()
    {

    }

    private IPAddress GetLocalIPAddress()
    {
      try
      {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
          if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4
          {
            return ip;
          }
        }
        throw new Exception("No IPv4 address found!");
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error: " + ex.Message);
      }

      return default;
    }
  }
}
