

public class MyThread extends Thread {



    public void run() {

        q1.draw();
        System.out.println("Thread " + Thread.currentThread().getId() + " finished drawing");

    }

}
