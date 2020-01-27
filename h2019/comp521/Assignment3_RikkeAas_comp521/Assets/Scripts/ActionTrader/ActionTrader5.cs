using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrader5 : AbstractAction
{
    public GameObject traderHandler;
    
    public ActionTrader5()
    {
        SetPreInventoryConditions(new int[7]{1,0,1,0,0,0,0});
        SetPostInventory(new int[7]{-1,0,-1,0,1,0,0});
    }

    void Start() 
    {
        SetActionObject(traderHandler.GetComponent<TraderHandler>().GetTraderFromName(5));
    }
}
