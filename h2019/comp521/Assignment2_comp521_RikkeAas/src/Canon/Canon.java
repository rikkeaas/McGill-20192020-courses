package Canon;

public class Canon {

    private int xPos, yPos;
    private int width, height;
    private float rotation;

    public Canon(int xPos, int yPos, int width, int height, float initialRotation) {
        this.xPos = xPos;
        this.yPos = yPos;
        this.width = width;
        this.height = height;
        this.rotation = initialRotation;
    }

    public void rotate(float deltaRoatation) {
        rotation += deltaRoatation;
        if (rotation > -Math.PI) {
            rotation = (float) -Math.PI;
        }
        if (rotation < -1.5 * Math.PI) {
            rotation = (float) (-1.5 * Math.PI);
        }
    }

    public int getxPos() {
        return xPos;
    }

    public int getyPos() {
        return yPos;
    }

    public int getWidth() {
        return width;
    }

    public int getHeight() {
        return height;
    }

    public float getRotation() {
        return rotation;
    }
}
