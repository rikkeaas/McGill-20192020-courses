package StoneHenge;

import java.util.ArrayList;

public class StoneHenge {

    private ArrayList<Stone> stones = new ArrayList<>();
    static int noiseRange = 3;
    private int minX, maxX; // Boundaries in x axis of the stonehenge
    private int minY;

    public StoneHenge(int posX, int posY, int longEdge, int shortEdge) {
        posY -= noiseRange;
        stones.add(new Stone(shortEdge, longEdge, posX, posY));
        stones.add(new Stone(shortEdge, longEdge, posX + (longEdge - shortEdge), posY));
        stones.add(new Stone(longEdge, shortEdge, posX, posY - longEdge - noiseRange));

        minX = posX;
        maxX = posX + longEdge;
        minY = posY - longEdge - shortEdge - noiseRange;
    }

    public ArrayList<Stone> getStoneHenge() {
        return stones;
    }

    public int getMinX() {
        return minX;
    }

    public int getMaxX() {
        return maxX;
    }

    public int getMinY() {
        return minY;
    }

}
