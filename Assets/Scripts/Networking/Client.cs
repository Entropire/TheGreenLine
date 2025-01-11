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

    public override void Start(CancellationToken ct)
    {
      Thread thread = new Thread(() => StartAsync(ct));
      onMessage?.Invoke("client: Starting new thread for client!");
      thread.Start();
    }

    private async void StartAsync(CancellationToken ct)
    {
      try
      {
        client = new TcpClient();
        client.Connect(lobbyData.ip, lobbyData.port);
        stream = client.GetStream();

        onMessage?.Invoke($"client: Connecting to {lobbyData.ip}:{lobbyData.port}");
        onConnected?.Invoke();

        SendPacket(PacketType.ChatMessage, "You are connected to the client!");
        await ListenForPackets(ct);
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

