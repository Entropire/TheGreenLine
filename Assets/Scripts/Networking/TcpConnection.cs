using System;
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
    public Action<string> onError;

    protected TcpClient client;
    protected NetworkStream stream;

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
      while (true)
      {
        if (!client.Connected)
        {
          break;
        }

        byte[] lenghtBuffer = new byte[4];
        await stream.ReadAsync(lenghtBuffer, 0, 4);
        int messageLength = BitConverter.ToInt32(lenghtBuffer, 0);

        byte[] messageBuffer = new byte[messageLength];
        int totalBytesRead = 0;

        while (totalBytesRead < messageLength)
        {
          int bytesRead = await stream.ReadAsync(messageBuffer, totalBytesRead, messageLength - totalBytesRead);

          if (bytesRead == 0)
          {
            break;
          }

          totalBytesRead += bytesRead;
        }

        string jsonString = Encoding.UTF8.GetString(messageBuffer); 
        Packet packet = JsonUtility.FromJson<Packet>(jsonString);
        string messagebits = "";
        for (int i = 0; i < messageBuffer.Length; i++)
        {
          messagebits += messageBuffer[i];
        }
        PacketHandler.HandlePacket(packet);
      }
    }

    public async void SendPacket(PacketType packetType, string packetData)
    {
      if (client == null || !client.Connected)
      {
        return;
      }

      Packet packet = new Packet(packetType, packetData);
      byte[] messageBuffer = Encoding.UTF8.GetBytes(JsonUtility.ToJson(packet));

      byte[] lenghtBuffer = BitConverter.GetBytes(messageBuffer.Length);

      await stream.WriteAsync(lenghtBuffer, 0, lenghtBuffer.Length);
      await stream.WriteAsync(messageBuffer, 0, messageBuffer.Length);


      string messagebits = "";
      for (int i = 0; i < messageBuffer.Length; i++)
      {
        messagebits += messageBuffer[i];
      }
    }
  }
}
