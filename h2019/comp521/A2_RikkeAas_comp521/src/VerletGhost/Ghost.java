package VerletGhost;

import Game.Main;

import java.util.ArrayList;

public class Ghost {

    private float baseAccelerationX = 0.5f;
    private float baseAccelerationY = -0.5f;
    private ArrayList<Point> points;
    private ArrayList<LineConstraint> lineConstraints;
    private int constraintIterations = 3;

    public Ghost(float startX, float startY) {
        points = new ArrayList<>();
        lineConstraints = new ArrayList<>();
        setupGhost(startX, startY);
    }

    private void setupGhost(float startX, float startY) {
        // Adding all the points of the ghost. See VerletGhostDrawing.jpg for details
        points.add(new Point(startX + 10, startY - 45));
        points.add(new Point(startX + 20, startY - 45));
        points.add(new Point(startX, startY - 30));
        points.add(new Point(startX, startY - 40));
        points.add(new Point(startX, startY - 50));
        points.add(new Point(startX + 10, startY - 60));
        points.add(new Point(startX + 20, startY - 60));
        points.add(new Point(startX + 30, startY - 50));
        points.add(new Point(startX + 30, startY - 40));
        points.add(new Point(startX + 30, startY - 30));
        points.add(new Point(startX + 25, startY));
        points.add(new Point(startX + 20, startY - 30));
        points.add(new Point(startX + 15, startY - 10));
        points.add(new Point(startX + 10, startY - 30));
        points.add(new Point(startX + 5, startY - 20));

        for (int i = 2; i < points.size() - 1; i++) {
            points.get(i).setNextPoint(points.get(i+1));
            lineConstraints.add(new LineConstraint(points.get(i), points.get(i+1)));
        }
        points.get(points.size()-1).setNextPoint(points.get(2));
        lineConstraints.add(new LineConstraint(points.get(points.size() - 1), points.get(2)));

        // Adding all non-outline line constraints. See VerletGhostDrawing.jpg for details
        lineConstraints.add(new LineConstraint(points.get(0), points.get(1)));
        lineConstraints.add(new LineConstraint(points.get(0), points.get(5)));
        lineConstraints.add(new LineConstraint(points.get(1), points.get(6)));
        lineConstraints.add(new LineConstraint(points.get(0), points.get(13)));
        lineConstraints.add(new LineConstraint(points.get(1), points.get(11)));
        lineConstraints.add(new LineConstraint(points.get(0), points.get(4)));
        lineConstraints.add(new LineConstraint(points.get(1), points.get(7)));
        lineConstraints.add(new LineConstraint(points.get(3), points.get(8)));

        lineConstraints.add(new LineConstraint(points.get(0), points.get(9)));
        lineConstraints.add(new LineConstraint(points.get(1), points.get(2)));

        lineConstraints.add(new LineConstraint(points.get(5), points.get(14)));
        lineConstraints.add(new LineConstraint(points.get(6), points.get(10)));
        lineConstraints.add(new LineConstraint(points.get(5), points.get(12)));
        lineConstraints.add(new LineConstraint(points.get(6), points.get(12)));

        lineConstraints.add(new LineConstraint(points.get(5), points.get(9)));
        lineConstraints.add(new LineConstraint(points.get(6), points.get(2)));

        lineConstraints.add(new LineConstraint(points.get(5), points.get(7)));
        lineConstraints.add(new LineConstraint(points.get(6), points.get(4)));

        lineConstraints.add(new LineConstraint(points.get(4), points.get(7)));
        lineConstraints.add(new LineConstraint(points.get(2), points.get(9)));


    }

    public ArrayList<Point> getPoints() {
        return points;
    }

    /**
     * Method to update the position of the ghost. The acceleration will be randomly generated for each update, but will
     * always be between 0 and 1. Y acceleration will 70% of the time be negative (meaning upwards), otherwise positive.
     * Random generation of acceleration is to make the ghosts move more naturally and overlap less.
     * @param deltaTime The time since last update in seconds
     */
    public void updatePosition(float deltaTime) {
        float randAcceleration = (float) Math.random();
        float yAcceleration = Math.random() < 0.7 ? -randAcceleration : randAcceleration;
        for (Point point : points) {
            point.updatePosition(deltaTime, randAcceleration, yAcceleration);
        }
    }

    /**
     * Method to iterate a given number of times through the constraints of the ghost, first line, then boundary
     */
    public void solveConstraints() {
        for (int i = 0; i < constraintIterations; i++) {
            for (LineConstraint constraint : lineConstraints) {
                constraint.relaxLineConstraint();
            }
            for (Point point : points) {
                point.relaxBoundaryConstraints();
            }

        }
    }

    /**
     * Method to check if any of the points of the ghost are past the right boundary of the screen.
     * @return True if any of the points have moved past the right boundary of the screen, False otherwise
     */
    public boolean readyToDeSpawn() {
        for (Point point : points) {
            if (point.getxPos() > Main.width) return true;
        }
        return false;
    }

}
