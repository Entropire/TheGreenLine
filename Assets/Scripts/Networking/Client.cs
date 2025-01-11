using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Server
{
  internal class Client
  {
    public event Action onConnected;
    public event Action<string> onMessage;

    private TcpClient client;
    private NetworkStream stream;

    public void Start(IPAddress ip, ushort port, CancellationToken ct)
    {
      Thread thread = new Thread(() => StartAsync(ip, port, ct));
      onMessage?.Invoke("client: Starting new thread for client!");
      thread.Start();
    }

    private void StartAsync(IPAddress ip, ushort port, CancellationToken ct)
    {
      try
      {
        client = new TcpClient();
        client.Connect(ip, port);
        stream = client.GetStream();

        onMessage?.Invoke($"client: Connecting to {ip}:{port}");
        onConnected?.Invoke();

        SendPacket(PacketType.Message, "You are connected to the client!");
        ListenForPackets(client, ct);
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
        client?.Dispose();
      }
    }

    private async void ListenForPackets(TcpClient client, CancellationToken ct)
    {
      onMessage?.Invoke("client: Listening for incoming packets");
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

        Packet packet = await ReadPacket(stream);
        Console.WriteLine($"[{packet.type}] Host: {packet.message}");
      }
    }

    private async Task<Packet> ReadPacket(Stream stream)
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

