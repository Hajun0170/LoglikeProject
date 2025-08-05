using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI; // 일시정지 UI (선택)

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseUI != null)
                pauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseUI != null)
                pauseUI.SetActive(false);
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
