using UnityEngine;

[System.Serializable]
public class WaveData
{
    public float waveDuration = 30f;
    public int meleeCount = 0;
    public int rangedCount = 0;
    public int tankCount = 0;
    public bool spawnMidBoss = false;
    public bool spawnFinalBoss = false;
    public bool lockUntilBossDead = false;
}
