using UnityEngine.SceneManagement;
using UnityEngine;

public class Returntitle : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("title"); // 메인씬 이동.
    }
}
