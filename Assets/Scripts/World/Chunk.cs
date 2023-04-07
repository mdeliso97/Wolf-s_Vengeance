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

        float density = 0.00001f * treeDensity;

        float[,] t_min_max = new float[9, 2];
        for (int i = 1; i <= 9; i++) {
            t_min_max[i-1, 0] = 0.1f * i - density;
            t_min_max[i-1, 1] = 0.1f * i + density;
        }

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                float t = map[x, y];
                for (int i = 0; i < 9; i++) {
                    if (t_min_max[i, 0] < t && t < t_min_max[i, 1]) {
                        Vector3 position = new Vector3(posX*size + x, posY*size + y, y/size + 1);
                        _treePositions.Add(position);
                    }
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
