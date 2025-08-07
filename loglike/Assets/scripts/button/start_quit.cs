using UnityEngine;
using UnityEngine.SceneManagement;

public class start_quit : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("MainScene"); // 메인씬 이동.
    }

    public void OnQuitButton()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 종료 구문
    #else
        Application.Quit(); // 빌드버전 종료 구문
    #endif
    }
}