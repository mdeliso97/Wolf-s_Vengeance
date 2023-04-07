using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    private int posX;
    private int posY;
    private int size;

    private Vector3[] treePositions;
    private GameObject[] trees;

    public Chunk(int posX, int posY, int size, int noiseOffset, float treeDensity) {
        this.posX = posX;
        this.posY = posY;
        this.size = size;

        float[,] map = NoiseGenerator.Generate(size, size, 0.1f, (posX + noiseOffset)*size, (posY + noiseOffset)*size);

        List<Vector3> _treePositions = new List<Vector3>();

        float density = 0.0001f * treeDensity;
        float t_min = 0.5f + density;
        float t_max = 0.5f - density;

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                float t = map[x, y];

                if (t < t_min && t > t_max) {
                    Vector3 position = new Vector3(posX*size + x, posY*size + y, y + 1);
                    _treePositions.Add(position);
                }
            }
        }

        treePositions = _treePositions.ToArray();
    }

    public int NumTrees() {
        return treePositions.Length;
    }

    public void SetTrees(GameObject[] trees) {
        this.trees = trees;
        for (int i = 0; i < treePositions.Length; i++) {
            this.trees[i].transform.position = treePositions[i];
        }
    }

    public GameObject[] GetTrees() {
        return trees;
    }
}
