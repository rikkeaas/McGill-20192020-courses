using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSeatingArea : MonoBehaviour
{
    public GameObject table;
    public GameObject planter;

    private List<GameObject> tables = new List<GameObject>();
    private List<GameObject> planters = new List<GameObject>();

    void Start()
    {
        GenerateTables();
        GeneratePlanters();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Remove food courd and regenerate
        {
            foreach (GameObject currTable in tables)
            {
                Destroy(currTable);
            }
            foreach (GameObject currPlanter in planters)
            {
                Destroy(currPlanter);   
            }
            Start();
        }
    }

    private void GenerateTables()
    {
        tables = new List<GameObject>();

        int amount = UnityEngine.Random.Range(3, 5);

        float tableX = UnityEngine.Random.Range(-15, 20);
        float tableZ = UnityEngine.Random.Range(-8, 8);

        tables.Add(Instantiate(table, new Vector3(tableX, 0, tableZ), Quaternion.identity));

        while(tables.Count < amount)
        {
            tableX = UnityEngine.Random.Range(-15, 20);
            tableZ = UnityEngine.Random.Range(-8, 8);

            if (!OverlappingWithSomething(new Vector3(tableX, 0, tableZ))) 
            {
                tables.Add(Instantiate(table, new Vector3(tableX, 0, tableZ), Quaternion.identity));
            }
        }
    }

    private void GeneratePlanters()
    {
        planters = new List<GameObject>();

        int amount = UnityEngine.Random.Range(2, 6);

        int counter = 0;
        while (planters.Count < amount && counter < 100)
        {
            float xScale = UnityEngine.Random.Range(1f, 3f); // Random scale
            float zScale = UnityEngine.Random.Range(1f, 3f);
            float planterX = UnityEngine.Random.Range(-15, 20); // And random position
            float planterZ = UnityEngine.Random.Range(-8, 8);

            if (!OverlappingWithSomething(new Vector3(planterX, 0, planterZ)))
            {
                GameObject newPlanter = Instantiate(planter, new Vector3(planterX, 0, planterZ), Quaternion.identity);
                newPlanter.transform.localScale = new Vector3(xScale, newPlanter.transform.localScale.y, zScale);
                planters.Add(newPlanter);
            }
            else 
            {
                counter++;
            }
        }
    }

    // Method to check if new food court object will overlap with existing food court objects
    private bool OverlappingWithSomething(Vector3 newPos)
    {
        foreach (GameObject currTable in tables)
        {
            if (Vector3.Distance(currTable.transform.position, newPos) < 6.3f)
            {
                return true;
            }
        }
        foreach (GameObject currPlanter in planters)
        {
            if (Vector3.Distance(currPlanter.transform.position, newPos) < 6f)
            {
                return true;
            }
        }
        return false;
    }

    public List<GameObject> GetPlanters()
    {
        return planters;
    }

    public List<GameObject> GetTables()
    {
        return tables;
    }

}
