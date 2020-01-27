package Canon;

import Game.Main;
import VerletGhost.Ghost;
import VerletGhost.Point;

public class CanonBall {

    private float posX, posY;
    private float velocityX, velocityY;
    private float gravity = 0.7f;
    private int radius = 10;
    private int stoneHengeRightCoord = Main.getStoneHenge().getMaxX();
    private int stoneHengeLeftCoord = Main.getStoneHenge().getMinX();
    private int stoneHengeTopCoord = Main.getStoneHenge().getMinY();

    public CanonBall(float posX, float posY, float startVelocityX, float startVelocityY) {
        this.posX = posX;
        this.posY = posY;
        this.velocityX = startVelocityX;
        this.velocityY = startVelocityY;
    }

    /**
     * Method to move the canon ball one step, and also check for collisions.
     * @param windSpeed The wind speed will impact the canon ball if it is over the stone henge
     */
    public void move(float windSpeed) {
        if (collisionWithStonehengeRightEdge(posX + velocityX)) {
            velocityX *= -0.7;
            posX = stoneHengeRightCoord + radius/2f;
        }
        // This case should be theoretically impossible, but is here to ensure no unexpected behaviour
        else if (collisionWithStonehengeLeftEdge(posX + velocityX)) {
            velocityX *= -0.7;
            posX = stoneHengeLeftCoord - radius/2f;
        }
        else if (collisionWithStonehengeTop(posY + velocityY)) {
            velocityY *= -0.7;
            posY = stoneHengeTopCoord - radius/2f;
        }
        // The wind should only impact the canon ball above the stone henge
        else {
            if (posX >= stoneHengeLeftCoord && posX <= stoneHengeRightCoord) {
                posX += windSpeed;
            }
            posX += velocityX;

            velocityY += gravity;
            posY += velocityY;
        }
    }

    public float getPosX() {
        return posX;
    }

    public float getPosY() {
        return posY;
    }

    private boolean collisionWithStonehengeRightEdge(float nextPosX) {
        if (nextPosX - radius/2 <= stoneHengeRightCoord && posX >= stoneHengeRightCoord) {
            return posY > stoneHengeTopCoord;
        }
        return false;
    }

    private boolean collisionWithStonehengeLeftEdge(float nextPosX) {
        if (nextPosX + radius/2 >= stoneHengeLeftCoord && posX <= stoneHengeLeftCoord) {
            return posY > stoneHengeTopCoord;
        }
        return false;
    }

    private boolean collisionWithStonehengeTop(float nextPosY) {
        if (posX > stoneHengeLeftCoord && posX < stoneHengeRightCoord) {
            return nextPosY + radius/2 >= stoneHengeTopCoord && posY <= stoneHengeTopCoord;
        }
        return false;
    }

    /**
     * Method to check if this canon ball will be colliding with a given ghost. Explained in readme.txt under task 5.
     * @param ghost The ghost canon ball might or might not be colling with
     * @return True if collision, false otherwise.
     */
    public boolean collidingWithGhost(Ghost ghost) {
        float nextX = posX + velocityX;
        float nextY = posY + velocityY;

        for (Point point : ghost.getPoints()) {
            if (point.getNextPoint() == null) continue;
            float ghostLineX = point.getxPos() - point.getNextPoint().getxPos();
            float ghostLineY = point.getyPos() - point.getNextPoint().getyPos();
            float currToGhostX = point.getxPos() - posX;
            float currToGhostY = point.getyPos() - posY;
            float nextToGhostX = point.getxPos() - nextX;
            float nextToGhostY = point.getyPos() - nextY;

            float cross1 = ghostLineX * currToGhostY - ghostLineY * currToGhostX;
            float cross2 = ghostLineX * nextToGhostY - ghostLineY * nextToGhostX;

            if (Math.signum(cross1) != Math.signum(cross2)) {
                float ballLineX = posX - nextX;
                float ballLineY = posY - nextY;
                float ghost1ToBallX = posX - point.getxPos();
                float ghost1ToBallY = posY - point.getyPos();
                float ghost2ToBallX = posX - point.getNextPoint().getxPos();
                float ghost2ToBallY = posY - point.getNextPoint().getyPos();

                cross1 = ballLineX * ghost1ToBallY - ballLineY * ghost1ToBallX;
                cross2 = ballLineX * ghost2ToBallY - ballLineY * ghost2ToBallX;

                if (Math.signum(cross1) != Math.signum(cross2)) {
                    point.addVelocity(velocityX, velocityY);
                    point.getNextPoint().addVelocity(velocityX, velocityY);
                    return true;
                }

            }
        }
        return false;
    }

    public boolean isOutOfBounds() {
        return (posX < 0 || posX >= Main.width) || (posY < 0 || posY > Main.height - Main.groundOffset);
    }

    public boolean isMotionless() {
        return velocityX == 0 && velocityY == 0;
    }

    public int getRadius() {
        return radius;
    }
}
