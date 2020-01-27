package VerletGhost;

public class LineConstraint {

    private Point point1, point2;
    private float length;

    public LineConstraint(Point point1, Point point2) {
        this.point1 = point1;
        this.point2 = point2;
        this.length = calculateDistance();
    }

    private float calculateDistance() {
        float dx = point1.getxPos() - point2.getxPos();
        float dy = point1.getyPos() - point2.getyPos();
        return (float) Math.sqrt(dx*dx + dy*dy);
    }

    public void relaxLineConstraint() {
        float currDistance = calculateDistance();
        float diffPercentage = (length - currDistance) / currDistance;

        float dx = point1.getxPos() - point2.getxPos();
        float dy = point1.getyPos() - point2.getyPos();

        point1.translateX(dx * diffPercentage / 2f);
        point1.translateY(dy * diffPercentage / 2f);
        point2.translateX(-dx * diffPercentage / 2f);
        point2.translateY(-dy * diffPercentage / 2f);

    }
}
