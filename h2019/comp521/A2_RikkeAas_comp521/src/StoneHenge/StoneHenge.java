package StoneHenge;

import java.util.ArrayList;

public class StoneHenge {

    private ArrayList<Stone> stones = new ArrayList<>();
    static int noiseRange = 3; // The range of the first octave of the perlin noise (range is [-noiseRange,noiseRange])
    // We need the following boundaries for easy collision detection
    private int minX, maxX; // Boundaries in x axis of the stonehenge
    private int minY; // Boundary of the top of the stonehenge

    public StoneHenge(int posX, int posY, int longEdge, int shortEdge) {
        posY -= noiseRange;
        stones.add(new Stone(shortEdge, longEdge, posX, posY));
        stones.add(new Stone(shortEdge, longEdge, posX + (longEdge - shortEdge), posY));
        stones.add(new Stone(longEdge, shortEdge, posX, posY - longEdge - noiseRange));

        minX = posX;
        maxX = posX + longEdge;
        minY = posY - longEdge - shortEdge - noiseRange; // Subtracting noiseRange since we did this for the y coord of the top stone
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
