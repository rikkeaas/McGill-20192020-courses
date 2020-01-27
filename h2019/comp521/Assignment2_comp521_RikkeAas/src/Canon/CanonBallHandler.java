package Canon;

import VerletGhost.Ghost;

import java.util.ArrayList;

public class CanonBallHandler {

    private ArrayList<CanonBall> canonBalls = new ArrayList<>();
    private float startX, startY;
    private float initialVelocity;

    public CanonBallHandler(float startX, float startY, float initialVelocity) {
        this.startX = startX;
        this.startY = startY;
        this.initialVelocity = initialVelocity;
    }

    public void generateCanonBall(float canonAngle) {
        float velocityX = (float) (initialVelocity * Math.sin(-canonAngle));
        float velocityY = (float) (initialVelocity * Math.cos(canonAngle));
        canonBalls.add(new CanonBall(startX, startY, velocityX, velocityY));
    }

    public ArrayList<CanonBall> getCanonBalls() {
        return canonBalls;
    }

    public void moveCanonBalls(float windSpeed) {
        int nbOfCanonBalls = canonBalls.size();
        for (int i = 0; i < nbOfCanonBalls; i++) {
            CanonBall currBall = canonBalls.get(i);
            currBall.move(windSpeed);
            if (currBall.isOutOfBounds() || currBall.isMotionless()) {
                canonBalls.remove(currBall);
                i--;
                nbOfCanonBalls--;
            }
        }
    }

    public void checkForCollisionWithGhost(Ghost ghost) {
        ArrayList<CanonBall> tempBalls = new ArrayList<>();
        for (CanonBall ball : canonBalls) {
            if (ball.collidingWithGhost(ghost)) continue;
            tempBalls.add(ball);
        }
        canonBalls = tempBalls;
    }

}
