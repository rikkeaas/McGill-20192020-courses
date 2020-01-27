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
import Wind.Wind;
import processing.core.PApplet;

import java.util.ArrayList;

public class Graphics extends PApplet {

    private int width = Main.width, height = Main.height;
    private long prevCloudTime;
    private long prevTime;
    private Wind wind = Main.getWind();

    public void setup() {
        drawComponents();
        prevCloudTime = System.currentTimeMillis();
        prevTime = System.currentTimeMillis();
    }

    private void drawComponents() {
        clear();
        background(255);

        stroke(0);

        line(0, height - 50, width, height - 50);

        drawStoneHenge();
        drawCloud();
        drawCanon();
        drawCanonBalls();
        drawGhost();
    }

    public void draw() {
        long currTime = System.currentTimeMillis();
        if (currTime - prevCloudTime >= 2000) {
            wind.newWind();
            prevCloudTime = currTime;
        }

        for (Ghost ghost : Main.getGhostHandler().getGhosts()) {
            Main.getCanonBallHandler().checkForCollisionWithGhost(ghost);
        }

        Main.getCloud().move(wind.getWindSpeed(), width);
        Main.getCanonBallHandler().moveCanonBalls(wind.getWindSpeed());
        Main.getGhostHandler().updatePositions((currTime - prevTime) / 1000f);
        Main.getGhostHandler().relaxConstraints();
        prevTime = currTime;

        drawComponents();

    }

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

    private void drawCloud() {
        stroke(100);
        fill(100);
        Cloud cloud = Main.getCloud();
        for (CloudPuff cloudPuff : cloud.getCloudPuffs()) {
            ellipse(cloudPuff.getxPos(), cloudPuff.getyPos(), cloudPuff.getWidth(), cloudPuff.getHeight());
        }
    }

    private void drawStoneHenge() {
        stroke(0);
        fill(200);
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

    private void drawCanon() {
        stroke(0);
        fill(200);
        Canon canon = Main.getCanon();
        pushMatrix();
        translate(canon.getxPos(), canon.getyPos());
        rotate(canon.getRotation());
        rect(0, 0, -canon.getWidth(), canon.getHeight());
        popMatrix();

    }

    private void drawCanonBalls() {
        stroke(0);
        fill(50);
        for (CanonBall canonBall : Main.getCanonBallHandler().getCanonBalls()) {
            circle(canonBall.getPosX(), canonBall.getPosY(), canonBall.getRadius());
        }
    }

    private void drawGhost() {

        for (Ghost ghost : Main.getGhostHandler().getGhosts()) {
            stroke(0);
            for (Point point : ghost.getPoints()) {
                if (point.getNextPoint() == null) {
                    fill(255);
                    circle(point.getxPos(), point.getyPos(), 5);
                } else {
                    fill(0);
                    circle(point.getxPos(), point.getyPos(), 2);
                    line(point.getxPos(), point.getyPos(), point.getNextPoint().getxPos(), point.getNextPoint().getyPos());
                }
            }
        }
    }

    public void settings() {
        size(width, height);
    }
}
