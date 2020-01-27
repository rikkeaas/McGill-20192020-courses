using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositTu : ActionDeposit
{
    void Start()
    {
        base.Initialize();
        SetSpicesToDeposit(new int[]{1,0,0,0,0,0,0});
    }

    public override string ToString()
    {
        return "Deposit Tu in caravan";
    }

}
