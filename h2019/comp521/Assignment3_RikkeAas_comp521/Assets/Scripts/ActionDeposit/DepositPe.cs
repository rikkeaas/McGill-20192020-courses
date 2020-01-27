using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositPe : ActionDeposit
{
    void Start()
    {
        base.Initialize();
        SetSpicesToDeposit(new int[]{0,0,0,0,0,1,0});
    }

    public override string ToString()
    {
        return "Deposit Pe in caravan";
    }

}
