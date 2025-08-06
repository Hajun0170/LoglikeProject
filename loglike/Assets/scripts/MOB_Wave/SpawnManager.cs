using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<WaveData> waveList;
    private int currentWaveIndex = 0;

    private float waveStartTime;
    private float nextSpawnTime;
    private bool waveActive = false;

    public Transform player;
    public float spawnRadius = 8f;
    public float spawnCooldown = 1f;

    private bool bossAlive = false;

    public void Init()
    {
        currentWaveIndex = 0;
        StartNextWave();
    }

    void Update()
    {
        if (!waveActive) return;

        // ✅ index 범위 초과 방지
        if (currentWaveIndex >= waveList.Count)
        {
            waveActive = false;
            return;
        }

        WaveData wave = waveList[currentWaveIndex];

        if (wave.lockUntilBossDead && bossAlive)
        {
            return;
        }

        if (!wave.lockUntilBossDead && Time.time - waveStartTime > wave.waveDuration)
        {
            currentWaveIndex++; // ✅ 인덱스 증가 시점을 여기로 이동해도 무방함
            StartNextWave();
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemies(wave);
            nextSpawnTime = Time.time + spawnCooldown;
        }
    }

    void StartNextWave()
    {
        if (currentWaveIndex >= waveList.Count)
        {
            Debug.Log("🌊 모든 웨이브 종료!");
            GameManager.Instance.OnGameClear();
            waveActive = false;
            return;
        }

        WaveData wave = waveList[currentWaveIndex];
        waveStartTime = Time.time;
        nextSpawnTime = Time.time;
        waveActive = true;

        Debug.Log($"📢 웨이브 {currentWaveIndex + 1} 시작!");

        if (wave.spawnMidBoss)
        {
            bossAlive = true;
            PoolManager.Instance.Spawn("MidBoss", player.position + Vector3.right * 10f, Quaternion.identity);
        }
        else if (wave.spawnFinalBoss)
        {
            bossAlive = true;
            PoolManager.Instance.Spawn("FinalBoss", player.position + Vector3.right * 12f, Quaternion.identity);
        }

        // ✅ 인덱스 증가를 StartNextWave 끝에서 수행
        currentWaveIndex++;
    }

    void SpawnEnemies(WaveData wave)
    {
        if (wave.meleeCount > 0)
        {
            PoolManager.Instance.Spawn("MeleeEnemy", GetSpawnPosition(), Quaternion.identity);
            wave.meleeCount--;
        }
        if (wave.rangedCount > 0)
        {
            PoolManager.Instance.Spawn("RangedEnemy", GetSpawnPosition(), Quaternion.identity);
            wave.rangedCount--;
        }
        if (wave.tankCount > 0)
        {
            PoolManager.Instance.Spawn("TankEnemy", GetSpawnPosition(), Quaternion.identity);
            wave.tankCount--;
        }
    }

    Vector2 GetSpawnPosition()
    {
        return player.position + (Vector3)(Random.insideUnitCircle.normalized * spawnRadius);
    }

    public void OnBossDefeated()
    {
        bossAlive = false;
        Debug.Log("💀 보스 처치됨!");
        StartNextWave();
    }
}
