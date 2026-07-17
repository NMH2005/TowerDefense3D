using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelect : MonoBehaviour {
    [SerializeField] private string mainSceneName = "MainScene";

    public void OnSandboxClicked()
    {
        GameSession.SelectedMode = GameMode.Sandbox;
        SceneManager.LoadScene(mainSceneName);
    }

    public void OnEndlessClicked()
    {
        GameSession.SelectedMode = GameMode.Endless;
        SceneManager.LoadScene(mainSceneName);
    }
}