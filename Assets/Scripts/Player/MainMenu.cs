using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private string gameModeSceneName = "GameModeSelect"; 

    public void OnPlayClicked()
    {
        SceneManager.LoadScene(gameModeSceneName);
    }

    public void OnSettingsClicked()
    {
    }

    public void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}