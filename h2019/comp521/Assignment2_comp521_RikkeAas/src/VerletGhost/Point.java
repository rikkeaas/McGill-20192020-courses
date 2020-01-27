package VerletGhost;

import Game.Main;
import StoneHenge.StoneHenge;

public class Point {

    private float xPos, yPos;
    private float prevXPos, prevYPos;
    private Point nextPoint = null;

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

    public void updatePosition(float deltaTime, float accX, float accY) {
        float nextX = 2 * xPos - prevXPos + accX * deltaTime * deltaTime;
        float nextY = 2 * yPos - prevYPos + accY * deltaTime * deltaTime;

        prevXPos = xPos;
        prevYPos = yPos;
        xPos = nextX;
        yPos = nextY;
    }

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
