using System;
using System.Net.Sockets;
using System.Threading;

namespace Assets.Scripts.Networking
{
  internal class Client : TcpConnection
  {
    private LobbyData lobbyData;

    public Client(LobbyData lobbyData)
    {
      this.lobbyData = lobbyData;
    }

    public override void Start()
    {
      Thread thread = new Thread(StartAsync);
      thread.Start();
    }

    private async void StartAsync()
    {
      try
      {
        using (cancellationToken.Register(() => client?.Dispose()))
        {
          client = new TcpClient();
          client.Connect(lobbyData.ip, lobbyData.port);
          stream = client.GetStream();

          if (client.Connected)
          {
            onConnected?.Invoke();
          }

          if (cancellationToken.IsCancellationRequested)
          {
            return;
          }

          SendPacket(PacketType.ChatMessage, "You are connected to the client!");
          await ListenForPackets();
        }
      }
      catch (Exception ex)
      {
        onError?.Invoke(ex.Message);
      }
      finally
      {
        client?.Dispose();
      }
    }
  }
}

