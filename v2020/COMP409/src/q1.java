import java.awt.*;
import java.awt.image.*;
import java.io.*;
import java.util.Random;
import javax.imageio.*;

public class q1 {

    // Parameters
    public static int n = 1;
    public static int width;
    public static int height;
    public static int k;

    private static Random rand = new Random();

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
            BufferedImage outputimage = new BufferedImage(width,height,BufferedImage.TYPE_INT_ARGB);

            for(int f = 0; f < 10; f++) {
                Color randCol = new Color(rand.nextInt(255), rand.nextInt(255), rand.nextInt(255));
                int randColInt = randCol.getRGB();
                int randSize = rand.nextInt(80);

                int randStartX = rand.nextInt(width - randSize);
                int randStartY = rand.nextInt(height - randSize);



                for (int i = randStartX; i <= randStartX + randSize; i++) {
                    for (int j = randStartY; j <= randStartY + randSize; j++) {
                        if (i == randStartX || i == randStartX + randSize || j == randStartY || j == randStartY + randSize) {
                            outputimage.setRGB(i, j, 0xFF000000);
                        }
                        else {
                            outputimage.setRGB(i, j, randColInt);
                        }
                    }
                }
            }
            // ------------------------------------
            // Your code would go here
            
            // The easiest mechanisms for getting and setting pixels are the
            // BufferedImage.setRGB(x,y,value) and getRGB(x,y) functions.
            // Note that setRGB is synchronized (on the BufferedImage object).
            // Consult the javadocs for other methods.

            // The getRGB/setRGB functions return/expect the pixel value in ARGB format, one byte per channel.  For example,
            //  int p = img.getRGB(x,y);
            // With the 32-bit pixel value you can extract individual colour channels by shifting and masking:
            //  int red = ((p>>16)&0xff);
            //  int green = ((p>>8)&0xff);
            //  int blue = (p&0xff);
            // If you want the alpha channel value it's stored in the uppermost 8 bits of the 32-bit pixel value
            //  int alpha = ((p>>24)&0xff);
            // Note that an alpha of 0 is transparent, and an alpha of 0xff is fully opaque.
            
            // ------------------------------------
            
            // Write out the image
            File outputfile = new File("outputimage.png");
            ImageIO.write(outputimage, "png", outputfile);

        } catch (Exception e) {
            System.out.println("ERROR " +e);
            e.printStackTrace();
        }
    }
}
