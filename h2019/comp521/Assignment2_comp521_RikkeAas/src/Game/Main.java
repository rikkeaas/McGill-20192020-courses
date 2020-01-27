package Game;

import Canon.CanonBallHandler;
import Canon.Canon;
import StoneHenge.StoneHenge;
import Cloud.Cloud;
import VerletGhost.GhostHandler;
import Wind.Wind;
import processing.core.PApplet;

public class Main {

    public static int width = 800, height = 600;
    public static int groundOffset = 50;
    private static StoneHenge stoneHenge;
    private static Cloud cloud;
    private static Wind wind;
    private static Canon canon;
    private static CanonBallHandler canonBallHandler;
    private static GhostHandler ghosts;

    public static void main(String[] args) {
        stoneHenge = new StoneHenge(width / 2  - groundOffset, height - groundOffset, 100, 30);
        cloud = new Cloud(width / 2f, 100, 200, 50);
        wind = new Wind(6);
        canon = new Canon(4 * width/5, height - groundOffset, 15, 35, -4);
        canonBallHandler = new CanonBallHandler(4 * width/5f, height - groundOffset, 20);
        ghosts = new GhostHandler(width/4f, height - Main.groundOffset);
        PApplet.main("Game.Graphics", args);
    }

    public static StoneHenge getStoneHenge() {
        return stoneHenge;
    }

    public static Cloud getCloud() {
        return cloud;
    }

    public static Wind getWind() {
        return wind;
    }

    public static Canon getCanon() {
        return canon;
    }

    public static CanonBallHandler getCanonBallHandler() {
        return canonBallHandler;
    }

    public static GhostHandler getGhostHandler() {
        return ghosts;
    }

    public static void shoot(float initialAngle) {
        canonBallHandler.generateCanonBall(initialAngle);
    }
}
