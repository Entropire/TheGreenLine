using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    [SerializeField] private GameObject ui;

    [SerializeField] private TMP_InputField ipAddress;
    
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        ui.SetActive(false);
    }

    public void Client()
    {
        UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        transport.ConnectionData.Address = ipAddress.text;
        
        NetworkManager.Singleton.StartClient();
        ui.SetActive(false);
    }

    public void Server()
    {
        NetworkManager.Singleton.StartServer();
        ui.SetActive(false);
    }
}
