using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrader1 : AbstractAction
{
    private GameObject traderHandler;

    void Start() 
    {
        traderHandler = GameObject.Find("Traders");
        SetActionObject(traderHandler.GetComponent<TraderHandler>().GetTraderFromName(1));

        SetPreInventoryConditions(new int[7]{0,0,0,0,0,0,0});
        SetPostInventory(new int[7]{2,0,0,0,0,0,0});
        SetMaxInventorySize(2); // Because trader 1 doesn't take any spices and gives two
    }

}
