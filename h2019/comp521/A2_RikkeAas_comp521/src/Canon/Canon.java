package Canon;

/**
 * This class just holds the information of the canon. Not much logic.
 */
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

    /**
     * Method to change the rotation of the canon. The left angle from the canon to the ground should be 0-90 degrees
     * which translates to between -1.5*PI and -PI radians.
     * @param deltaRotation The change in rotation in radians
     */
    public void rotate(float deltaRotation) {
        rotation += deltaRotation;
        if (rotation > -Math.PI) {
            rotation = (float) -Math.PI;
        }
        if (rotation < -1.5 * Math.PI) {
            rotation = (float) (-1.5 * Math.PI);
        }
    }

    // Following methods are getters for the different values defining the canon (position, size, rotation)

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
