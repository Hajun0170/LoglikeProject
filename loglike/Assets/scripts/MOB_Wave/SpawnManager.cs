using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public List<WaveData> waveList;
    private int currentWaveIndex = 0;

    private float nextSpawnTime;
    private bool waveActive = false;

    public Transform player;
    public float spawnRadius = 8f;
    public float spawnCooldown = 1f;

    private bool bossAlive = false;

    // 웨이브 타이머 관련
    private float currentWaveStartTime;
    private float currentWaveDuration;

    // 웨이브별 스폰 수량
    private int remainMelee = 0;
    private int remainRanged = 0;
    private int remainTank = 0;

    public Slider FinalBossSlider;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        currentWaveIndex = 0;
        StartNextWave();
    }

    void Update()
    {
        if (!waveActive) return;
        if (currentWaveIndex >= waveList.Count) return;

        WaveData currentWave = waveList[currentWaveIndex];

        // 일반 웨이브: 시간이 지나면 다음 웨이브로
        if (!currentWave.lockUntilBossDead)
        {
            if (Time.time - currentWaveStartTime >= currentWaveDuration)
            {
                waveActive = false;
                StartNextWave();
                return;
            }
        }
        // 보스 웨이브: 보스가 죽으면 다음 웨이브로
        else if (!bossAlive)
        {
            waveActive = false;
            StartNextWave();
            return;
        }

        // 적 스폰
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
        Debug.Log("모든 웨이브 종료!");
        GameManager.Instance.OnGameClear();
        waveActive = false;
        return;
    }

    WaveData wave = waveList[currentWaveIndex]; // 인덱스 증가 전, 현재 웨이브 정보로 셋업
    currentWaveStartTime = Time.time;
    currentWaveDuration = wave.waveDuration;
    nextSpawnTime = Time.time;
    waveActive = true;

    remainMelee = wave.meleeCount;
    remainRanged = wave.rangedCount;
    remainTank = wave.tankCount;

    Debug.Log($"웨이브 {currentWaveIndex + 1} 시작");

    if (wave.spawnMidBoss)
    {
        bossAlive = true;
        PoolManager.Instance.Spawn("MidBoss", player.position + Vector3.right * 10f, Quaternion.identity);
    }
    
   if (wave.spawnFinalBoss)
{
    bossAlive = true;

    GameObject bossObj = PoolManager.Instance.Spawn("FinalBoss", player.position + Vector3.right * 12f, Quaternion.identity);
    if (bossObj != null)
    {
        FinalBoss fb = bossObj.GetComponent<FinalBoss>();
        if (fb != null && FinalBossSlider != null)
        {
            FinalBossSlider.gameObject.SetActive(true);  //UI 표시
            fb.Init(FinalBossSlider);  //슬라이더 연결
        }
    }
}

    // 여기에서 인덱스를 증가시켜야 다음 웨이브로 정확히 넘어감
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
        Debug.Log("보스 처치됨!");
        // 보스 웨이브는 Update에서 자동으로 다음 웨이브 진입 처리
    }

    public float GetRemainingTime()
    {
        // 타이머는 일반 웨이브일 때만 표시, 보스 웨이브는 0 반환
        WaveData wave = GetCurrentWaveData();
        if (wave != null && wave.lockUntilBossDead)
            return 0f;

        return Mathf.Max(0f, currentWaveDuration - (Time.time - currentWaveStartTime));
    }

    public WaveData GetCurrentWaveData()
    {
        if (currentWaveIndex < waveList.Count)
            return waveList[currentWaveIndex];
        return null;
    }
}