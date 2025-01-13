using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
  void Start()
  {
    Application.runInBackground = true; // Keeps the game running when not in focus
  }
}