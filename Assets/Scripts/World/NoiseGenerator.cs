using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public static float[,] Generate(int width, int height, float scale, int offsetX = 0, int offsetY = 0) {
        float[,] noise = new float[height, width];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float sampleX = (float)(x + offsetX) * scale;
                float sampleY = (float)(y + offsetY) * scale;

                noise[x, y] = Mathf.PerlinNoise(sampleX, sampleY);
            }
        }

        return noise;
    }
}
