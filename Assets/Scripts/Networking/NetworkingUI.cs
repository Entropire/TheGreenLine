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
      client.Start(IPAddress.Parse("127.0.0.1"), 8080, cts.Token);
    }

    public void Host()
    {
      Host host = new Host();
      host.onMessage += (msg) => print(msg);
      host.Start(IPAddress.Parse("127.0.0.1"), 8080, cts.Token);
    }
  }
}