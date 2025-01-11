using System.Net;
using System.Threading;
using UnityEditor.Rendering.Universal;
using UnityEngine;

namespace Server
{
  internal class NetworkingUI : MonoBehaviour
  {
    CancellationTokenSource cts = new CancellationTokenSource();

    public void Client()
    {
      Client client = new Client();
      client.onMessage += (msg) => Debug.Log(msg);
      client.onError += (msg) => Debug.LogError(msg);
      client.Start(IPAddress.Parse("192.168.178.195"), 8000, cts.Token);
    }

    public void Host()
    {
      Host host = new Host();
      host.onMessage += (msg) => Debug.Log(msg);
      host.onError += (msg) => Debug.LogError(msg);
      host.Start(IPAddress.Parse("192.168.178.195"), 8000, cts.Token);
    }
  }
}