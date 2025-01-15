using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
  void Start()
  {
    Application.runInBackground = true;
  }
}