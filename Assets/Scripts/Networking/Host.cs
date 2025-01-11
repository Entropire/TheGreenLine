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

    public override void Start(CancellationToken cancellationToken = default)
    {
      Thread thread = new Thread(() => StartAsync(cancellationToken));
      onMessage?.Invoke("host: Starting new thread for host!");
      thread.Start();
    }

    private async void StartAsync(CancellationToken cancellationToken)
    {

      try
      {
        listener = new TcpListener(lobbyData.ip, lobbyData.port);
        onMessage?.Invoke($"host: Listener started");
        listener.Start();

        Broadcaster broadcaster = new Broadcaster();
        Task.WaitAll(ListenForClient(cancellationToken, broadcaster), broadcaster.SendBroadCast(lobbyData));

        if (client != null)
        {
          onConnected?.Invoke();
        }

        await ListenForPackets(cancellationToken);
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
        client.Dispose();
        listener.Stop();
      }
    }

    private async Task ListenForClient(CancellationToken ct, Broadcaster broadcaster)
    {
      onMessage?.Invoke("host: Listening for clients");
      try
      {
        TcpClient client = await listener.AcceptTcpClientAsync();
        this.client = client;
        this.stream = client.GetStream();

        SendPacket(PacketType.ChatMessage, "You are connected to the server!");
      }
      catch (OperationCanceledException)
      {
        onError?.Invoke("Client connection attempt canceled.");
      }

      listener.Stop();
      broadcaster.CancelOperations();
    }
  }
}





