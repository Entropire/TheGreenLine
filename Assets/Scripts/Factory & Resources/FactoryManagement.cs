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
    public GameObject ResourceManagement;
    [SerializeField]
    public int ResourceTier;
    int Iron;
    int Trans_Eq;
    int ExampleMoney;

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
            Produce(Sweden);
            Timer = 0;
        }
    }

    void Produce(bool Sweden)
    {
       if (Sweden == true)
        {
            Iron = ResourceManagement.GetComponent<ResourceTrack>().Iron;
            Iron = Iron + 1 * ProdBoost;
        }
       if (Sweden == false)
        {
            Trans_Eq = ResourceManagement.GetComponent<ResourceTrack>().Trans_Eq;
            Trans_Eq = Trans_Eq + 1 * ProdBoost;
        }
    }
    public void ProdUpgrade(int CurrentFunds)
    {
        int upgradeprice = 500;
        if (CurrentFunds >= upgradeprice)
        {
            upgradeprice = upgradeprice ^ 2;
            ProdBoost++;
        }
    }
}
