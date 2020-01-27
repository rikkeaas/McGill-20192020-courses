package StoneHenge;

import java.util.ArrayList;

public class Edge {

    private static int initialDistBetwenPoints = 24;
    private static int nbOfOctaves = 4;
    private static int divisor = 2;

    private ArrayList<Float> noiseValues;
    private ArrayList<Float> pointValues;
    private boolean vertical;

    public Edge(int length, boolean vertical, int startPoint, int startNoise) {
        this.vertical = vertical;

        ArrayList<ArrayList<Float>> noiseOctaves = generateAllOctaves(StoneHenge.noiseRange, initialDistBetwenPoints, nbOfOctaves, length);
        noiseValues = combineOctaves(noiseOctaves);

        pointValues = new ArrayList<>();
        float point = 0;
        float distBetweenPoints = ((float) length) / ((float) noiseValues.size());
        while(pointValues.size() != noiseValues.size()) {
            pointValues.add(point);
            point += distBetweenPoints;
        }

        addStartPosition(noiseValues, startNoise);
        addStartPosition(pointValues, startPoint);

        noiseValues.set(0, (float) startNoise);
        noiseValues.set(noiseValues.size() - 1, (float) startNoise);
    }

    private void addStartPosition(ArrayList<Float> values, int startPosition) {
        for (int i = 0; i < values.size(); i++) {
            values.set(i, values.get(i) + startPosition);
        }
    }

    public ArrayList<Float> getXValues() {
        if (vertical) return noiseValues;
        else return pointValues;
    }

    public ArrayList<Float> getYValues() {
        if (vertical) return pointValues;
        else return noiseValues;
    }

    private ArrayList<Float> combineOctaves(ArrayList<ArrayList<Float>> noiseOctaves) {
        ArrayList<Float> combinedOctaves = new ArrayList<>();
        for (int i = 0; i < noiseOctaves.get(0).size(); i++) {
            float totalNoise = 0;
            for (ArrayList<Float> noiseOctave : noiseOctaves) {
                totalNoise += noiseOctave.get(i);
            }
            combinedOctaves.add(totalNoise);
        }
        return combinedOctaves;
    }


    private ArrayList<ArrayList<Float>> generateAllOctaves(int noiseRange, int distBetweenPoints, int nbOfOctaves, int width) {
        ArrayList<ArrayList<Float>> result = new ArrayList<>();
        for (int i = 0; i < nbOfOctaves; i++) {
            result.add(generateOneOctave(noiseRange, distBetweenPoints, width));
            noiseRange /= divisor;
            distBetweenPoints /= divisor;
        }
        return result;
    }

    private ArrayList<Float> generateOneOctave(int noiseRange, int distBetweenPoints, int width) {
        ArrayList<Float> octave = new ArrayList<>();
        int currWidth = 0;
        float newNoise = 0;
        float nextNoise = 0;
        while (currWidth <= width) {
            if (currWidth % distBetweenPoints == 0) {
                newNoise = nextNoise;
                if (currWidth + distBetweenPoints >= width) nextNoise = 0; // Want last point to have noise 0 so edges will meet
                else nextNoise = noise(noiseRange);
                octave.add(newNoise);
            }
            else {
                float interpolatedNoise = interpolate(newNoise, nextNoise, (currWidth % distBetweenPoints) / (float) distBetweenPoints);
                octave.add(interpolatedNoise);
            }
            currWidth++;
        }
        return octave;
    }

    // Adding random number in range [-noiseRange, noiseRange]
    private float noise(int noiseRange) {
        return (float) Math.random() * (2 * noiseRange + 1) - noiseRange;
    }

        // Nb nb fiks dette!!!! Fant på nettside https://codepen.io/Tobsta/post/procedural-generation-part-1-1d-perlin-noise
    private float interpolate(float fstPoint, float sndPoint, float wantedPoint) {
//        return fstPoint * (1 - wantedPoint) + sndPoint * wantedPoint;
        float temp = wantedPoint * (float) Math.PI;
        float temp2 = (float) (1 - Math.cos(temp)) * 0.5f;
        return fstPoint * (1 - temp2) + sndPoint * temp2;
    }


}






























//    private int length;
//    private int noiseOctaves;
//    private float distanceBetweenPoints;
//    private float[] pointValues;
//    private float[] noiseValues;
//
//    // Want length / distanceBetweenPoints = 1 + 2^n
//    public Edge(int length, int noiseOctaves, int distanceBetweenPoints, int noiseRange) {
//        this.length = length;
//        this.noiseOctaves = noiseOctaves;
//        this.distanceBetweenPoints = distanceBetweenPoints;
//
//        // Adding the corresponding x or y to the noise value we will generate
//        pointValues = new float[length / distanceBetweenPoints + 1];
//        for (int i = 0; i < length / distanceBetweenPoints + 1; i++) {
//            pointValues[i] = i * distanceBetweenPoints;
//        }
//        noiseValues = generatePerlinNoise(noiseOctaves, noiseRange, pointValues.length);
//    }
//

//
//    private float[] generatePerlinNoise(int noiseOctaves, int noiseRange, int nbOfPoints) {
//        // Generating the noise of each octave
//        ArrayList<ArrayList<Float>> allOctaves = new ArrayList<>(generatePerlinNoiseForAllOctaves(noiseOctaves, noiseRange, nbOfPoints));
//
//        // Adding the octaves together to form the completed perlin noise
//        float[] perlinNoise = new float[nbOfPoints];
//
//        for (int i = 0; i < nbOfPoints; i++) {
//            perlinNoise[i] = 0;
//        }
//        for (ArrayList<Float> octave : allOctaves) {
//            for (int i = 0; i < nbOfPoints; i++) {
//                if (i % ((nbOfPoints - 1) / (octave.size() - 1)) == 0) {
//                    perlinNoise[i] += octave.get(i / ((nbOfPoints - 1) / (octave.size() - 1)));
//                }
//                else {
//                    float prevPoint = octave.get(i / ((nbOfPoints - 1) / (octave.size() - 1)));
//                    float nextPoint = octave.get(1 + i / ((nbOfPoints - 1) / (octave.size() - 1)));
//                    System.out.println((i % octave.size()) / (float) octave.size());
//
//                    perlinNoise[i] += interpolate(prevPoint, nextPoint, (i % octave.size()) / (float) octave.size());
//                }
//
//            }
//        }
//        return perlinNoise;
//
//
////
////        for (int i = 0; i < noiseOctaves; i++) {
////            int nbOfPointForCurrOctave = nbOfPoints / (noiseOctaves-i);
////            ArrayList<Float> perlinNoiseForCurrOctave = generatePerlinNoiseForOneOctave(noiseRange, nbOfPointForCurrOctave);
////            noiseRange /= 2;
////            for (int j = 0; j < nbOfPoints; j++) {
////                if (j % (nbOfPointForCurrOctave + 1) == 0) { // Calculating perlin noise for this point in this octave
////                    perlinNoise.set(j, perlinNoise.get(j) + perlinNoiseForCurrOctave.get(j / nbOfPointForCurrOctave));
////                }
////                else {
////                    System.out.println(j);
////                    System.out.println(nbOfPointForCurrOctave);
////                    float prevPoint = perlinNoiseForCurrOctave.get(j / (nbOfPointForCurrOctave + 1));
////                    float nextPoint = perlinNoiseForCurrOctave.get(1 + (j / (nbOfPointForCurrOctave + 1)));
////                    float interpolation = interpolate(prevPoint, nextPoint, (j % nbOfPointForCurrOctave) / (float) nbOfPointForCurrOctave);
////                    perlinNoise.set(j, perlinNoise.get(j) + interpolation);
////                }
////            }
////        }
//    }
//
//    private ArrayList<ArrayList<Float>> generatePerlinNoiseForAllOctaves(int noiseOctaves, int noiseRange, int nbOfPoints) {
//        ArrayList<ArrayList<Float>> allOctaves = new ArrayList<>();
//        for (int i = noiseOctaves - 1; i >= 0; i--) {
//            int currNbOfPoints = 1 + ((nbOfPoints - 1) / (int) Math.pow(2, i));
//            System.out.println(currNbOfPoints);
//            System.out.println(noiseRange);
//            allOctaves.add(generatePerlinNoiseForOneOctave(noiseRange, currNbOfPoints));
//            noiseRange /= 2;
//        }
//        return allOctaves;
//    }
//
//
//    /**
//     * Method to randomly generate noise values from a given range for a number of points
//     * @param noiseRange The range from which we will randomly choose noise values (range is [-noiseRange, noiseRange]
//     * @param nbOfPoints The number points we want to generate random noise for
//     * @return A list of the random noise values
//     */
//    private ArrayList<Float> generatePerlinNoiseForOneOctave(int noiseRange, int nbOfPoints) {
//        ArrayList<Float> perlinNoise = new ArrayList<>();
//        for (int i = 0; i < nbOfPoints; i++) {
//            float rand = (float) Math.random() * (2 * noiseRange + 1) - noiseRange; // Adding random number in range [-noiseRange, noiseRange]
//            perlinNoise.add(rand);
//        }
//        return perlinNoise;
//    }
//
//



