using System;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Networking
{
  internal static class PacketHandler
  {
    private static SynchronizationContext mainThreadContext = SynchronizationContext.Current;
    private static TcpConnection tcpConnection;

    public static event Action onConnected;
    public static event Action onDisconnected;
    public static event Action<string> onChatMessage;

    public static void HandlePacket(Packet packet)
    {
      mainThreadContext?.Post(_ =>
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
      }, null);
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
      Debug.Log("PacketHandler sending packets");
      tcpConnection?.SendPacket(packetType, message);
    }
  }
}
