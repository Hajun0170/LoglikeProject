using UnityEngine;
using TMPro;

public class WaveTimerUI : MonoBehaviour
{
    public TMP_Text txtTimer;

    private SpawnManager spawnManager;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Update()
    {
        if (spawnManager == null || spawnManager.waveList.Count == 0) return;

        WaveData currentWave = GetCurrentWave();
        if (currentWave == null) return;

        // 보스 웨이브면 텍스트 고정
        if (currentWave.lockUntilBossDead && IsBossAlive())
        {
            txtTimer.text = "Kill the Boss!";
            return;
        }

        // ✅ 여기서 새로운 필드로 수정
        float waveStartTime = GetPrivateField<float>(spawnManager, "currentWaveStartTime");
        float waveDuration = GetPrivateField<float>(spawnManager, "currentWaveDuration");
        float timeLeft = Mathf.Max((waveStartTime + waveDuration) - Time.time, 0f);

        txtTimer.text = $"Next Wave: {timeLeft:F1}초";
    }

    WaveData GetCurrentWave()
    {
        int currentWaveIndex = GetPrivateField<int>(spawnManager, "currentWaveIndex") - 1;
        var waveList = spawnManager.waveList;
        if (currentWaveIndex < 0 || currentWaveIndex >= waveList.Count) return null;
        return waveList[currentWaveIndex];
    }

    bool IsBossAlive()
    {
        return GetPrivateField<bool>(spawnManager, "bossAlive");
    }

    // private 변수 접근용
    T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field == null) return default;
        return (T)field.GetValue(obj);
    }
}
