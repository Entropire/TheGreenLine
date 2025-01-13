using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.Networking
{
  internal class Broadcaster
  {
    public Action<LobbyData> onLobiesUpdate;

    private List<LobbyData> lobbies = new List<LobbyData>();
    private CancellationTokenSource cancellationTokenSource;
    private CancellationToken cancellationToken;
    private ushort port = 8000;

    public Broadcaster()
    {
      cancellationTokenSource = new CancellationTokenSource();
      cancellationToken = cancellationTokenSource.Token;
    }

    public void SetBroadcasterPort(ushort port)
    {
      this.port = port;
    }

    public async Task SendBroadCast(LobbyData lobbyData)
    {
      using (UdpClient udpClient = new UdpClient())
      using (cancellationToken.Register(() => udpClient.Dispose()))
      {
        while (true)
        {
          Debug.Log("broadcast send");

          if (cancellationToken.IsCancellationRequested)
          {
            break;
          }

          udpClient.EnableBroadcast = true;

          string message = JsonUtility.ToJson(lobbyData);
          byte[] data = Encoding.UTF8.GetBytes(message);

          IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, port);
          udpClient.Send(data, data.Length, endPoint);

          await Task.Delay(1000);
        }
      }
    }

    public void ListenForBroadCast()
    {
      Task.Run(ListenForBroadCastAsync);
    }

    public async Task ListenForBroadCastAsync()
    {
      using (UdpClient udpClient = new UdpClient(port))
      using (cancellationToken.Register(() => udpClient.Dispose()))
      {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);

        while (true)
        { 
          if (cancellationToken.IsCancellationRequested)
          {
            Debug.Log("Stopped Listening for broadcasts");
            break;
          }
          Debug.Log("Listening for broadcasts");
          try
          {
            byte[] data = udpClient.Receive(ref remoteEndPoint);
            string message = Encoding.UTF8.GetString(data);
            LobbyData lobbyData = JsonUtility.FromJson<LobbyData>(message);

            if (!lobbies.Contains(lobbyData))
            {
              Debug.Log("Found a broad cast");
              lobbies.Add(lobbyData);
              onLobiesUpdate?.Invoke(lobbyData);
            }
          }
          catch (SocketException ex)
          {
            if (cancellationToken.IsCancellationRequested)
            {
              Debug.Log("Receive interrupted due to cancellation.");
              break;
            }
            Debug.LogError($"SocketException: {ex.Message}");
          }
          await Task.Delay(100);
        }
      }

      Debug.Log("Stopped Listening for broadcasts");
    }
    public void CancelOperations()
    {
      cancellationTokenSource.Cancel();
    }

    public void RefreshLobbies()
    {
      lobbies = new List<LobbyData>();
    }

    public List<LobbyData> GetLobbies()
    {
      return lobbies;
    }
  }
}
