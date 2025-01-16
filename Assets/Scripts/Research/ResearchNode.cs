using System;

namespace Assets.Scripts.Research
{
  [Serializable]
  public class ResearchNode
  {
    public int nodeID;
    public string name;
    public float duration;
    public int[] prerequisites;
    public string[] unlocks;
  }
}
