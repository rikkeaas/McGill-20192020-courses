using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReclaimTu : ActionReclaim
{

    void Start()
    {
        base.Initialize();
        SetSpicesToReclaim(new int[]{1,0,0,0,0,0,0});
    }

    
    public override string ToString()
    {
        return "Reclaim Tu from caravan";
    }
}
