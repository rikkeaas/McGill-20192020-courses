import java.awt.*;
import java.util.Random;

public class MyThread extends Thread {

    int rectWidth = -1;
    int rectHeight = -1;
    int startX;
    int startY;
//    int squareCount = 0;
    private Random rand = new Random();

    public void run() {
        while (q1.k > 0) {
            int randWidth = rand.nextInt(q1.width - 1) + 1;
            int randHeight = rand.nextInt(q1.height - 1) + 1;

            int randStartX = rand.nextInt(q1.width - randWidth);
            int randStartY = rand.nextInt(q1.height - randHeight);

            if (q1.canDrawSquare(this, randWidth, randHeight, randStartX, randStartY)) {
                draw(rectWidth, rectHeight, startX, startY);
                rectWidth = -1;
            }
        }
//        System.out.println("Thread " + this.getId() + " has drawn " + squareCount);
    }

    public boolean willOverlap(int otherWidth, int otherHeight, int otherX, int otherY) {
        if (rectWidth == -1) return false;

        if (otherX + otherWidth > startX && otherX < startX + rectWidth) {
            if (otherY + otherHeight > startY && otherY < startY + rectHeight) {
                return true;
            }
        }
        return false;
    }

    private void draw(int rectWidth, int rectHeight, int startX, int startY) {
//        squareCount += 1;
        Color randCol = new Color(rand.nextInt(255), rand.nextInt(255), rand.nextInt(255));
        int randColInt = randCol.getRGB();

        for (int i = startX; i <= startX + rectWidth; i++) {
            for (int j = startY; j <= startY + rectHeight; j++) {
                if (i == startX || i == startX + rectWidth || j == startY || j == startY + rectHeight) {
                    q1.outputimage.setRGB(i, j, 0xFF000000);
                } else {
                    q1.outputimage.setRGB(i, j, randColInt);
                }
            }
        }
    }

}
