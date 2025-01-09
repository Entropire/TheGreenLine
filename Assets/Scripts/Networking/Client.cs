using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Assets.Scripts.Networking
{
    internal class Client
    {
        public Action<String> onMessage;

        TcpClient client;
        private bool runProgram = true;

        public void Start()
        {
            try
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 8080);
                onMessage.Invoke("Connected to server");

                System.Threading.Tasks.Task.Run(ReceiveData);
            }
            catch (Exception e)
            {
                client.Close();
                onMessage.Invoke($"You have been disconnected from the server: {e}");
                throw;
            }
        }

        private async void ReceiveData()
        {
            byte[] bytes = new byte[1024];
            string data;
            int bytesRead;

            NetworkStream stream = client.GetStream();

            while (true)
            {
                try
                {
                    if (!client.Connected)
                        continue;

                    bytesRead = await stream.ReadAsync(bytes, 0, bytes.Length);

                    if (bytesRead <= 0)
                        continue;

                    data = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                    onMessage.Invoke($"Server: {data}");
                }
                catch (IOException ex)
                {
                    onMessage.Invoke($"Disconnected from server");
                    break;
                }
            }
        }

        private void SendMessage(string? message)
        {
            if (client.Connected)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    Byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    onMessage.Invoke($"failed to send message to the server: {e}");
                    throw;
                }
            }
            else
            {
                onMessage.Invoke($"Disconnected from server");
            }
        }
    }
}
