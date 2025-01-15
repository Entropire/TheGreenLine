using Assets.Scripts.Networking;
using Assets.Scripts.UI;
using TMPro;
using UnityEngine;

public class Chat : MonoBehaviour
{
  [SerializeField] private TMP_InputField inputField;
  [SerializeField] private Transform chatContents;
  [SerializeField] private GameObject messagePrefab;
  [SerializeField] private TMP_Text messageText;

  private void Start()
  {
    inputField.onSubmit.AddListener(OnInputSubmit);

    PacketHandler.onChatMessage += AddMessage;
  }

  private void OnInputSubmit(string message)
  {
    AddMessage(message);
    //NetworkingUI.instance.tcpConnection.SendPacket(PacketType.ChatMessage, message);
  }

  private void AddMessage(string message)
  {
    messageText.text = message;
    GameObject newMessage = Instantiate(messagePrefab);
    newMessage.transform.parent = chatContents;
  }
}
