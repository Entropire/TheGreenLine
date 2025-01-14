using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class HostUI : MonoBehaviour
{
  [SerializeField] TMP_Dropdown dropdown;
  private List<IPAddress> iPAddresses = new List<IPAddress>();

  private void OnEnable()
  {
    GetLocalIPAddress();
    dropdown.ClearOptions();
    foreach (IPAddress ip in iPAddresses)
    {
      TMP_Dropdown.OptionData dropdownOption = new TMP_Dropdown.OptionData(ip.ToString());
      dropdown.options.Add(dropdownOption);
    } 

    dropdown.RefreshShownValue();
  }

  private void GetLocalIPAddress()
  {
    try
    {
      var host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (var ip in host.AddressList)
      {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
          iPAddresses.Add(ip);
        }
      }
      throw new Exception("No IPv4 address found!");
    }
    catch (Exception ex)
    {
      Console.WriteLine("Error: " + ex.Message);
    }
  }
}
