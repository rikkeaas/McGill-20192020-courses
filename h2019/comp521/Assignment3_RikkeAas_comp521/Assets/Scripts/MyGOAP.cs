using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGOAP : MonoBehaviour
{
    public GameObject planDisplay;

    private List<AbstractAction> allActions = new List<AbstractAction>(); // List of all possible actions the player can do
    private Queue<AbstractAction> plan;
    private AbstractAction nextAction;
    private Player player;
    private Caravan caravan;
    private float timeStamp = -1;

    private int[][] goals = new int[7][] // List of the goals of the GOAP AI
    {
        new int[7]{2,0,0,0,0,0,0},
        new int[7]{0,2,0,0,0,0,0},
        new int[7]{0,0,2,0,0,0,0},
        new int[7]{0,0,0,2,0,0,0},
        new int[7]{0,0,0,0,2,0,0},
        new int[7]{0,0,0,0,0,2,0},
        new int[7]{0,0,0,0,0,0,2}
    };

    void Start()
    {
        plan = new Queue<AbstractAction>();
        player = this.GetComponent<Player>();
        caravan = GameObject.FindWithTag("Caravan").GetComponent<Caravan>();

        // Adding all possible actions to the list of actions
        allActions.Add(this.GetComponent<ActionTrader1>());
        allActions.Add(this.GetComponent<ActionTrader2>());
        allActions.Add(this.GetComponent<ActionTrader3>());
        allActions.Add(this.GetComponent<ActionTrader4>());
        allActions.Add(this.GetComponent<ActionTrader5>());
        allActions.Add(this.GetComponent<ActionTrader6>());
        allActions.Add(this.GetComponent<ActionTrader7>());
        allActions.Add(this.GetComponent<ActionTrader8>());
        allActions.Add(this.GetComponent<DepositTu>());
        allActions.Add(this.GetComponent<DepositSa>());
        allActions.Add(this.GetComponent<DepositCa>());
        allActions.Add(this.GetComponent<DepositCi>());
        allActions.Add(this.GetComponent<DepositCl>());
        allActions.Add(this.GetComponent<DepositPe>());
        allActions.Add(this.GetComponent<DepositSu>());
        allActions.Add(this.GetComponent<ReclaimTu>());
        allActions.Add(this.GetComponent<ReclaimSa>());
        allActions.Add(this.GetComponent<ReclaimCa>());
        allActions.Add(this.GetComponent<ReclaimCi>());
        allActions.Add(this.GetComponent<ReclaimCl>());
        allActions.Add(this.GetComponent<ReclaimPe>());
        allActions.Add(this.GetComponent<ReclaimSu>());
    }

    void Update()
    {
        if (plan.Count == 0 && nextAction == null) // Player doesn't have a plan so it must replan
        {
            MakePlan();
            // Displaying the new plan if there is a new plan (when all goals are achieved the plan will be empty)
            if (plan.Count != 0) planDisplay.GetComponent<Plan>().DisplayPlan(new List<AbstractAction>(plan));
        }
        else if (nextAction == null) // Previous action is done, so player moves on to next action
        {
            nextAction = plan.Dequeue();
        }
        else // Player is working on finishing the action stored in nextAction
        {
            if (!nextAction.IsInRange(this.transform)) // Player must be in range of trader/caravan to do action
            {
                player.MoveTowards(nextAction.GetActionObject());
            }
            else if (timeStamp == -1) // Player has to stay by trader/caravan 0.5s to do action, so we must track time
            {
                timeStamp = Time.time;
            }
            else if (Time.time - timeStamp >= 0.5) // Player has stayed by trader/caravan 0.5s, so action can be executed
            {
                if (!nextAction.PreConditionsSatisfied(player.GetInventory())) // Something has changed since plan was made, action can't be done
                {
                    planDisplay.GetComponent<Plan>().DisplayMessage("Plan failed, replanning");
                    plan = new Queue<AbstractAction>(); // We empty plan so planner can replan with new world state
                    nextAction = null;
                }
                else // Action can be executed
                {
                    (int[],int[]) updatedState = nextAction.DoAction(player.GetInventory());
                    player.ChangeInventory(updatedState.Item1); // Updating inventory
                    caravan.UpdateSpices(updatedState.Item2); // Updating caravan
                    nextAction = null; // Action has been executed, so nextAction is cleared
                    timeStamp = -1; // Time counter is reset
                }
            }
        }
    }

    // Method to find the first unfulfilled goal and create a plan based on this with the help of the Graph.cs class
    private void MakePlan()
    {
        for (int i = 0; i < 7; i++)
        {
            if (caravan.getSpiceCount(i) >= goals[i][i]) // Goal fulfilled so we skip it
            {
                continue;
            }

            // Create graph with the found unfulfilled goal as the goal, and with the current world state
            Graph graph = new Graph(allActions, player.GetInventory(), caravan.GetAllSpiceCounts(), goals[i]);
            plan = new Queue<AbstractAction>(graph.FindPlan());
            return; // Plan has been constructed so we exit the mehtod
        }
        planDisplay.GetComponent<Plan>().DisplayMessage("All Goals Completed");
    }
}
