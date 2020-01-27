using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionReclaim : AbstractAction
{
    public GameObject caravan;

    // Method to set pre and post conditions for reclaim actions
    public void SetSpicesToReclaim(int[] spicesToReclaim)
    {
        SetPostInventory(spicesToReclaim);
        int sum = 0;
        for (int i = 0; i < spicesToReclaim.Length; i++)
        {
            sum += spicesToReclaim[i];
            spicesToReclaim[i] = -spicesToReclaim[i];
        }
        SetMaxInventorySize(4 - sum); // If player is trying to reclaim more than 4 spices the inventory size condition will never be true
        SetPostCaravan(spicesToReclaim);
    }

    public void Initialize()
    {
        SetActionObject(caravan);
    }

    // Overriding precondition satisfaction check since it has to take into account the caravan preconditions also
    public override bool PreConditionsSatisfied(int[] preInventory) 
    {
        bool baseConditionsSatisfied = base.PreConditionsSatisfied(preInventory);
        if (!baseConditionsSatisfied) return false;

        return caravan.GetComponent<Caravan>().HasSpices(GetPostInventory()); // What is in post inventory is what has to be in the caravan prestate
    }
}
