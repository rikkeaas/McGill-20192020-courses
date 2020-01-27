using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plan : MonoBehaviour
{
    private Text text;
    
    void Start()
    {
        text = this.GetComponent<Text>();
    }

    public void DisplayPlan(List<AbstractAction> plan)
    {
        string planString = "";
        for (int i = 0; i < plan.Count; i++)
        {
            planString += plan[i].ToString() + '\n'; // Abstract action has a ToString() override which gives a good description of the action
        }

        text.text = planString;
    }

    // Method to display a message instead of a plan (e.g. when all goals are satisfied)
    public void DisplayMessage(string message)
    {
        text.text = message;
    }
}
