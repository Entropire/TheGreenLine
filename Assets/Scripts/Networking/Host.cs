using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.Networking
{
  internal class Host : TcpConnection
  {
    private TcpListener listener;
    private LobbyData lobbyData;

    public Host(LobbyData lobbyData)
    {
      this.lobbyData = lobbyData;
    }

    public override void Start()
    {
      Thread thread = new Thread(() => StartAsync());
      onMessage?.Invoke("host: Starting new thread for host!");
      thread.Start();
    }

    private async void StartAsync()
    {
      try
      {
        using (cancellationToken.Register(() => listener?.Stop()))
        {
          listener = new TcpListener(lobbyData.ip, lobbyData.port);
          listener.Start();
          onMessage?.Invoke($"host: Host started");

          await ListenForClient();

          if (cancellationToken.IsCancellationRequested)
          {
            return; 
          }

          if (client != null)
          {
            onConnected?.Invoke();
          }

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
        onMessage?.Invoke("Host: Host stopped");
        client?.Dispose();
        listener?.Stop();
      }
    }

    private async Task ListenForClient()
    {
      onMessage?.Invoke("host: Listening for clients");

      try
      {
        TcpClient client = await listener.AcceptTcpClientAsync();
        this.client = client;
        this.stream = client.GetStream();

        onMessage?.Invoke("host: Client connected.");

        SendPacket(PacketType.ChatMessage, "You are connected to the server!");
      }
      catch (OperationCanceledException)
      {
        onError?.Invoke("Client connection attempt canceled.");
      }
      catch (Exception ex)
      {
        onError?.Invoke($"Error while listening for client: {ex.Message}");
      }
      finally
      {
        onMessage?.Invoke("Stopped listening for clients");
      }
    }
  }
}





