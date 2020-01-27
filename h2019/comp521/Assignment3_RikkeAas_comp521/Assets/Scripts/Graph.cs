using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Node> graph = new List<Node>();
    int[] caravanStart;
    int[] goal;

    class Node // Anonymos class Node is used to build the graph
    {
        List<Node> children; // List of all possible next actions
        AbstractAction action; // The action this node represents
        // World state after action has been executed
        int[] inventoryStateAfterAction;
        int[] caravanStateAfterAction;

        public Node(AbstractAction action, int[] inventoryState, int[] caravanState)
        {
            children = new List<Node>();
            this.action = action;
            this.inventoryStateAfterAction = (int[]) inventoryState.Clone();
            this.caravanStateAfterAction = (int[]) caravanState.Clone();
        }

        public void AddChild(Node newChild)
        {
            children.Add(newChild);
        }

        public List<Node> GetChildren()
        {
            return children;
        }

        public AbstractAction GetAction()
        {
            return action;
        }

        public int[] GetPostInventory()
        {
            return (int[])inventoryStateAfterAction.Clone();
        }

        public int GetPostCaravanItem(int idx)
        {
            return caravanStateAfterAction[idx];
        }
    }

    public Graph(List<AbstractAction> allActions, int[] inventoryStartState, int[] caravanStartState, int[] caravanGoalState)
    {
        // Saving caravan initial state and goal state to be able to judge if nodes get player closer to goal
        caravanStart = (int[]) caravanStartState.Clone();
        goal = (int[]) caravanGoalState.Clone();

        graph.Add(new Node(null, inventoryStartState, caravanStartState)); // Start node
        int counter = 0; // Safety counter to avoid infinet loop (should not be possible, but just in case)
        while (!CloserToGoal() && counter < 10) // If there is a node to get us closer to the goal we don't need to build the graph bigger
        {
            counter++;
            List<Node> nextGen = new List<Node>();
            foreach (Node node in graph)
            {
                if (node.GetChildren().Count == 0) // If node already has children, it necessairily has all possible children so they shouldn ot be added again
                {
                    foreach (AbstractAction action in allActions) // Checking each action to see if it's a possible next action for the node
                    {
                        if (action.PreConditionsSatisfied(node.GetPostInventory()))
                        {
                            (int[],int[]) effect = action.DoAction(node.GetPostInventory());
                            for (int i = 0; i < 7; i++)
                            {
                                effect.Item2[i] += node.GetPostCaravanItem(i); // Adding effect on caravan to current caravan state
                            }
                            Node newNode = new Node(action, effect.Item1, effect.Item2);
                            nextGen.Add(newNode);
                            node.AddChild(newNode);
                        }
                    }
                }
            }
            graph.AddRange(nextGen); // Adding all new nodes to the graph before moving to next iteration
        }
    }

    public List<AbstractAction> FindPlan() 
    {
        List<AbstractAction> plan = new List<AbstractAction>();
        if (graph.Count == 1) // Only node added is the first one (start state node)
        {
            Debug.Log("Graph building failed, no plan constructed");
            return plan;
        }
        Node start = graph[0]; // First node in graph list is the start state node so we start the DFS from there
        plan = TraverseGraph(start);
        plan.Reverse(); // DFS gives us a list of nodes from the bottom up, so it must be reversed
        return plan;
    }

    // Depth first search to find path to a node that brings player closer to its goal
    private List<AbstractAction> TraverseGraph(Node currNode)
    {
        List<AbstractAction> plan = new List<AbstractAction>();
        foreach (Node child in currNode.GetChildren())
        {
            if (NodeCloserToGoal(child)) // Found a node that is closer to the goal state, so no need to search longer
            {
                plan.Add(child.GetAction());
                return plan;
            }
            plan = TraverseGraph(child);
            if (plan.Count != 0) // child has some path to a node closer to the goal state so we return this
            {
                plan.Add(child.GetAction());
                return plan;
            }
        }
        return plan;
    }

    // Method to check if any of the nodes in graph list is closer to the goal state
    private bool CloserToGoal()
    {
        foreach (Node node in graph)
        {
            if (NodeCloserToGoal(node)) return true;
        }
        return false;
    }

    private bool NodeCloserToGoal(Node node)
    {
        AbstractAction action = node.GetAction();
        if (action == null) return false; // Start node has null as its action, and this obviously can't get us closer to the goal state
        for (int i = 0; i < 7; i++)
        {
            if (goal[i] != 0) // Each goal only has one value that is not zero, and this is the important part of the goal state
            {
                // If caravan state i after node action is greater than the initial caravan state i we are closer to the goal state
                return caravanStart[i] < node.GetPostCaravanItem(i); 
            }
        }
        return false;
    }
}
