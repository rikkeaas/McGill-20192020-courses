using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositCl : ActionDeposit
{
    void Start()
    {
        base.Initialize();
        SetSpicesToDeposit(new int[]{0,0,0,0,1,0,0});
    }

    public override string ToString()
    {
        return "Deposit Cl in caravan";
    }

}
