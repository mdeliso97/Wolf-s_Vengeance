using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public class World : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> trees = new List<GameObject>();
    public List<GameObject> hunters = new List<GameObject>();
    public SpriteRenderer[] points = new SpriteRenderer[4];
    public SpriteRenderer bossBase;

    public GameObject bossJaegerPrefab;
    public bool isBossInitiated = false;

    public Image bossDirectionUI;
    public Image[] bossHearts;

    [Header("Camera")]
    public Camera camera;

    [Header("Objects")]
    public float spawnTreeDensity = 2f;
    public float bossTreeDensity = 1f;
    public int bossDistance = 1_000;

    [Header("Enemy Spawn Settings")]
    public float spawnInterval = 20f;
    

    private Vector3 bossPosition;

    private List<GameObject> usedTrees = new List<GameObject>();
    private List<GameObject> freeTrees = new List<GameObject>();
    private Dictionary<string, Chunk> chunks = new Dictionary<string, Chunk>();
    private int chunkSize = 50;

    private float time = 0;
    private int numUpdates = 0;

    private bool fadeAudio = false;
    private float fadeTime = 0f;
    private float fadeDuration = 1f;
    private AudioSource battleAudio;
    private AudioSource bossFightAudio;

    void Start() {
        SetBossPosition();
        for (int i = 0; i < bossHearts.Length; i++)
        {
            bossHearts[i].enabled = false;
        }
        battleAudio = GameObject.Find("Battle Soundtrack").GetComponent<AudioSource>();
        bossFightAudio = GameObject.Find("Bossfight Soundtrack").GetComponent<AudioSource>();

        battleAudio.Play();

        time = spawnInterval - 4;
    }

    void Update() {
        // float timeStart = Time.realtimeSinceStartup;
        LoadChunks();
        // time += Time.realtimeSinceStartup - timeStart;

        // numUpdates++;
        // if (numUpdates % 60 == 0) {
        //     Debug.Log("Avg LoadChunks execution time: " + (time/60 * 1000) + " ms");
        //     Debug.Log("total Trees: " + (freeTrees.Count + usedTrees.Count));
        //     numUpdates = 0;
        //     time = 0;

        //     Debug.Log("rect " + bossDirectionUI.rectTransform.rect);
        // }

        UpdateBossPositionUI();

        if (Vector2.Distance(new Vector2(camera.transform.position.x, camera.transform.position.y), new Vector2(bossPosition.x, bossPosition.y)) < 20 && !isBossInitiated)
        {
            StartBossSequence();
            isBossInitiated = true;

            fadeAudio = true;
            bossFightAudio.Play();
        }

        if (fadeAudio)
        {
            FadeAudio();
        }


        time += Time.deltaTime;
        if (time > spawnInterval && !isBossInitiated) {
            Vector3[] spawnPositions = GetSpawnPositions(4);
            foreach (Vector3 spawnPosition in spawnPositions) {
                
                int randomNumber = Random.Range(0, 5);
                if (randomNumber == 0)
                {
                    Instantiate(hunters[0], spawnPosition, Quaternion.identity);
                } else
                {
                    Instantiate(hunters[1], spawnPosition, Quaternion.identity);
                }
            }
            time -= spawnInterval;
            spawnInterval *= 0.9f;
            if (spawnInterval < 5f)
            {
                spawnInterval = 5f;
            }
        }
    }

    private void FadeAudio()
    {
        fadeTime += Time.deltaTime;

        if (fadeTime < fadeDuration)
        {
            battleAudio.volume = Mathf.Lerp(1.0f, 0.0f, fadeTime / fadeDuration);
            bossFightAudio.volume = Mathf.Lerp(0.0f, 1.0f, fadeTime / fadeDuration);
        }
        else
        {
            fadeAudio = false;
            battleAudio.Stop();
        }
    }

    private void StartBossSequence()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("jaeger");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
        for (int i = 0; i < bossHearts.Length; i++)
        {
            bossHearts[i].enabled = true;
        }
        bossJaegerPrefab.SetActive(true);
        bossJaegerPrefab.transform.position = bossPosition;
    }

    private void LoadChunks() {
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));

        int noiseOffset = 100;

        float left = bottomLeft.x;
        float right = topRight.x;
        float top = topRight.y;
        float bottom = bottomLeft.y;
        
        int bossDistanceSquared = bossDistance*bossDistance;
        int chunkSizeSquared = chunkSize*chunkSize;

        int leftChunk = Mathf.FloorToInt(left/chunkSize) - 1;
        int rightChunk = Mathf.CeilToInt(right/chunkSize);
        int topChunk = Mathf.CeilToInt(top/chunkSize);
        int bottomChunk = Mathf.FloorToInt(bottom/chunkSize) - 1;

        string[] usedChunks = new string[(rightChunk-leftChunk+1) * (topChunk-bottomChunk+1)];

        for (int x = leftChunk; x <= rightChunk; x++) {
            for (int y = bottomChunk; y <= topChunk; y++) {
                string key = "" + x + "-" + y;

                if (!chunks.ContainsKey(key)) {
                    int distanceSquared = chunkSizeSquared * (x*x + y*y);
                    float treeDensity = spawnTreeDensity + (bossTreeDensity - spawnTreeDensity) * Mathf.Min(1.0f, (float)distanceSquared / (float)bossDistanceSquared);
                    // Debug.Log("Tree Density: " + treeDensity);

                    Chunk chunk = new Chunk(x, y, chunkSize, noiseOffset, treeDensity);
                    int numTrees = chunk.NumTrees();

                    if (freeTrees.Count < numTrees) {
                        int missingTrees = numTrees - freeTrees.Count;
                        for (int i = 0; i < missingTrees; i++) {
                            int treeIndex = Random.Range(0, trees.Count);// int treeIndex = 0;
                            freeTrees.Add(Instantiate(trees[treeIndex], new Vector3(0, 0, 0), Quaternion.identity));
                        }
                        // Debug.Log("Instantiated " + missingTrees + " trees");
                    }

                    GameObject[] _trees = new GameObject[numTrees];
                    for (int i = 0; i < numTrees; i++) {
                        _trees[i] = freeTrees[0];
                        usedTrees.Add(freeTrees[0]);
                        freeTrees.RemoveAt(0);
                    }
                    chunk.SetTrees(_trees);

                    chunks.Add(key, chunk);
                }

                usedChunks[(rightChunk - leftChunk + 1) * (y - bottomChunk) + x - leftChunk] = key;
            }
        }

        List<string> chunksToRemove = new List<string>();
        foreach (KeyValuePair<string, Chunk> value in chunks) {
            if (!usedChunks.Contains(value.Key)) {
                chunksToRemove.Add(value.Key);
            }
        }

        foreach (string key in chunksToRemove) {
            GameObject[] _trees = chunks[key].GetTrees();
            for (int i = 0; i < _trees.Length; i++) {
                usedTrees.Remove(_trees[i]);
                freeTrees.Add(_trees[i]);
            }
            chunks.Remove(key);
        }
    }

    private void SetBossPosition() {
        float positionY = Random.Range(-bossDistance, bossDistance);
        float positionX = Mathf.Sqrt(bossDistance * bossDistance - positionY * positionY);
        positionX = Random.Range(-1, 1) > 0 ? positionX : -positionX;
        bossPosition = new Vector3(positionX, positionY, 0);
        bossBase.transform.position = bossPosition + new Vector3(0, 10, 0);
        print(bossPosition);
        print(bossBase.transform.position);

        Debug.Log("Boss Position: x " + positionX + " - y " + positionY);
    }

    private void UpdateBossPositionUI() {
        Vector3 center = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        Vector2 bossDirection = new Vector2(bossPosition.x - center.x, bossPosition.y - center.y).normalized;
        Vector2 up = new Vector2(0f, 1f);
        
        float alpha = Vector2.Angle(bossDirection, up);
        if (bossDirection.x > 0f) {
            alpha = -alpha;
        }
        alpha += 180;

        bossDirectionUI.rectTransform.rotation = Quaternion.Euler(
            bossDirectionUI.rectTransform.eulerAngles.x,
            bossDirectionUI.rectTransform.eulerAngles.y,
            alpha
        );
    }

    private Vector3[] GetSpawnPositions(int amount = 10) {
        Vector3[] positions = new Vector3[amount];

        Vector3 topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, 0));

        float width = topRight.x - topLeft.x;
        float height = topLeft.y - bottomLeft.y;
        float aspectRatio = height / width;
        float margin = width * 0.2f;

        int amountHorizontal = (int)(amount / 2f / (1 + aspectRatio));
        int amountVertical = (int)(amount / 2f - amountHorizontal);

        // Debug.Log("num in x " + amountHorizontal);
        // Debug.Log("num in y " + amountVertical);
        // Debug.Log("total " + (amountHorizontal + amountVertical) * 2);

        for (int i = 0; i < amountVertical; i++) {
            float y = bottomLeft.y + (i + 1) * height / (amountVertical + 1);
            Vector3 left = new Vector3(topLeft.x - margin, y);
            positions[2*i] = left;
            Vector3 right = new Vector3(topRight.x + margin, y);
            positions[2*i + 1] = right;
        }

        for (int i = 0; i < amountHorizontal; i++) {
            float x = topLeft.x + (i + 1) * width / (amountHorizontal + 1);
            Vector3 top = new Vector3(x, topLeft.y + margin);
            positions[2*amountVertical + 2*i] = top;
            Vector3 bottom = new Vector3(x, bottomLeft.y - margin);
            positions[2*amountVertical + 2*i + 1] = bottom;
        }

        return positions;
    }

    private void SetViewportEdgePoints() {
        Vector3 p1 = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 p2 = camera.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector3 p3 = camera.ViewportToWorldPoint(new Vector3(1, 0, 0));
        Vector3 p4 = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        p1.z = 0f;
        p2.z = 0f;
        p3.z = 0f;
        p4.z = 0f;
        points[0].transform.position = p1;
        points[1].transform.position = p2;
        points[2].transform.position = p3;
        points[3].transform.position = p4;
    }
}
