package Cloud;

/**
 * Class to hold the position and size of each cloud part so that they can be drawn on screen.
 */
public class CloudPuff {

    private float xPos, yPos;
    private int width, height;

    public CloudPuff(float xPos, float yPos, int width, int height) {
        this.xPos = xPos;
        this.yPos = yPos;
        this.width = width;
        this.height = height;
    }

    public void changexPos(float deltaX) {
        xPos += deltaX;
    }

    public float getxPos() {
        return xPos;
    }

    public float getyPos() {
        return yPos;
    }

    public int getWidth() {
        return width;
    }

    public int getHeight() {
        return height;
    }
}
