using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    public GameObject shops;
    public GameObject seatingArea;
    public GameObject shopperHandler;

    private ShopGenerator shopScript;
    private GenerateSeatingArea seatingScript;
    private ShopperHandler shopperScript;
 
    void Awake()
    {
        shopScript = shops.GetComponent<ShopGenerator>();
        seatingScript = seatingArea.GetComponent<GenerateSeatingArea>();
        shopperScript = shopperHandler.GetComponent<ShopperHandler>();
    }

    // Method to fetch a random shop
    public GameObject GetRandomShop()
    {
        return shopScript.ChooseRandomShop();
    }

    // Method to find the obstacle that is closest to a collision with a agent given the agents position and two extrapolations
    public GameObject FindClosestObstacle(Vector3 pos, Vector3 movementDir, Vector3 nextNextPos, GameObject target)
    {
        GameObject closest = ClosestShop(pos, movementDir, nextNextPos, target);
        GameObject closestPlanter = ClosestPlanter(pos, movementDir, nextNextPos);
        // If the closest planter is closer than the closest shop we update the overal closest object
        if (closestPlanter != null && (closest == null || (Vector3.Distance(pos, closestPlanter.transform.position) < Vector3.Distance(pos, closest.transform.position))))
        {
            closest = closestPlanter;
        }
        GameObject closestTable = ClosestTable(pos, movementDir, nextNextPos);
        // If the closest table is closer than the previously closest collision object we update the closest collision object to the table
        if (closestTable != null && (closest == null || (Vector3.Distance(pos, closestTable.transform.position) < Vector3.Distance(pos, closest.transform.position))))
        {
            closest = closestTable;
        }  

        // If there is a target, and it is closer than the closest collision object, we can safely move in the cirrent direction so we remove the obstacle
        // (effectively removing the obstacle avoidance force)
        if (target != null && closest != null && Vector3.Distance(target.transform.position, pos) < Vector3.Distance(closest.transform.position, pos))
        {
            closest = null;
        }      

        return closest;
    }

    private GameObject ClosestShop(Vector3 pos, Vector3 nextPos, Vector3 nextNextPos, GameObject target)
    {
        GameObject closest = null;
        foreach (GameObject shop in shopScript.GetShops())   
        {
            if (shop.Equals(target)) continue; // Don't consider target shop as a collison
            // Checking if the agent already is, or will be in any of the extrapolations within the approx radius of the shop, signalising a collision
            if (Vector3.Distance(shop.transform.position, pos) <= 3 || Vector3.Distance(shop.transform.position, nextPos) <= 3 || Vector3.Distance(shop.transform.position, nextNextPos) <= 3)
            {
                // Only care if the collision is the closest one we have found so far..
                if (closest != null && (Vector3.Distance(closest.transform.position, pos) > Vector3.Distance(shop.transform.position, pos)))
                {
                    closest = shop;
                }
                // .. or if we haven't found any other collisions yet
                else if (closest == null)
                {
                    closest = shop;
                }
            }
        } 
        return closest;
    }

    // Method to find the closest planter collision (if any) to a given position and two extrapolations
    private GameObject ClosestPlanter(Vector3 pos, Vector3 nextPos, Vector3 nextNextPos)
    {
        GameObject closest = null;
        foreach (GameObject planter in seatingScript.GetPlanters())
        {
            // Checking if the agent already is, or will be in any of the extrapolations within the max radius of a planter, signalising a collision
            if (Vector3.Distance(planter.transform.position, pos) <= 3f || Vector3.Distance(planter.transform.position, nextPos) <= 3f || Vector3.Distance(planter.transform.position, nextNextPos) <= 3f)
            {
                // Only care if the collision is the closest one we have found so far..
                if (closest != null && (Vector3.Distance(closest.transform.position, pos) > Vector3.Distance(planter.transform.position, pos)))
                {
                    closest = planter;
                }
                // .. or if we haven't found any other collisions yet
                else if (closest == null)
                {
                    closest = planter;
                }
            }
        }
        return closest;
    }

    // Method to find the closest table collision (if any) to a given position and two extrapolations
    private GameObject ClosestTable(Vector3 pos, Vector3 nextPos, Vector3 nextNextPos)
    {
        GameObject closest = null;
        foreach (GameObject table in seatingScript.GetTables())
        {
            // Checking if the agent already is, or will be in any of the extrapolations within the radius of the table, signalising a collision
            if (Vector3.Distance(table.transform.position, pos) <= 4.5f || Vector3.Distance(table.transform.position, nextPos) <= 4.5f || Vector3.Distance(table.transform.position, nextNextPos) <= 4.5f)
            {
                // Only care if the collision is the closest one we have found so far..
                if (closest != null && (Vector3.Distance(closest.transform.position, pos) > Vector3.Distance(table.transform.position, pos)))
                {
                    closest = table;
                }
                // .. or if we haven't found any other collisions yet
                else if (closest == null)
                {
                    closest = table;
                }
            }
        } 
        return closest;
    }

    // Method to calculate separation force for the shopper given as the argument
    public Vector3 CalculateSeparation(GameObject currShopper)
    {
        Vector3 combinedForce = new Vector3(0,0,0);

        foreach (GameObject shopper in shopperScript.GetShoppers())
        {
            if (shopper.Equals(currShopper)) continue; // Don't consider the given shopper
            float dist = Vector3.Distance(currShopper.transform.position, shopper.transform.position);
            if (dist <=1.3f) // Only add the force if the shopper is very close to the given shopper
            {
                float dx = currShopper.transform.position.x - shopper.transform.position.x;
                float dz = currShopper.transform.position.z - shopper.transform.position.z;
                combinedForce += (new Vector3(dx, 0, dz)) / dist; // Weighting by a factor of the distance (so that large distances gives small weight)
            }
        }
        return combinedForce;
    }

    // Method to find a free seat in the food court
    public GameObject FindFreeSeat()
    {
        foreach (GameObject table in seatingScript.GetTables())
        {
            foreach (GameObject chair in table.GetComponent<ChairGenerator>().GetChairs())
            {
                if (!chair.GetComponent<ChairProps>().GetOccupied()) // No shopper has claimed this seat
                {
                    chair.GetComponent<ChairProps>().SetOccupied(true); // Claim the seat so only one shopper can be at the seat at a time
                    return chair;
                }
            }
        }
        return null; // No free seats in the food court
    }

    // Method to find flyered shoppers within a given radius of the advertiser
    public GameObject FindFlyeredShopper(Vector3 advPos, float obsDist)
    {
        foreach (GameObject shopper in shopperScript.GetShoppers())
        {
            if (shopper.GetComponent<SteeringBehaviour>().IsFlyered()) 
            {
                if (Vector3.Distance(advPos, shopper.transform.position) <= obsDist)
                {
                    return shopper;
                }
            }
        }
        return null;
    }

    


}
