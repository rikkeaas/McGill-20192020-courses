package StoneHenge;

import java.util.ArrayList;

/**
 * Wrapper class for the edges defining a stone of the stonehenge
 */
public class Stone {

    private ArrayList<Edge> edges = new ArrayList<>();

    public Stone(int width, int height, int posX, int posY) {
        edges.add(new Edge(width, false, posX, posY)); // Adding bottom
        edges.add(new Edge(width, false, posX, posY - height)); // Adding top
        edges.add(new Edge(height, true, posY - height, posX)); // Adding left side
        edges.add(new Edge(height, true, posY - height, posX + width)); // Adding right side
    }

    public ArrayList<Edge> getEdges() {
        return edges;
    }
}
