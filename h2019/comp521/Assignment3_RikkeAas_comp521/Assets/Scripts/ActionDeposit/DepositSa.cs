using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositSa : ActionDeposit
{
    void Start()
    {
        base.Initialize();
        SetSpicesToDeposit(new int[]{0,1,0,0,0,0,0});
    }

    public override string ToString()
    {
        return "Deposit Sa in caravan";
    }

}
