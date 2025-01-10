using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Assets.Scripts.Networking
{
    public class Client
    {
        public Action<String> onMessage;
        
        private TcpClient client;
        
        public void Start() => new Thread(StartTcpClient).Start();

        private async void StartTcpClient()
        {
            try
            {
                onMessage.Invoke("Connecting to the server...");
                client = new TcpClient();
                await client.ConnectAsync("127.0.0.1", 7777);
                onMessage.Invoke("Connected!");
                await RecievePackets();
            }
            catch (Exception e)
            {
                onMessage(e.Message);
            }
            finally
            {
                client.Close();
                onMessage.Invoke("Disconnected!");
            }
        }

        private async Task RecievePackets()
        {
            onMessage.Invoke("Listening for packets");
            byte[] bytes = new byte[1024];

            NetworkStream stream = client.GetStream();
            
            while (true)
            {
                try
                {
                    if (!client.Connected)
                        continue;
                
                    var bytesRead = await stream.ReadAsync(bytes, 0, bytes.Length);
                
                    if (bytesRead <= 0)
                        continue;
                
                    string data = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                    Packet packet = JsonUtility.FromJson<Packet>(data);
                    
                    onMessage.Invoke($"Server: [{packet.type}] {packet.data}");
                }
                catch (IOException e)
                {
                    onMessage.Invoke(e.Message);
                    break;
                }
            }
        }

        public void SendPacket(PacketType packetType, string packetData)
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
                onMessage.Invoke("Disconnected from server");
            }
        }
    }
}