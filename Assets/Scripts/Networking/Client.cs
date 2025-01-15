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
      onMessage?.Invoke("client: Starting new thread for client!");
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

          if (cancellationToken.IsCancellationRequested)
          {
            return;
          }

          onMessage?.Invoke($"client: Connecting to {lobbyData.ip}:{lobbyData.port}");
          onConnected?.Invoke();

          //SendPacket(PacketType.ChatMessage, "You are connected to the client!");
          await ListenForPackets();
        }
      }
      catch (SocketException ex)
      {
        onError?.Invoke($"Unable to connect: {ex.Message}");
      }
      catch (ArgumentException ex)
      {
        onError?.Invoke($"Invalid IP or port: {ex.Message}");
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

