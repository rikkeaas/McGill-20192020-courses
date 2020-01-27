package Game;

import Canon.Canon;
import Canon.CanonBall;
import StoneHenge.Edge;
import StoneHenge.Stone;
import StoneHenge.StoneHenge;
import Cloud.Cloud;
import Cloud.CloudPuff;
import VerletGhost.Ghost;
import VerletGhost.Point;
import processing.core.PApplet;

import java.util.ArrayList;

public class Game extends PApplet {

    private int width = Main.width, height = Main.height;
    private long prevWindTime; // Variable storing last time the wind speed was changed
    private long prevTime;

    // Processing method called when the window opens
    public void setup() {
        drawComponents();
        prevWindTime = System.currentTimeMillis();
        prevTime = System.currentTimeMillis();
    }

    // Processing method defining the dimensions of the window
    public void settings() {
        size(width, height);
    }

    // Processing method called at every rendering (30fps)
    public void draw() {
        long currTime = System.currentTimeMillis();
        if (currTime - prevWindTime >= 2000) { // Changing wind speed every 2 seconds
            Main.newWindSpeed();
            prevWindTime = currTime;
        }

        // Checking for collision between ghosts and canon balls
        for (Ghost ghost : Main.getGhostHandler().getGhosts()) {
            Main.getCanonBallHandler().checkForCollisionWithGhost(ghost);
        }

        // Moving all movable components by their different physics implementations
        Main.getCloud().move(Main.getWindSpeed(), width);
        Main.getCanonBallHandler().moveCanonBalls(Main.getWindSpeed());
        Main.getGhostHandler().updatePositions((currTime - prevTime) / 1000f);

        drawComponents(); // Redrawing all components on the screen
        prevTime = currTime;
    }

    // Processing method called every time a key is pressed
    public void keyPressed() {
        if (keyCode == UP) {
            Main.getCanon().rotate(0.1f);
        }
        if (keyCode == DOWN) {
            Main.getCanon().rotate(-0.1f);
        }
        if (key == ' ') {
            Main.shoot(Main.getCanon().getRotation());
        }
    }

    /**
     * Method to remove all components and redraw them on the screen
     */
    private void drawComponents() {
        clear(); // Removing everything from screen
        background(255); // Setting background to white
        stroke(0); // Setting line color to black

        line(0, height - 50, width, height - 50); // Line of ground

        drawStoneHenge();
        drawCloud();
        drawCanon();
        drawCanonBalls();
        drawGhost();
    }

    /**
     * Method to draw cloud object in gray color
     */
    private void drawCloud() {
        stroke(100);
        fill(100);
        Cloud cloud = Main.getCloud();
        for (CloudPuff cloudPuff : cloud.getCloudPuffs()) {
            ellipse(cloudPuff.getxPos(), cloudPuff.getyPos(), cloudPuff.getWidth(), cloudPuff.getHeight());
        }
    }

    /**
     * Method to draw stone henge object. Have to loop through each point for each edge for each stone and draw lines.
     */
    private void drawStoneHenge() {
        stroke(0);
        StoneHenge stonehenge = Main.getStoneHenge();
        for (Stone stone : stonehenge.getStoneHenge()) {
            for (Edge edge : stone.getEdges()) {
                ArrayList<Float> x = edge.getXValues();
                ArrayList<Float> y = edge.getYValues();
                for (int i = 0; i < x.size() - 1; i++) {
                    line(x.get(i), y.get(i), x.get(i + 1), y.get(i + 1));
                }

            }
        }
    }

    /**
     * Method to draw canon object. Nb very simple, only a rectangle with rotation.
     */
    private void drawCanon() {
        stroke(0);
        fill(200);
        Canon canon = Main.getCanon();
        pushMatrix(); // Need this to isolate the rotation of the axis to only drawing the canon (don't want the rest rotated)
        translate(canon.getxPos(), canon.getyPos()); // Setting the (0,0) coord (where the rotation happens) to the position of the canon
        rotate(canon.getRotation());
        rect(0, 0, -canon.getWidth(), canon.getHeight());
        popMatrix(); // Ends rotation of axis (back to normal)

    }

    /**
     * Method to draw all the canon balls that exist
     */
    private void drawCanonBalls() {
        stroke(0);
        fill(50);
        for (CanonBall canonBall : Main.getCanonBallHandler().getCanonBalls()) {
            circle(canonBall.getPosX(), canonBall.getPosY(), canonBall.getRadius());
        }
    }

    /**
     * Method to draw the four ghosts. Each point of each ghost has a reference to the next point in the outline of the ghost.
     * The exceptions are the eyes which have a null value. This way it is easy to identify the eyes and draw them as bigger circles,
     * and draw lines between points to define ghost outlines.
     */
    private void drawGhost() {
        stroke(0);
        for (Ghost ghost : Main.getGhostHandler().getGhosts()) {
            for (Point point : ghost.getPoints()) {
                if (point.getNextPoint() == null) { // The eyes
                    fill(255);
                    circle(point.getxPos(), point.getyPos(), 5);
                } else { // Points of the outline of the ghost
                    fill(0);
                    circle(point.getxPos(), point.getyPos(), 2);
                    line(point.getxPos(), point.getyPos(), point.getNextPoint().getxPos(), point.getNextPoint().getyPos());
                }
            }
        }
    }
}
