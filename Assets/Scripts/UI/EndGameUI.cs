using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour {
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;

    private void OnEnable()
    {
        EventManager.OnGameOver += ShowGameOver;
        EventManager.OnAllWavesCompleted += ShowVictory;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= ShowGameOver;
        EventManager.OnAllWavesCompleted -= ShowVictory;
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    private void ShowGameOver()
    {
        titleText.text = "GAMEOVER";
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ShowVictory()
    {
        titleText.text = "VICTORY!";
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}