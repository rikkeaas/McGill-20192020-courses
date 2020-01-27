using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrader2 : AbstractAction
{

    public GameObject traderHandler;
    
    public ActionTrader2()
    {
        SetPreInventoryConditions(new int[7]{2,0,0,0,0,0,0});
        SetPostInventory(new int[7]{-2,1,0,0,0,0,0});
    }

    void Start() 
    {
        SetActionObject(traderHandler.GetComponent<TraderHandler>().GetTraderFromName(2));
    }
}
