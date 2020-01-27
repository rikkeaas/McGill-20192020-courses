using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Trader : MonoBehaviour
{
    private int traderIndex;

    public void SetName(int name)
    {
        traderIndex = name;
    }

    public int GetName() 
    {
        return traderIndex;
    }

    public (int[],int[]) GetTradeAction() 
    {
        int[] takes;
        int[] gives;

        switch (traderIndex)
        {
            case 1:
                takes = new int[7]{0,0,0,0,0,0,0}; // Takes nothing
                gives = new int[7]{2,0,0,0,0,0,0}; // Gives two turmeric
                break;
            case 2:
                takes = new int[7]{2,0,0,0,0,0,0}; // Takes two turmeric
                gives = new int[7]{0,1,0,0,0,0,0}; // Gives one saffron
                break;
            case 3:
                takes = new int[7]{0,2,0,0,0,0,0}; // Takes two saffron
                gives = new int[7]{0,0,1,0,0,0,0}; // Gives one cardamom
                break;
            case 4:
                takes = new int[7]{4,0,0,0,0,0,0}; // Takes four turmeric
                gives = new int[7]{0,0,0,1,0,0,0}; // Gives one cinnamon
                break;  
            case 5:
                takes = new int[7]{1,0,1,0,0,0,0}; // Takes one turmeric and one cardamom
                gives = new int[7]{0,0,0,0,1,0,0}; // Gives one cloves
                break;
            case 6:
                takes = new int[7]{2,1,0,1,0,0,0}; // Takes two turmeric, one saffron and one cinnamon
                gives = new int[7]{0,0,0,0,0,1,0}; // Gives one pepper
                break; 
            case 7:
                takes = new int[7]{0,0,4,0,0,0,0}; // Takes four cardamom
                gives = new int[7]{0,0,0,0,0,0,1}; // Gives one sumac
                break;
            case 8:
                takes = new int[7]{0,1,0,1,1,0,0}; // Takes one saffron, one cinnamon and one cloves
                gives = new int[7]{0,0,0,0,0,0,1}; // Gives one sumac
                break;
            default:
                Debug.Log("couldn't match trader");
                takes = new int[7]{0,0,0,0,0,0,0};
                gives = new int[7]{0,0,0,0,0,0,0};
                break;
        }
        return (takes,gives);
    }
}
