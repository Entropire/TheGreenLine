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
    public Action<List<LobbyData>> onLobiesUpdate;

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
      {
        while (true)
        {
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

    public async Task ListenForBroadCast()
    {
      using (UdpClient udpClient = new UdpClient(port))
      {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);

        while (true)
        {
          if (cancellationToken.IsCancellationRequested)
          {
            break;
          }

          byte[] data = udpClient.Receive(ref remoteEndPoint);
          string message = Encoding.UTF8.GetString(data);
          LobbyData lobbyData = JsonUtility.FromJson<LobbyData>(message);

          if (!lobbies.Contains(lobbyData))
          {
            lobbies.Add(lobbyData);
            onLobiesUpdate?.Invoke(lobbies);
          }

          await Task.Delay(100);
        }
      }
    }

    public void CancelOperations()
    {
      cancellationTokenSource.Cancel();
    }
  }
}
