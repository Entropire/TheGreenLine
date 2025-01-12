using System.Net;

namespace Assets.Scripts.Networking
{
  internal record LobbyData(string name, IPAddress ip)
  {
    public string name { private set; get; } = name;
    public IPAddress ip { private set; get; } = ip;
    public ushort port { private set; get; } = 8000;
  }
}
