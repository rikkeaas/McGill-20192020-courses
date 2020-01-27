package Cloud;

import java.util.ArrayList;

/**
 * Container class for the parts making up the cloud. Makes sure all the parts move synchronized and takes care of wrapping
 * logic when the cloud leaves the screen in either direction.
 */
public class Cloud {

    private ArrayList<CloudPuff> cloudPuffs = new ArrayList<>(); // List of the
    private float xPos; // We only need to keep track of the x coord because the y coord of the cloud is constant

    public Cloud(float xPos, float yPos, int width, int height) {
        this.xPos = xPos;
        cloudPuffs.add(new CloudPuff(xPos, yPos, width, height));
        cloudPuffs.add(new CloudPuff(xPos - width / 4f, yPos - height / 3f, width / 3, height / 2));
        cloudPuffs.add(new CloudPuff(xPos + width / 4f, yPos - height / 3f, width / 3, height / 2));
        cloudPuffs.add(new CloudPuff(xPos, yPos - height / 2f, width / 3, 2*height / 3));
    }

    /**
     * Method to get the list containing the parts of the cloud
     * @return The list of cloud puffs (parts of the cloud)
     */
    public ArrayList<CloudPuff> getCloudPuffs() {
        return cloudPuffs;
    }

    /**
     * Method to move all the parts of the cloud by a given float. Also checks if cloud will move outside screen and if
     * this is the case it wraps the cloud to the other side of the screen.
     * @param deltaX The given value to move each part of the cloud
     * @param windowWidth The width of the screen/window
     */
    public void move(float deltaX, int windowWidth) {
        if (xPos + deltaX < 0) { // Cloud is moving too far to the left
            deltaX += windowWidth;
        }
        else if (xPos + deltaX > windowWidth) { // Cloud is moving too far right
            deltaX -= windowWidth;
        }
        xPos += deltaX; // Updating general position of cloud

        for (CloudPuff cloudPuff : cloudPuffs) { // Moving all the cloud puffs
            cloudPuff.changexPos(deltaX);
        }
    }
}
