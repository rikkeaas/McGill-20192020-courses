using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrader3 : AbstractAction
{
    public GameObject traderHandler;
    
    public ActionTrader3()
    {
        SetPreInventoryConditions(new int[7]{0,2,0,0,0,0,0});
        SetPostInventory(new int[7]{0,-2,1,0,0,0,0});
    }

    void Start() 
    {
        SetActionObject(traderHandler.GetComponent<TraderHandler>().GetTraderFromName(3));
    }
}
