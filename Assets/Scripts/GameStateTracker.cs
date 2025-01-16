using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
  public class GameStateTracker : MonoBehaviour
  {
    public Player.Player playerOne;
    public Player.Player playerTwo;
    public double gameTimeSeconds;


    private void Update()
    {
      gameTimeSeconds += Time.deltaTime;
    }
  }
}
