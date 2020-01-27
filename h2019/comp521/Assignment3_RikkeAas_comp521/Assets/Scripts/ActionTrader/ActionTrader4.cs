using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrader4 : AbstractAction
{
    public GameObject traderHandler;
    
    public ActionTrader4()
    {
        SetPreInventoryConditions(new int[7]{4,0,0,0,0,0,0});
        SetPostInventory(new int[7]{-4,0,0,1,0,0,0});
    }

    void Start() 
    {
        SetActionObject(traderHandler.GetComponent<TraderHandler>().GetTraderFromName(4));
    }
}
