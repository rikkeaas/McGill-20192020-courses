using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{

    public MazeCell cellPrefab;
    private MazeCell[,] grid;
    private int sizeX, sizeY;

    public void Generate (int sizeX, int sizeY) {
       
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        grid = new MazeCell[sizeX,sizeY];

        int centerX = (sizeX - 1) / 2;
        int centerY = (sizeY - 1) / 2;
        GenerateCell(centerX, centerY, centerX, centerY);

        List<(int, int, int,int)> neighbours = new List<(int, int, int,int)>(); // Parent x, Parent y, Neighbour x, Neighbour y
        FindNeighbours(centerX, centerY, neighbours);

        while (neighbours.Count > 0) {
            int randIndex = Random.Range(0, neighbours.Count);
            int newX = neighbours[randIndex].Item3;
            int newY = neighbours[randIndex].Item4;
            if (grid[newX, newY] == null) {
                GenerateCell(newX, newY, neighbours[randIndex].Item1, neighbours[randIndex].Item2);
                FindNeighbours(newX, newY, neighbours);
            }
            neighbours.RemoveAt(randIndex);
        }

        RemoveWall("south", 0, sizeY-1);
        // grid[x,y].transform.GetChild(1).gameObject.SetActive(false);

    }

    void FindNeighbours(int x, int y, List<(int, int, int, int)> neighbours) {
        if (x + 1 < sizeX && grid[x+1,y] == null) {
            neighbours.Add((x, y, x+1, y));
        }
        if (x - 1 >= 0 && grid[x-1,y] == null) {
            neighbours.Add((x, y, x-1, y));
        }
        if (y + 1 < sizeY && grid[x,y+1] == null) {
            neighbours.Add((x, y, x, y+1));
        }
        if (y - 1 >= 0 && grid[x,y-1] == null) {
            neighbours.Add((x, y, x, y-1));
        }

    }

    void GenerateCell (int x, int y, int parentX, int parentY) {

        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        grid[x,y] = newCell;
        newCell.name = "Cell " + x + ", " + y;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(x - sizeX/2 + 0.5f, 0f, y - sizeY/2 + 0.5f);
        
        if (x > parentX) {
            RemoveWall("south", x, y);
            RemoveWall("north", parentX, parentY);
        }
        if (x < parentX) {
            RemoveWall("north", x, y);
            RemoveWall("south", parentX, parentY);
        }
        if (y > parentY) {
            RemoveWall("east", x, y);
            RemoveWall("west", parentX, parentY);
        }
        if (y < parentY) {
            RemoveWall("west", x, y);
            RemoveWall("east", parentX, parentY);
        }
    }

    void RemoveWall(string direction, int x, int y) {
        switch (direction)
        {
            case "north": grid[x,y].transform.GetChild(0).gameObject.SetActive(false); break;
            case "east": grid[x,y].transform.GetChild(1).gameObject.SetActive(false); break;
            case "south": grid[x,y].transform.GetChild(2).gameObject.SetActive(false); break;
            case "west": grid[x,y].transform.GetChild(3).gameObject.SetActive(false); break;
            default: break;
        }
    }
}
