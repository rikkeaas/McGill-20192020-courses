using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReclaimSu : ActionReclaim
{

    void Start()
    {
        base.Initialize();
        SetSpicesToReclaim(new int[]{0,0,0,0,0,0,1});
    }

    
    public override string ToString()
    {
        return "Reclaim Su from caravan";
    }
}
