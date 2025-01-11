namespace Assets.Scripts.Networking
{
  internal class PacketHandler
  {
    public void HandlePacket(Packet packet)
    {
      switch (packet.type)
      {
        case PacketType.Connected:
          HandleConnectedPacket(packet.message);
          break;
        case PacketType.Disconnected:
          HandleDisconnectedPacket(packet.message);
          break;
        case PacketType.ChatMessage:
          HandleChatMessagePacket(packet.message);
          break;
      }
    }

    private void HandleConnectedPacket(string data)
    {

    }

    private void HandleDisconnectedPacket(string data)
    {

    }

    private void HandleChatMessagePacket(string data)
    {

    }
  }
}
