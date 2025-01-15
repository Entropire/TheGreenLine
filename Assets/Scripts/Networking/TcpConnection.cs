using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Networking
{
  internal abstract class TcpConnection
  {
    public Action onConnected;
    public Action<string> onMessage;
    public Action<string> onError;

    protected TcpClient client;
    protected NetworkStream stream;

    private PacketHandler packetHandler = new PacketHandler();
    private CancellationTokenSource tokenSource = new CancellationTokenSource();
    protected CancellationToken cancellationToken;

    public TcpConnection()
    {
      cancellationToken = tokenSource.Token;
    }

    public abstract void Start();

    public void Stop()
    {
      tokenSource.Cancel();
    }

    protected async Task ListenForPackets()
    {
      onMessage?.Invoke("client: Listening for incoming packets");
      while (true)
      {
        if (!client.Connected)
        {
          break;
        }

        if (!stream.DataAvailable)
        {
          await Task.Delay(100);
          continue;
        }

        Packet packet = ReadPacket(stream);
        Debug.Log($"package recieved: {packet.type}:{packet.message}");
        PacketHandler.Instance.HandlePacket(packet);
      }
    }

    protected Packet ReadPacket(Stream stream)
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

    public void SendPacket(PacketType packetType, string packetData)
    {
      if (client == null || !client.Connected)
      {
        return;
      }

      Packet packet = new Packet(packetType, packetData);
      byte[] messageBuffer = Encoding.UTF8.GetBytes(JsonUtility.ToJson(packet));

      byte[] lenghtBuffer = BitConverter.GetBytes(messageBuffer.Length);

      Debug.Log($"sending a backet: {packet.type}:{packet.message}");

      stream.Write(lenghtBuffer, 0, lenghtBuffer.Length);
      stream.Write(messageBuffer, 0, messageBuffer.Length);
    }
  }
}
