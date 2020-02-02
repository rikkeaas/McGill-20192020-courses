import java.awt.image.*;
import java.io.*;
import java.util.ArrayList;
import javax.imageio.*;

public class q1 {

    // Parameters
    public static int n = 1;
    public static int width;
    public static int height;
    public static volatile int k;


    static BufferedImage outputimage;
    private static ArrayList<MyThread> threads = new ArrayList<>();

    private static long timer;

    public static void main(String[] args) {
        try {

            // example of reading/parsing an argument
            if (args.length == 4) {
                n = Integer.parseInt(args[2]);
                width = Integer.parseInt(args[0]);
                height = Integer.parseInt(args[1]);
                k = Integer.parseInt(args[3]);
                System.out.println(width + ", " + height + ", " + n + ", " + k);
            }
            else {
                System.out.println("Wrong nb of arguments..");
                return;
            }

            // once we know what size we want we can creat an empty image
            outputimage = new BufferedImage(width,height,BufferedImage.TYPE_INT_ARGB);


            for (int i = 0; i < n; i++) {
                MyThread t = new MyThread();
                threads.add(t);
            }

            timer = System.currentTimeMillis();

            for (MyThread thread : threads) {
                thread.start();
            }

            for (MyThread thread : threads) {
                thread.join();
            }

            // Print time in milliseconds to console
            System.out.println(System.currentTimeMillis() - timer);

            // Write out the image
            File outputfile = new File("outputimage.png");
            ImageIO.write(outputimage, "png", outputfile);

        } catch (Exception e) {
            System.out.println("ERROR " +e);
            e.printStackTrace();
        }
    }

    public static synchronized boolean canDrawSquare(MyThread currThread, int threadWidth, int threadHeight, int threadX, int threadY) {
        for (MyThread thread : threads) {
            if (thread.getId() == currThread.getId()) continue;

            if (thread.willOverlap(threadWidth, threadHeight, threadX, threadY)) return false;
        }

        currThread.rectHeight = threadHeight;
        currThread.rectWidth = threadWidth;
        currThread.startX = threadX;
        currThread.startY = threadY;
        k -= 1;

        return true;
    }


}
