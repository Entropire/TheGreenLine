using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManagement : MonoBehaviour
{
    [SerializeField]
    public float EffBoost; //Efficiency Boost
    public int ProdBoost; //Production Boost
    public int FactoryAmnt; //How many factories
    public int BaseTimer; //Base timer for the factory
    [SerializeField]
    public bool Sweden;
    [SerializeField]
    public int Iron;
    public int ResourceTier;

    protected float Timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= BaseTimer / EffBoost)
        {
            if (Sweden == true)
            {
                Produce(true);
            }
        }
    }

    void Produce(bool Sweden)
    {
       if (Sweden == true)
        {
            Iron++;
        }
    }
}
