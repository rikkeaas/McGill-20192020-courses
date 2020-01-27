using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReclaimSa : ActionReclaim
{

    void Start()
    {
        base.Initialize();
        SetSpicesToReclaim(new int[]{0,1,0,0,0,0,0});
    }

    
    public override string ToString()
    {
        return "Reclaim Sa from caravan";
    }
}
