using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositSu : ActionDeposit
{
    void Start()
    {
        base.Initialize();
        SetSpicesToDeposit(new int[]{0,0,0,0,0,0,1});
    }

    public override string ToString()
    {
        return "Deposit Su in caravan";
    }

}
