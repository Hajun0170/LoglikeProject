using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SpawnManager spawnManager;

    void Awake() => Instance = this;

    void Start()
    {
        spawnManager.Init(); // 웨이브 시스템 시작
    }

    public void OnGameClear()
    {
        Debug.Log("게임 클리어!");
        Time.timeScale = 0;
    }

    public void OnGameOver()
    {
        Debug.Log("게임 오버!");
        Time.timeScale = 0;
    }
}