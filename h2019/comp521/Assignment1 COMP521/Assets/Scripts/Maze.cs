using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject pathIndicator;
    public int winX, winY;

    private GameObject[,] mazeGrid; // The actual physical maze
    private bool[,][] mainLogicGrid; // Storing the initial maze (where we find the correct path player could take to win)
    private bool[,][] currentLogicGrid; // Maze at the current time dimension
    private List<(int,int)> path; // List of coordinates of path from start position to the win cell
    private GameObject nextStep;
    private int mazeSizeX = 8, mazeSizeY = 8;
    private int playerStartX = 0, playerStartY = 0;
    private (int,int)[] directions = new (int,int)[]{(0,1),(1,0),(0,-1),(-1,0)}; // [north, east, south, west]


    void Start()
    {
        mainLogicGrid = new bool[mazeSizeX, mazeSizeY][]; // Representing each direction for each cell [xCoord, yCoord, [north, east, south, west]]
        mazeGrid = new GameObject[mazeSizeX, mazeSizeY];
        Initialise(); // build first maze
        path = new List<(int,int)>();

        while(!ValidPathExists(playerStartX, playerStartY, 0)) // if no valid path, build new first maze
        {
            mainLogicGrid = new bool[mazeSizeX, mazeSizeY][];
            Initialise();
        }

        AddCellsToPhysicalMaze(); 
        SetUpWalls(); // Building the physical maze
        mazeGrid[0,0].transform.GetChild(2).gameObject.SetActive(false); // Removing south wall of start cell so player can enter
    }

    void AddCellsToPhysicalMaze() 
    { 
        for (int x = 0; x < mazeSizeX; x++)
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                GameObject newCell = Instantiate(cellPrefab);
                mazeGrid[x,y] = newCell;
                newCell.name = "Cell " + x + ", " + y;
                newCell.transform.parent = transform;
                newCell.transform.localPosition = new Vector3(x*5, 0f, y*5);
            }
        }
    }

    void SetUpWalls()
    {
        for (int x = 0; x < mazeSizeX; x++)
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                for (int direction = 0; direction < 4; direction++) 
                {
                    if (!currentLogicGrid[x,y][direction]) mazeGrid[x,y].transform.GetChild(direction).gameObject.SetActive(true); // You can't go this direction (N,E,S,W)
                    else mazeGrid[x,y].transform.GetChild(direction).gameObject.SetActive(false); // You can go in this direction
                }
            }
        }
    }
    
    void Initialise()
    {
        // Tuple is organized like this: (Parent x, Parent y, Neighbour x, Neighbour y)
        List<(int, int, int,int)> neighbours = new List<(int, int, int,int)>();
        currentLogicGrid = new bool[mazeSizeX, mazeSizeY][];
        // Staring maze generation from where player starts
        currentLogicGrid[0,0] = new bool[]{false, false, false, false};
        FindNeighbours(0, 0, neighbours);
        BuildMaze(neighbours);
        CopyLogicGrid(mainLogicGrid, currentLogicGrid);
    }

    // help method, copies maze from logic grod fromGrid to logic grid toGrid
    void CopyLogicGrid(bool[,][] toGrid, bool[,][]fromGrid)
    {
        for (int x = 0; x < mazeSizeX; x++) 
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                toGrid[x,y] = new bool[4];
                for (int direction = 0; direction < 4; direction++)
                {
                    toGrid[x,y][direction] = fromGrid[x,y][direction];
                }
            }
        }
    }

    // implementation of prim's algorithm
    void BuildMaze(List<(int, int, int, int)> neighbours)
    {
        while (neighbours.Count > 0)
        {
            int randIndex = Random.Range(0, neighbours.Count); // Chosing random neighbour to step towards
            int newX = neighbours[randIndex].Item3;
            int newY = neighbours[randIndex].Item4;
            if (currentLogicGrid[newX, newY] == null) // We haven't stepped here before
            { 
                AddPathFromParentToChild(newX, newY, neighbours[randIndex].Item1, neighbours[randIndex].Item2);
                FindNeighbours(newX, newY, neighbours);
            }
            neighbours.RemoveAt(randIndex);
        }
    }
    
    // finds non visited valid neighbours to cell with coordinates x y
    void FindNeighbours(int x, int y, List<(int, int, int, int)> neighbours) 
    {
        if (x + 1 < mazeSizeX && currentLogicGrid[x+1,y] == null) {
            neighbours.Add((x, y, x+1, y));
        }
        if (x - 1 >= 0 && currentLogicGrid[x-1,y] == null) {
            neighbours.Add((x, y, x-1, y));
        }
        if (y + 1 < mazeSizeY && currentLogicGrid[x,y+1] == null) {
            neighbours.Add((x, y, x, y+1));
        }
        if (y - 1 >= 0 && currentLogicGrid[x,y-1] == null) {
            neighbours.Add((x, y, x, y-1));
        }
    }

    // implementation of depth first search to check if path exists, and add path coordinates if exists
    bool ValidPathExists(int x, int y, int count) 
    {
        if (count > 16) return false; // path too long

        path.Add((x,y));
        if (x == winX && y == winY) return true; // arrived at destination

        for (int i = 0; i < 4; i++) // go through i'th direction
        {
            if (!mainLogicGrid[x,y][i]) continue; // can't go in this direction
            int newX = x + directions[i].Item1;  
            int newY = y + directions[i].Item2;
            if (!path.Contains((newX,newY))) // haven't been here before 
            {
                if (ValidPathExists(newX, newY, count + 1)) return true;
            }
        }
        path.RemoveAt(path.Count - 1); // backtracking
        return false;
    }

    void AddPathFromParentToChild (int x, int y, int parentX, int parentY) 
    {
        currentLogicGrid[x,y] = new bool[4]{false, false, false, false};
       
        if (y > parentY) 
        {
            currentLogicGrid[x,y][2] = true; // Can go south from new cell to parent cell
            currentLogicGrid[parentX,parentY][0] = true; // Can go north from parent cell to new cell
        }
        else if (y < parentY) 
        {
            currentLogicGrid[x,y][0] = true; // Can go north from new cell to parent cell
            currentLogicGrid[parentX,parentY][2] = true; // Can go south from parent cell to new cell
        }
        else if (x > parentX) 
        {
            currentLogicGrid[x,y][3] = true; // Can go west from new cell to parent cell
            currentLogicGrid[parentX,parentY][1] = true; // Can go east from parent cell to new cell
        }
        else if (x < parentX) 
        {
            currentLogicGrid[x,y][1] = true; // Can go east from new cell to parent cell
            currentLogicGrid[parentX,parentY][3] = true; // Can go west from parent cell to new cell
        }
    }

    // step maze in time dimension
    public void TimeStep(int time)
    {
        List<(int, int, int, int)> neighbours = new List<(int, int, int, int)>();
        currentLogicGrid = new bool[mazeSizeX, mazeSizeY][];
        if (time+1 > path.Count-1) // Path is done but player is still moving
        {
            currentLogicGrid[0, 0] = new bool[]{false, false, false, false};
            FindNeighbours(0, 0, neighbours);
        }
        else // Want to make sure it is possible to follow "intended" path
        {
            int currX = path[time].Item1, currY = path[time].Item2;
            int nextX = path[time+1].Item1, nextY = path[time+1].Item2;
            currentLogicGrid[currX, currY] = new bool[]{false, false, false, false};
            AddPathFromParentToChild(nextX, nextY, currX, currY); // Adding path between current and next part of solution path
            FindNeighbours(currX, currY, neighbours);
            FindNeighbours(nextX, nextY, neighbours);
        }

        BuildMaze(neighbours);
        SetUpWalls();
    }

    public void IndicateNextStep(int time)
    {
        if (nextStep != null) Destroy(nextStep); // Remove previous step indicator
        if (time > path.Count-1) return; // The whole path has already been shown
        int nextX = path[time].Item1, nextY = path[time].Item2;
        nextStep = Instantiate(pathIndicator);
        nextStep.transform.parent = transform;
        nextStep.transform.localPosition = new Vector3(nextX*5 + 2.5f, 1.5f, nextY*5 + 2.5f);
    }

    public bool IsWinningTile(int x, int y)
    {
        return x == winX && y == winY;
    }

    public void ResetMaze() 
    {
        CopyLogicGrid(currentLogicGrid, mainLogicGrid);
        if (nextStep != null) Destroy(nextStep);
        SetUpWalls();
    }
}
