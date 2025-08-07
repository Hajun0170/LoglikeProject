using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI; // 일시정지 UI (선택)

    private bool isPaused = false;
    private LevelUpManager levelUpManager;

 void Start()
    {
        levelUpManager = FindObjectOfType<LevelUpManager>();
    }
    void Update()
    {
        // 스킬 카드가 열려있으면 ESC 무시하는 기능 추가.
        if (levelUpManager != null && levelUpManager.IsCardActive)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

   public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        if (pauseUI != null)
            pauseUI.SetActive(isPaused);
    
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
