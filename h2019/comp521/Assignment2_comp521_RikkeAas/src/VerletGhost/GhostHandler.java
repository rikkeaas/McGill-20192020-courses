package VerletGhost;

import Game.Main;

import java.util.ArrayList;

public class GhostHandler {

    private ArrayList<Ghost> ghosts = new ArrayList<>();
    private float maxStartX, startY;

    public GhostHandler(float maxStartX, float startY) {
        this.maxStartX = maxStartX;
        this.startY = startY;
        for (int i = 0; i < 4; i++) {
            ghosts.add(generateGhost(maxStartX, startY));
        }
    }

    public void updatePositions(float deltaTime) {
        ArrayList<Ghost>  tempGhostList = new ArrayList<>();
        for (Ghost ghost : ghosts) {
            ghost.updatePosition(deltaTime);
            if (ghost.readyToDeSpawn()) {
                tempGhostList.add(generateGhost(maxStartX, startY));
            }
            else {
                tempGhostList.add(ghost);
            }
        }
        ghosts = tempGhostList;
    }

    public void relaxConstraints() {
        for (Ghost ghost : ghosts) {
            ghost.solveConstraints();
        }
    }

    public ArrayList<Ghost> getGhosts() {
        return ghosts;
    }

    private Ghost generateGhost(float maxStartX, float startY) {
        float randomX = (float) Math.random() * (maxStartX - 70);

        return new Ghost(randomX, startY);
    }
}
