using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform player;
    public float spawnRadius = 8f;

    public float waveInterval = 10f; // 웨이브 간격 (초)
    public float spawnCooldown = 1f; // 적 스폰 주기
    private float nextWaveTime = 0f;
    private float nextSpawnTime = 0f;

    private int currentWave = 1;
    private int enemiesToSpawn = 5;

    void Update()
    {
        if (Time.time >= nextWaveTime)
        {
            StartNewWave();
        }

        if (enemiesToSpawn > 0 && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnCooldown;
            enemiesToSpawn--;
        }
    }

    void StartNewWave()
    {
        currentWave++;
        enemiesToSpawn = 5 + currentWave * 2; // 웨이브마다 점점 더 많이 소환
        nextWaveTime = Time.time + waveInterval;
        Debug.Log("📢 웨이브 " + currentWave + " 시작!");
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = player.position + (Vector3)(Random.insideUnitCircle.normalized * spawnRadius);
        PoolManager.Instance.Spawn("Enemy", spawnPos, Quaternion.identity);
    }
}
