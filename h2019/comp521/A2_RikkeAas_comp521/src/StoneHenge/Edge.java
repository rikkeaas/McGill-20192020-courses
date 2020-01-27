package StoneHenge;

import java.util.ArrayList;

public class Edge {

    private static int initialDistBetwenPoints = 24;
    private static int nbOfOctaves = 4;
    private static int divisor = 2;

    // Following two lists can be either x or y coords, depending on whether it is a vertical or horizontal edge
    // For vertical edges noiseValues wil be x coords and pointValues are y coords, opposite for horizontal edges
    private ArrayList<Float> noiseValues;
    private ArrayList<Float> pointValues;
    private boolean vertical; // True if the edge is a vertical edge, false otherwise

    public Edge(int length, boolean vertical, int startPoint, int startNoise) {
        this.vertical = vertical;

        // Generating all perlin noise octaves, then adding them together to the complete noise values
        ArrayList<ArrayList<Float>> noiseOctaves = generateAllOctaves(StoneHenge.noiseRange, initialDistBetwenPoints, nbOfOctaves, length);
        noiseValues = combineOctaves(noiseOctaves);

        // Adding corresponding x or y coordinates for each noise value
        pointValues = new ArrayList<>();
        float point = 0;
        float distBetweenPoints = ((float) length) / ((float) noiseValues.size());
        while(pointValues.size() != noiseValues.size()) {
            pointValues.add(point);
            point += distBetweenPoints;
        }

        // Adding the noise and point values to the initial x and y coords (start point of line)
        addStartPosition(noiseValues, startNoise);
        addStartPosition(pointValues, startPoint);

        // Setting first and last noise values to the "base" value (start point) so that corners of each edge will meet up
        noiseValues.set(0, (float) startNoise);
        noiseValues.set(noiseValues.size() - 1, (float) startNoise);
    }

    /**
     * Method to add a value (start position) to every value in a list
     * @param values The list we want to add to
     * @param startPosition The value we want to add to every value of the list
     */
    private void addStartPosition(ArrayList<Float> values, int startPosition) {
        for (int i = 0; i < values.size(); i++) {
            values.set(i, values.get(i) + startPosition);
        }
    }

    /**
     * Method to get the x values of an edge (depending on whether the edge is vertical or horizontal)
     * @return The list of x values, noiseValues if vertical edge, pointValues if horizontal edge
     */
    public ArrayList<Float> getXValues() {
        if (vertical) return noiseValues;
        else return pointValues;
    }

    /**
     * Method to get the y values of an edge (depending on whether the edge is vertical or horizontal)
     * @return The list of y values, pointValues if vertical edge, noiseValues if horizontal edge
     */
    public ArrayList<Float> getYValues() {
        if (vertical) return pointValues;
        else return noiseValues;
    }

    /**
     * Method to add together all the octaves to get the complete perlin noise values
     * @param noiseOctaves List of the different noise octaves
     * @return List of the combined perlin noise of each point
     */
    private ArrayList<Float> combineOctaves(ArrayList<ArrayList<Float>> noiseOctaves) {
        ArrayList<Float> combinedOctaves = new ArrayList<>();
        for (int i = 0; i < noiseOctaves.get(0).size(); i++) { // All octaves have same size (bigger octaves have already been interpolated)
            float totalNoise = 0;
            for (ArrayList<Float> noiseOctave : noiseOctaves) {
                totalNoise += noiseOctave.get(i);
            }
            combinedOctaves.add(totalNoise);
        }
        return combinedOctaves;
    }

    /**
     * Method to generate a given number of perlin noise octaves
     * @param noiseRange The noise range of the biggest (most coarse) octave
     * @param distBetweenPoints Distance between points in the biggest octave
     * @param nbOfOctaves Total number of octaves we want
     * @param width Width of the edge we are calculating perlin noise for
     * @return List of lists of noise (each list is an octave)
     */
    private ArrayList<ArrayList<Float>> generateAllOctaves(int noiseRange, int distBetweenPoints, int nbOfOctaves, int width) {
        ArrayList<ArrayList<Float>> result = new ArrayList<>();
        for (int i = 0; i < nbOfOctaves; i++) {
            result.add(generateOneOctave(noiseRange, distBetweenPoints, width));
            noiseRange /= divisor; // Noise range is smaller for each octave
            distBetweenPoints /= divisor; // Distance between points is smaller for each octave (meaning more points -> fine grained noise)
        }
        return result;
    }

    /**
     * Method to generate one octave of perlin noise with a given noise range and nb of points (width and distance between points)
     * @param noiseRange The range of the noise in the current octave [-noiseRange,noiseRange]
     * @param distBetweenPoints Distance between points in the octave
     * @param width Width of the edge we are calculating perlin noise for
     * @return The noise values for the current octave of perlin noise
     */
    private ArrayList<Float> generateOneOctave(int noiseRange, int distBetweenPoints, int width) {
        ArrayList<Float> octave = new ArrayList<>();
        int currWidth = 0;
        // We need newNoise and nextNoise to be able to directly do the interpolation between points we generate noise for
        float newNoise = 0;
        float nextNoise = 0;
        while (currWidth <= width) {
            if (currWidth % distBetweenPoints == 0) { // Case where we are generating a new noise value
                newNoise = nextNoise;
                if (currWidth + distBetweenPoints >= width) nextNoise = 0; // Want last point to have noise 0 so edges will meet
                else nextNoise = noise(noiseRange);
                octave.add(newNoise);
            }
            else { // Case where we are interpolating between newNoise and nextNoise
                float interpolatedNoise = interpolate(newNoise, nextNoise, (currWidth % distBetweenPoints) / (float) distBetweenPoints);
                octave.add(interpolatedNoise);
            }
            currWidth++;
        }
        return octave;
    }

    /**
     * Helper method to generate a random number in range [-noiseRange, noiseRange]
     * @return The randomly generated number
     */
    private float noise(int noiseRange) {
        return (float) Math.random() * (2 * noiseRange + 1) - noiseRange;
    }

    /**
     * A method to do smooth interpolation between two points.
     * Formula found on http://www.java-gaming.org/topics/solved-smoothly-interpolate-between-two-points/34706/view.html
     * @param fstPoint The noise value of the previous point we know
     * @param sndPoint The noise value of the next point we know
     * @param wantedPoint The relative distance between the two points (value between 0 and 1)
     * @return The interpolated noise
     */
    private float interpolate(float fstPoint, float sndPoint, float wantedPoint) {
        float smoothing = wantedPoint * wantedPoint * (3.0f - 2.0f * wantedPoint);
        return fstPoint + smoothing * (sndPoint - fstPoint);
    }

}
