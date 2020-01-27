package Wind;

public class Wind {

    private float windSpeed;
    private float maxWindSpeed; // The range of wind speeds will be [-maxWindSpeed, maxWindSpeed]

    public Wind(float maxWindSpeed) {
        this.maxWindSpeed = maxWindSpeed;
        windSpeed = randomWind();
    }

    /**
     * Private method to generate a random wind speed in the range [-maxWindSpeed, maxWindSpeed]
     * @return The randomly generated wind speed
     */
    private float randomWind() {
        return (float) Math.random() * (2 * maxWindSpeed) - maxWindSpeed;
    }

    /**
     * Method other objects can call to make the wind object change its wind speed value
     */
    public void newWind() {
        windSpeed = randomWind();
    }

    /**
     * Method to get the speed of the wind
     * @return The speed of the wind
     */
    public float getWindSpeed() {
        return windSpeed;
    }
}
