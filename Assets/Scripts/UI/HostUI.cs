using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class HostUI : MonoBehaviour
{
  [SerializeField] TMP_Dropdown dropdown;
  private List<IPAddress> iPAddresses = new List<IPAddress>();


  private void OnEnable()
  {
    dropdown.ClearOptions();

    GetLocalIPAddress();

    foreach (IPAddress ip in iPAddresses)
    {
      TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(ip.ToString());
      dropdown.options.Add(optionData);
    }
  }

  private void GetLocalIPAddress()
  {
    try
    {
      var host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (var ip in host.AddressList)
      {
        if (ip.AddressFamily == AddressFamily.InterNetwork && !iPAddresses.Contains(ip))
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
