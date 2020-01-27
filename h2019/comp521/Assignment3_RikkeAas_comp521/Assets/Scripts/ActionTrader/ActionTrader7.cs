using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrader7 : AbstractAction
{
    public GameObject traderHandler;
    
    public ActionTrader7()
    {
        SetPreInventoryConditions(new int[7]{0,0,4,0,0,0,0});
        SetPostInventory(new int[7]{0,0,-4,0,0,0,1});
    }

    void Start() 
    {
        SetActionObject(traderHandler.GetComponent<TraderHandler>().GetTraderFromName(7));
    }
}
