using System;
using UnityEngine;

namespace Assets.Scripts.Networking
{
  internal class PacketHandler : MonoBehaviour
  {
    public static event Action<string> onChatMessage;
    public static PacketHandler Instance;

    private void Start()
    {
      if (Instance == null)
      {
        Instance = this;
      }
    }

    public void HandlePacket(Packet packet)
    {
      Debug.Log(packet.type + ":" + packet.message);

      switch (packet.type)
      {
        case PacketType.Connected:
          onChatMessage?.Invoke("Connected");
          break;
        case PacketType.Disconnected:
          onChatMessage?.Invoke("Other player has disconnected!");
          break;
        case PacketType.ChatMessage:
          onChatMessage?.Invoke(packet.message);
          break;
      }
    }
  }
}
