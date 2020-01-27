package Cloud;

import java.util.ArrayList;

public class Cloud {

    private ArrayList<CloudPuff> cloudPuffs = new ArrayList<>();
    private float xPos, yPos;

    public Cloud(float xPos, float yPos, int width, int height) {
        this.xPos = xPos;
        this.yPos = yPos;
        cloudPuffs.add(new CloudPuff(xPos, yPos, width, height));
        cloudPuffs.add(new CloudPuff(xPos - width / 4f, yPos - height / 3f, width / 3, height / 2));
        cloudPuffs.add(new CloudPuff(xPos + width / 4f, yPos - height / 3f, width / 3, height / 2));
        cloudPuffs.add(new CloudPuff(xPos, yPos - height / 2f, width / 3, 2*height / 3));
    }

    public ArrayList<CloudPuff> getCloudPuffs() {
        return cloudPuffs;
    }

    public void move(float deltaX, int windowWidth) {
        if (xPos + deltaX < 0) {
            deltaX += windowWidth;
        }
        else if (xPos + deltaX > windowWidth) {
            deltaX -= windowWidth;
        }

        xPos += deltaX;
        for (CloudPuff cloudPuff : cloudPuffs) {
            cloudPuff.changexPos(deltaX);
        }
    }
}
