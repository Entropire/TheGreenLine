using System.Net;
using System.Threading;
using UnityEngine;

namespace Server
{
  internal class NetworkingUI : MonoBehaviour
  {
    CancellationTokenSource cts = new CancellationTokenSource();

    public void Client()
    {
      Client client = new Client();
      client.onMessage += (msg) => print(msg);
      client.Start(IPAddress.Parse("192.168.178.195"), 8000, cts.Token);
    }

    public void Host()
    {
      Host host = new Host();
      host.onMessage += (msg) => print(msg);
      host.Start(IPAddress.Parse("192.168.178.195"), 8000, cts.Token);
    }
  }
}