using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Networking
{
    internal class Host
    {
        public Action<String> onMessage;
        
        private TcpClient client;
        private TcpListener listener;

        public void Start() => new Thread(StartTcpHost).Start();

        private async void StartTcpHost()
        {
            try
            {
                onMessage.Invoke("Starting server...");
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
                listener.Start();
                onMessage.Invoke("Server online!");
                await ListenForClient();
                await ReceivePacket();
            }
            catch (Exception e)
            {
                onMessage(e.Message);
            }
            finally
            {
                listener.Stop();
                onMessage.Invoke("Server stopped.");
            }
        }

        private async Task ListenForClient()
        {
            onMessage.Invoke("Listening for clients");
            try
            {
                while (client == null)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    this.client = client;
                    
                    SendPacket(PacketType.Message, "You are connected to the server!");
                    onMessage.Invoke($"client connected");
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

        private async Task ReceivePacket()
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
                            Packet packet = JsonUtility.FromJson<Packet>(data);
                            
                            onMessage.Invoke($"client: [{packet.type}] {packet.data}");
                        }
                        catch (IOException e)
                        {
                            onMessage.Invoke(e.Message);
                            break;
                        }
                    }
                }
            }
        }

        public void SendPacket(PacketType packetType, String packetData)
        {
            if (client.Connected)
            {
                try
                {
                    Packet packet = new Packet(packetType, packetData);
                    Byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(packet));
                    
                    NetworkStream stream = client.GetStream();
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