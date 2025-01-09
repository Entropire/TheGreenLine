using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Networking
{
    internal class serverTest : MonoBehaviour
    {
        public void StartServer()
        {
            Server server = new Server();

            server.onMessage += (msg) => Debug.Log(msg);

            server.Start();
        }

        public void StartClient()
        {
            Client client = new Client();

            client.onMessage += (msg) => Debug.Log(msg);

            client.Start();
        }
    }
}
