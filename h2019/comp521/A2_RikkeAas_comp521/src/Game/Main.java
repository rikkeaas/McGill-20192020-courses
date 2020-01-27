package Game;

import Canon.CanonBallHandler;
import Canon.Canon;
import StoneHenge.StoneHenge;
import Cloud.Cloud;
import VerletGhost.GhostHandler;
import Wind.Wind;
import processing.core.PApplet;

public class Main {

    public static int width = 800, height = 600; // Dimensions of screen
    public static int groundOffset = 50; // Ground height

    // The different components of the "game"
    private static StoneHenge stoneHenge;
    private static Cloud cloud;
    private static Wind wind;
    private static Canon canon;
    private static CanonBallHandler canonBallHandler;
    private static GhostHandler ghostHandler;

    public static void main(String[] args) {
        // Initializing all components
        stoneHenge = new StoneHenge(width / 2  - groundOffset, height - groundOffset, 100, 30);
        cloud = new Cloud(width / 2f, 100, 200, 50);
        wind = new Wind(6);
        canon = new Canon(4 * width/5, height - groundOffset, 15, 35, -4);
        canonBallHandler = new CanonBallHandler(4 * width/5f, height - groundOffset, 20);
        ghostHandler = new GhostHandler(width/4f, height - Main.groundOffset);

        // Starting the processing app
        PApplet.main("Game.Game", args);
    }

    /**
     * Method to shoot a canon ball at the same angle as the canon
     * @param initialAngle The angle in radians of the canon at the time of shooting
     */
    static void shoot(float initialAngle) {
        canonBallHandler.generateCanonBall(initialAngle);
    }

    /**
     * Method to get the speed of the wind
     * @return The speed of the wind
     */
    static float getWindSpeed() {
        return wind.getWindSpeed();
    }

    /**
     * Method to tell the wind object to generate a new random wind speed
     */
    static void newWindSpeed() {
        wind.newWind();
    }

    // Following methods are getter methods for all the components so that other parts of the project can have access to them

    public static StoneHenge getStoneHenge() {
        return stoneHenge;
    }

    public static Cloud getCloud() {
        return cloud;
    }

    public static Canon getCanon() {
        return canon;
    }

    public static CanonBallHandler getCanonBallHandler() {
        return canonBallHandler;
    }

    public static GhostHandler getGhostHandler() {
        return ghostHandler;
    }

}
