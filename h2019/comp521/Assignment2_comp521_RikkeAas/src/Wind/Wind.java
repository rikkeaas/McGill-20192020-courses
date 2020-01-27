package Wind;

public class Wind {

    private float windSpeed;
    private float maxwindSpeed;

    public Wind(float maxwindSpeed) {
        this.maxwindSpeed = maxwindSpeed;
        windSpeed = randomWind();
    }

    private float randomWind() {
        return (float) Math.random() * (2 * maxwindSpeed) - maxwindSpeed;
    }

    public void newWind() {
        windSpeed = randomWind();
    }

    public float getWindSpeed() {
        return windSpeed;
    }
}
