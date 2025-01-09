using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
                client.Connect("192.168.178.195", 8080);
                onMessage.Invoke("Connected to server");

                Thread thread = new Thread(ReceiveData);
                thread.Start();
            }
            catch (Exception e)
            {
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

            try
            {

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
            catch (Exception e)
            {
                onMessage.Invoke(e.Message);
            }
            finally
            {
                client.Close();
                onMessage.Invoke($"Disconnected from server");
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
