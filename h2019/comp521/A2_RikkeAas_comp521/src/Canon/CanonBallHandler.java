package Canon;

import VerletGhost.Ghost;
import java.util.ArrayList;

/**
 * Class to keep track of all canon balls currently on screen. The actual collision and move logic is handled by the canon ball.
 */
public class CanonBallHandler {

    private ArrayList<CanonBall> canonBalls = new ArrayList<>();
    private float startX, startY;
    private float initialVelocity;

    /**
     * @param startX The x coord all canon balls start at
     * @param startY The y coord all canon balls start at
     * @param initialVelocity The velocity all canon balls start with (includes x and y components)
     */
    public CanonBallHandler(float startX, float startY, float initialVelocity) {
        this.startX = startX;
        this.startY = startY;
        this.initialVelocity = initialVelocity;
    }

    /**
     * Method to generate a new canon ball with an initial angle, and default initial x,y-coords and velocity
     * @param canonAngle The initial angle of the canon ball (Angle of the canon barrel)
     */
    public void generateCanonBall(float canonAngle) {
        float velocityX = (float) (initialVelocity * Math.sin(-canonAngle)); // Calculating x component of velocity
        float velocityY = (float) (initialVelocity * Math.cos(canonAngle)); // Calculating y component of velocity
        canonBalls.add(new CanonBall(startX, startY, velocityX, velocityY));
    }

    public ArrayList<CanonBall> getCanonBalls() {
        return canonBalls;
    }

    /**
     * Method to move all canon balls one step. Also checks if canon balls are outside screen or motionless, and if this is
     * the case removes them from the list of canon balls so they will no longer be rendered.
     * @param windSpeed The current wind speed that should affect the canon balls that are above the stone henge.
     */
    public void moveCanonBalls(float windSpeed) {
        int nbOfCanonBalls = canonBalls.size();
        for (int i = 0; i < nbOfCanonBalls; i++) {
            CanonBall currBall = canonBalls.get(i);
            currBall.move(windSpeed); // Canon ball deals with move and collision logic itself.
            if (currBall.isOutOfBounds() || currBall.isMotionless()) { // Canon ball is no longer relevant and should be removed
                canonBalls.remove(currBall);
                i--;
                nbOfCanonBalls--;
            }
        }
    }

    /**
     * Method to ask each canon ball to check if it will intersect with a given ghost. If a canon ball will intersect it should be removed
     * @param ghost The ghost each canon ball should check if it will collide with
     */
    public void checkForCollisionWithGhost(Ghost ghost) {
        ArrayList<CanonBall> tempBalls = new ArrayList<>(); // List that will contain all the balls not colliding with ghost
        for (CanonBall ball : canonBalls) {
            if (ball.collidingWithGhost(ghost)) continue; // If ball will collide we don't add it to the tempBalls list
            tempBalls.add(ball);
        }
        canonBalls = tempBalls; // Updating list of canon balls so that colliding canon balls are removed
    }

}
