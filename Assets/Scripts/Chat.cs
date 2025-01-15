using Assets.Scripts.Networking;
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
    PacketHandler.SendPacket(PacketType.ChatMessage, message);
    inputField.text = string.Empty;
  }

  private void AddMessage(string message)
  {
    Debug.Log("test");
    Debug.Log(message);
    messageText.text = message;
    GameObject newMessage = Instantiate(messagePrefab);
    newMessage.transform.SetParent(chatContents);
  }
}
