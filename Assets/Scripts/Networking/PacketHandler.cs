using TMPro;
using UnityEngine;

namespace Assets.Scripts.Networking
{
  internal class PacketHandler : MonoBehaviour
  {
    [SerializeField] private TMP_Text TMP_Text;
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
          HandleConnectedPacket(packet.message);
          break;
        case PacketType.Disconnected:
          HandleDisconnectedPacket(packet.message);
          break;
        case PacketType.ChatMessage:
          HandleChatMessagePacket(packet.message);
          break;
      }
    }

    private void HandleConnectedPacket(string data)
    {
      TMP_Text.text = "connected";
    }

    private void HandleDisconnectedPacket(string data)
    {

    }

    private void HandleChatMessagePacket(string data)
    {
      TMP_Text.text = data;
    }
  }
}
