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

    // 💡 웨이브별 스폰 수량 저장용
    private int remainMelee = 0;
    private int remainRanged = 0;
    private int remainTank = 0;

void Start()
{
    Init();  // ✅ 반드시 있어야 함
}
    public void Init()
    {
        currentWaveIndex = 0;
        StartNextWave();
    }


    void Update()
    {
        if (!waveActive) return;

        if (currentWaveIndex >= waveList.Count)
        {
            waveActive = false;
            return;
        }

        if (waveList[currentWaveIndex].lockUntilBossDead && bossAlive)
        {
            return;
        }

        if (!waveList[currentWaveIndex].lockUntilBossDead &&
            Time.time - waveStartTime > waveList[currentWaveIndex].waveDuration)
        {
            StartNextWave();
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemies();
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

        // 💡 이 시점에 해당 웨이브의 적 수량 저장
        remainMelee = wave.meleeCount;
        remainRanged = wave.rangedCount;
        remainTank = wave.tankCount;

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

        // ✅ 웨이브 인덱스 증가는 여기서!
        currentWaveIndex++;
    }

    void SpawnEnemies()
    {
        if (remainMelee > 0)
        {
            PoolManager.Instance.Spawn("MeleeEnemy", GetSpawnPosition(), Quaternion.identity);
            remainMelee--;
        }
        if (remainRanged > 0)
        {
            PoolManager.Instance.Spawn("RangedEnemy", GetSpawnPosition(), Quaternion.identity);
            remainRanged--;
        }
        if (remainTank > 0)
        {
            PoolManager.Instance.Spawn("TankEnemy", GetSpawnPosition(), Quaternion.identity);
            remainTank--;
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
