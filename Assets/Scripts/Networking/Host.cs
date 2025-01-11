using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Server
{
  internal class Host : Client
  {
    public event Action onReady;
    public event Action onConnected;
    public event Action<string> onMessage;

    private TcpListener listener;
    private TcpClient client;
    private NetworkStream stream;

    public new void Start(IPAddress ip, ushort port, CancellationToken ct)
    {
      Thread thread = new Thread(() => StartAsync(ip, port, ct));
      onMessage?.Invoke("host: Starting new thread for host!");
      thread.Start();
    }

    private async void StartAsync(IPAddress ip, ushort port, CancellationToken ct)
    {

      try
      {
        listener = new TcpListener(ip, port);
        onMessage?.Invoke($"host: Listener started");
        listener.Start();

        onReady?.Invoke();
        await ListenForClient(ct);

        if (client != null)
        {
          onConnected?.Invoke();
        }

        await ListenForPackets(ct);
      }
      catch (SocketException ex)
      {
        Console.WriteLine($"Unable to connect: {ex.Message}");
      }
      catch (ArgumentException ex)
      {
        Console.WriteLine($"Invalid IP or port: {ex.Message}");
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        listener.Stop();

      }
    }

    private async Task ListenForClient(CancellationToken ct)
    {
      onMessage?.Invoke("host: Listening for clients");
      try
      {
        TcpClient client = await listener.AcceptTcpClientAsync();
        this.client = client;
        this.stream = client.GetStream();

        SendPacket(PacketType.Message, "You are connected to the server!");
      }
      catch (OperationCanceledException)
      {
        Console.WriteLine("Client connection attempt canceled.");
      }
    }

    private async Task ListenForPackets(CancellationToken ct)
    {
      onMessage?.Invoke("host: Listening for incoming packets");
      while (true)
      {
        if (ct.IsCancellationRequested || !client.Connected)
        {
          break;
        }

        if (!stream.DataAvailable)
        {
          Task.Delay(100);
          continue;
        }

        Packet packet = await ReadPacket();
        Console.WriteLine($"[{packet.type}] Client: {packet.message}");
      }
    }

    private async Task<Packet> ReadPacket()
    {
      byte[] lenghtBuffer = new byte[4];
      stream.Read(lenghtBuffer, 0, 4);
      int messageLenth = BitConverter.ToInt32(lenghtBuffer, 0);

      byte[] messageBuffer = new byte[messageLenth];
      int totalBytesRead = 0;

      while (totalBytesRead < messageLenth)
      {
        int bytesRead = stream.Read(messageBuffer, totalBytesRead, messageLenth - totalBytesRead);

        if (bytesRead == 0)
        {
          break;
        }

        totalBytesRead += bytesRead;
      }
      string jsonString = Encoding.UTF8.GetString(messageBuffer);
      return JsonUtility.FromJson<Packet>(jsonString);
    }

    public void SendPacket(PacketType packetType, String packetData)
    {
      if (client == null || !client.Connected)
      {
        return;
      }

      Packet packet = new Packet(packetType, packetData);
      byte[] messageBuffer = Encoding.UTF8.GetBytes(JsonUtility.ToJson(packet));

      byte[] lenghtBuffer = BitConverter.GetBytes(messageBuffer.Length);

      stream.Write(lenghtBuffer, 0, lenghtBuffer.Length);
      stream.Write(messageBuffer, 0, messageBuffer.Length);
    }
  }
}





