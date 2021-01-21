using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Spawnable
    {
        public string type;
        public float weight;
    }

    [System.Serializable]
    public struct SpawnSettings
    {
        public string type;
        public float minWait;
        public float maxWait;
        public int maxObjects;
    }

    private float totalWeight;

    private bool spawningObject = false;

    [SerializeField] private float groundSpawnDistance = 50f;

    public List<Spawnable> enemySpawnables = new List<Spawnable>();
    public List<SpawnSettings> spawnSettings = new List<SpawnSettings>();

    public static ObjectSpawner instance;

    private void Awake()
    {
        instance = this;

        totalWeight = 0f;
        foreach (var spawnable in enemySpawnables)
            totalWeight += spawnable.weight;
    }

    public void SpawnGround()
    {
        ObjectPooler.instance.SpawnFromPool("ground", new Vector3(0, 0, groundSpawnDistance), Quaternion.identity);
    }

    private IEnumerator SpawnObject(string type, float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPooler.instance.SpawnFromPool(type, new Vector3(Random.Range(-4.1f, 4.1f), 0.55f, -5f), Quaternion.identity);
        spawningObject = false;
        GameController.EnemyCount++;
    }

    void Update()
    {
        if (!spawningObject && GameController.EnemyCount < spawnSettings[0].maxObjects && !GameController.GamePaused)
        {
            spawningObject = true;
            float pick = Random.value * totalWeight;
            int chosenIndex = 0;
            float cumulativeWeight = enemySpawnables[0].weight;

            while (pick > cumulativeWeight && chosenIndex < enemySpawnables.Count - 1)
            {
                chosenIndex++;
                cumulativeWeight += enemySpawnables[chosenIndex].weight;
            }

            StartCoroutine(SpawnObject(enemySpawnables[chosenIndex].type,
                Random.Range(spawnSettings[0].minWait / GameController.DifficultyMultiplier,
                spawnSettings[0].maxWait / GameController.DifficultyMultiplier)));

        }
    }
}
