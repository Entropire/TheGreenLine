using System.Net;
using System.Threading;
using TMPro;
using UnityEditor.Rendering.Universal;
using UnityEngine;

namespace Server
{
  internal class NetworkingUI : MonoBehaviour
  {
    [SerializeField] private TMP_Text text;

    CancellationTokenSource cts = new CancellationTokenSource();
    private SynchronizationContext mainThreadContext;

    private void Start()
    {
      mainThreadContext = SynchronizationContext.Current;
    }

    public void Client()
    {
      Client client = new Client();
      client.onMessage += (msg) =>
      {
        mainThreadContext.Post(_ =>
        {
          Debug.Log(msg);
          text.text = msg;
        }, null);
      };
        client.onError += (msg) =>
      {
        mainThreadContext.Post(_ =>
        {
          Debug.Log(msg);
          text.text = msg;
        }, null);
      };
        client.Start(IPAddress.Parse("192.168.178.195"), 8000, cts.Token);
    }

    public void Host()
    {
      Host host = new Host();
      host.onMessage += (msg) =>
      {
        mainThreadContext.Post(_ =>
        {
          Debug.Log(msg);
          text.text = msg;
        }, null);
      };
      host.onError += (msg) =>
      {
        mainThreadContext.Post(_ =>
        {
          Debug.Log(msg);
          text.text = msg;
        }, null);
      };
      
      host.Start(IPAddress.Parse("192.168.178.195"), 8000, cts.Token);
    }
  }
}