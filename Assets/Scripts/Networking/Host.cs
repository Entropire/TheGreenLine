using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

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

          await ListenForClient();
          SendPacket(PacketType.ChatMessage, "You are connected to the client!");

          if (client.Connected)
          {
            onConnected?.Invoke();
          }

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
      catch (Exception ex)
      {
        onError?.Invoke(ex.Message);
      }
      finally
      {
        client?.Dispose();
        listener?.Stop();
      }
    }

    private async Task ListenForClient()
    {
      try
      {
        TcpClient client = await listener.AcceptTcpClientAsync();
        this.client = client;
        this.stream = client.GetStream();
      }
      catch (Exception ex)
      {
        onError?.Invoke($"Error while listening for client: {ex.Message}");
      }
    }
  }
}





