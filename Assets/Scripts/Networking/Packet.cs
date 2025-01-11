using System;

namespace Assets.Scripts.Networking
{
  public class Packet
  {
    public PacketType type { get; set; }
    public String message { get; set; }

    public Packet(PacketType type, String message)
    {
      this.type = type;
      this.message = message;
    }
  }
}

