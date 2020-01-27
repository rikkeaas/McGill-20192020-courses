package VerletGhost;

import Game.Main;
import StoneHenge.StoneHenge;

public class Point {

    private float xPos, yPos;
    private float prevXPos, prevYPos; // Keeping track of the previous position of the point
    private Point nextPoint = null; // Keeping track of one of the neighbouring points in the ghost outline (if point is eye, then this will stay null)

    public Point(float xPos, float yPos) {
        this.xPos = xPos;
        this.yPos = yPos;
        this.prevXPos = xPos;
        this.prevYPos = yPos;
    }

    public float getxPos() {
        return xPos;
    }

    public float getyPos() {
        return yPos;
    }

    public void translateX(float deltaX) {
        xPos += deltaX;
    }

    public void translateY(float deltaY) {
        yPos += deltaY;
    }

    public void setNextPoint(Point nextPoint) {
        this.nextPoint = nextPoint;
    }

    public Point getNextPoint() {
        return nextPoint;
    }

    /**
     * Method to update position of the point. Formula from lecture
     * @param deltaTime The time in seconds since last update
     * @param accX Acceleration of point in x direction
     * @param accY Acceleration of point in y direction
     */
    public void updatePosition(float deltaTime, float accX, float accY) {
        float nextX = 2 * xPos - prevXPos + accX * deltaTime * deltaTime;
        float nextY = 2 * yPos - prevYPos + accY * deltaTime * deltaTime;

        prevXPos = xPos;
        prevYPos = yPos;
        xPos = nextX;
        yPos = nextY;
    }

    /**
     * Method to relax the boundary constraints of a point. No point can go into the stonehenge or out of the screen
     * No constraint for the right side of the screen because this is where the ghost despawns (checked in ghost)
     */
    public void relaxBoundaryConstraints() {
        // Screen boundaries
        if (xPos < 0) xPos = 0;
        if (yPos < Main.height / 3) yPos = Main.height / 3f;
        if (yPos > Main.height - Main.groundOffset) yPos = Main.height - Main.groundOffset;

        // Stonehenge boundaries
        StoneHenge stoneHenge = Main.getStoneHenge();
        if (xPos > stoneHenge.getMinX() && xPos < stoneHenge.getMaxX() && yPos > stoneHenge.getMinY()) {
            if (prevXPos <= stoneHenge.getMinX()) {
                xPos = stoneHenge.getMinX();
            }
            else if (prevXPos >= stoneHenge.getMaxX()) {
                xPos = stoneHenge.getMaxX();
            }
            else {
                yPos = stoneHenge.getMinY();
            }
        }
    }

    public void addVelocity(float vX, float vY) {
        prevXPos = xPos;
        xPos += vX;
        prevYPos = yPos;
        yPos += vY;
    }
}
