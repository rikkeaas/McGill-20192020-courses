using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionDeposit : AbstractAction
{
    public GameObject caravan;


    public void Initialize()
    {
        SetActionObject(caravan);
    }

    // Method to define pre and post conditions of both inventory and caravan for deposit actions
    public void SetSpicesToDeposit(int[] spicesToDeposit)
    {
        SetPreInventoryConditions(spicesToDeposit);
        SetPostCaravan(spicesToDeposit);
        for (int i = 0; i < 7; i++)
        {
            spicesToDeposit[i] = -spicesToDeposit[i];
        }
        SetPostInventory(spicesToDeposit);

    }

}
