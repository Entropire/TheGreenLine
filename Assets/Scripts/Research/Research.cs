using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Research
{
  public class Research : MonoBehaviour
  {
    public static event Action<ResearchNode> onResearch;

    private Dictionary<int, ResearchNode> researchedNodes;
    private Dictionary<string, ResearchNode> unresearchedNodes;
    private ResearchNode nodeBeingResearched;

    private float timer;

    private void Start()
    {
      
    }

    private void Update()
    {
      timer += Time.deltaTime;

      if (timer >= nodeBeingResearched.duration)
      {
        timer -= nodeBeingResearched.duration;

        researchedNodes.Add(nodeBeingResearched.nodeID, nodeBeingResearched);
        onResearch?.Invoke(nodeBeingResearched);
        nodeBeingResearched = null;
      }
    }

    private void LoadReseachNodes()
    {

    }

    private void ReseachNode(TMP_Text tmp_text)
    {
      if (unresearchedNodes.ContainsKey(tmp_text.text))
      {
        nodeBeingResearched = unresearchedNodes[tmp_text.text];
        unresearchedNodes.Remove(tmp_text.text);
      }
    }
  }
}