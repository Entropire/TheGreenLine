using System.Net;

namespace Assets.Scripts.Networking
{
  internal record LobbyData(IPAddress ip)
  {
    public IPAddress ip { private set; get; } = ip;
    public ushort port { private set; get; } = 8002;
  }
}
