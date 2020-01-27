package VerletGhost;

import Game.Main;

import java.util.ArrayList;

public class GhostHandler {

    private ArrayList<Ghost> ghosts = new ArrayList<>();
    private float maxStartX, startY; // All ghosts start at ([0,maxStartX], startY)

    public GhostHandler(float maxStartX, float startY) {
        this.maxStartX = maxStartX;
        this.startY = startY;
        for (int i = 0; i < 4; i++) { // Generating the four initial ghosts
            ghosts.add(generateGhost(maxStartX, startY));
        }
    }

    /**
     * Method to update the position of each ghost. Method also calls the method to relax constraints.
     * @param deltaTime The time step since last update
     */
    public void updatePositions(float deltaTime) {
        ArrayList<Ghost>  tempGhostList = new ArrayList<>();
        for (Ghost ghost : ghosts) {
            ghost.updatePosition(deltaTime);
            if (ghost.readyToDeSpawn()) { // Checking if ghost is leaving screen on right side, if so generating a new ghost
                tempGhostList.add(generateGhost(maxStartX, startY));
            }
            else { // Will only add ghost if it is not leaving screen on right side
                tempGhostList.add(ghost);
            }
        }
        ghosts = tempGhostList; // Updating list of ghosts so that ghosts that are leaving screen are replaced by new ghosts.

        relaxConstraints();
    }

    /**
     * Method to make all the ghosts relax/solve their constraints
     */
    private void relaxConstraints() {
        for (Ghost ghost : ghosts) {
            ghost.solveConstraints();
        }
    }

    public ArrayList<Ghost> getGhosts() {
        return ghosts;
    }

    /**
     * Method to generate a ghost at a random x position between 0 and maxStartX
     * @param maxStartX The upper bound for generating a ghost in the x direction
     * @param startY The starting y coordinate of the bottom of ghosts
     * @return The new ghost
     */
    private Ghost generateGhost(float maxStartX, float startY) {
        float randomX = (float) Math.random() * (maxStartX - 70);
        return new Ghost(randomX, startY);
    }
}
