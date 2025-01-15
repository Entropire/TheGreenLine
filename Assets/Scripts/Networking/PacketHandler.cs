using System;
using UnityEngine;

namespace Assets.Scripts.Networking
{
  internal static class PacketHandler
  {
    private static TcpConnection tcpConnection;

    public static event Action onConnected;
    public static event Action onDisconnected;
    public static event Action<string> onChatMessage;

    public static void HandlePacket(Packet packet)
    {
      Debug.Log(packet.type + ":" + packet.message);

      switch (packet.type)
      {
        case PacketType.Connected:
          onConnected?.Invoke();
          break;
        case PacketType.Disconnected:
          onDisconnected?.Invoke();
          break;
        case PacketType.ChatMessage:
          onChatMessage?.Invoke(packet.message);
          break;
      }
    }

    public static void SetTcpConnection(TcpConnection tcpConnection)
    {
      PacketHandler.tcpConnection = tcpConnection;
    }

    public static void StopTcpConnection()
    {
      tcpConnection.Stop();
    }

    public static void SendPacket(PacketType packetType, string message)
    {
      tcpConnection?.SendPacket(packetType, message);
    }
  }
}
