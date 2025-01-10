using UnityEngine;

namespace Assets.Scripts.Networking
{
    internal class serverTest : MonoBehaviour
    {
        public void StartServer()
        {
            Host host = new Host();

            host.onMessage += (msg) => Debug.Log(msg);
            
            Debug.Log("Starting server");
            host.Start();
        }

        public void StartClient()
        {
            Client client = new Client();

            client.onMessage += (msg) => Debug.Log(msg);

            client.Start();
        }
    }
}
