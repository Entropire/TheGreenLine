using System.Net;

namespace Assets.Scripts.Networking
{
  internal class LobbyData
  {
    public string name { private set; get; }
    public IPAddress ip { private set; get; }
    public ushort port { private set; get; } = 8000;

    public LobbyData(string name, IPAddress ip)
    {
      this.name = name;
      this.ip = ip;
    }

    public override bool Equals(object obj)
    {
      if (obj is LobbyData other)
      {
        return this.name == other.name &&
               this.ip == other.ip &&
               this.port == other.port;
      }
      return false;
    }
  }
}
