using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceSender : MonoBehaviour
{
    public GameObject SendMenu;
    public GameObject ResourceManager;
    public ResourceManager resourceManager;
    public bool BoatSent;

    public bool PlayerHasSent;

    public float Timer;
    public int SendTime;

    public List<int> Resources = new List<int>();

    void Start()
    {
        resourceManager = ResourceManager.GetComponent<ResourceManager>();
        SendMenu.SetActive(false);
        Resources.Add(resourceManager.Steel);
    }

    

    void FixedUpdate()
    {
        
    }

    public void OpenSend()
    {
        SendMenu.SetActive(true);
    }

    public void CloseSend()
    {
        SendMenu.SetActive(false);
    }

    public void SendResource1()
    {
            BoatSent = true;
            Debug.Log("Steel Sent");
            resourceManager.Steel -= 10;

    }
    

    
    //{
        
    //}

    //public void SendRersource3()
    //{

    //}

    public void RecieveResources()
    {
            if(PlayerHasSent == true && Timer == SendTime)
            {
                
            }
    }

    public enum ResourceType
    {
        R1,
        R2,
        R3
    }

}
