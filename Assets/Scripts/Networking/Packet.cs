using System;

namespace Assets.Scripts.Networking
{
  [Serializable]
  public class Packet
  {
    public PacketType type;
    public string message;

    public Packet(PacketType type, string message)
    {
      this.type = type;
      this.message = message;
    }
  }
}

