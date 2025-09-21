using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverMenu : BaseMenu
{
    public Button QuitBtn;
    public Button MainMenuBtn;
    public TMP_Text TitleText;
    public TMP_Text ScoreText;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.GameOver;

        if (QuitBtn) QuitBtn.onClick.AddListener(QuitGame);

        if(MainMenuBtn) MainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        if (TitleText) TitleText.text = "GAME OVER";

        if (ScoreManager.instance != null)
            ScoreManager.instance.OnScoreChanged += UpdateScore;
    }

    public void UpdateScore(float scoreValue)//show score (time)
    {
        if (ScoreText == null) return;
        ScoreText.text = "Score: " + Mathf.FloorToInt(scoreValue);
    }
    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 0f; // Pause the game when entering menu
    }

    public override void ExitState()
    {
        base.ExitState();
        Time.timeScale = 1f; // Resume the game
    }

    private void OnDestroy() => Time.timeScale = 1.0f;
}
