using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.Networking
{
    internal class Server
    {
        public Action<String> onMessage;

        private TcpListener listener;
        private Dictionary<int, TcpClient> clients = new();

        public void Start()
        {
            IPAddress iPAddress = GetLocalIP();
            if (iPAddress == null)
            {
                return;
            }

            listener = new TcpListener(iPAddress, 8080);
            listener.Start();
            onMessage.Invoke($"Server started: {iPAddress}:8080");

            Thread thread = new Thread(RunServer);
            thread.Start();
        }

        private async void RunServer()
        {
            int i = 0;

            onMessage.Invoke("Listening for clients");
            try
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    int id = i++;
                    clients.Add(id++, client);

                    Task.Run(() => ReceiveData(id, client));

                    SendMessage(id, "You are connected to the server!");
                }
            }
            catch (Exception e)
            {
                onMessage.Invoke(e.Message);
                throw;
            }
            finally
            {
                listener.Stop();
                onMessage.Invoke("Server stopped");
            }
        }

        private async void ReceiveData(int id, TcpClient client)
        {
            byte[] bytes = new byte[1024];
            string data;
            int bytesRead;

            using (client)
            {
                if (client.Connected)
                {
                    NetworkStream stream = client.GetStream();

                    while (true)
                    {
                        try
                        {
                            bytesRead = await stream.ReadAsync(bytes, 0, bytes.Length);

                            if (bytesRead <= 0)
                                continue;

                            data = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                            onMessage.Invoke("client " + id + ": " + data);
                        }
                        catch (IOException ex)
                        {
                            break;
                        }
                    }
                }
            }

            clients.Remove(id);
        }

        private void SendMessage(int id, string message)
        {
            if (clients.TryGetValue(id, out TcpClient client) && client != null)
            {
                if (!client.Connected)
                {
                    clients.Remove(id);
                    return;
                }

                try
                {
                    NetworkStream stream = client.GetStream();

                    Byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    onMessage.Invoke($"failed to send message to client {id}: {e}");
                    throw;
                }
            }
        }

        private void BroadCastMessage(string message)
        {
            TcpClient client;

            foreach (var clientId in clients.Keys)
            {
                client = clients[clientId];

                if (client.Connected)
                {
                    SendMessage(clientId, message);
                }
                else
                {
                    clients.Remove(clientId);
                }
            }
        }

        private IPAddress GetLocalIP()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName); 

            foreach (var ip in ipAddresses)
            {
              
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            return null;
        }
    }
}
