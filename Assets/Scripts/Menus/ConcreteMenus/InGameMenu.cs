using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : BaseMenu
{
    [Header("UI Elements")]
    public Button PauseButton;
    public TMP_Text currentScore;
    public TMP_Text highScore;
    public TMP_Text time;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.InGame;

        // Hook up pause button
        if (PauseButton != null)
            PauseButton.onClick.AddListener(() => SetNextMenu(MenuStates.Pause));

        // Find UI elements if not assigned in Inspector
        if (currentScore == null)
            currentScore = GameObject.FindWithTag("ScoreText")?.GetComponent<TMP_Text>();
        if (time == null)
            time = GameObject.FindWithTag("TimeText")?.GetComponent<TMP_Text>();

        // Initialize UI
        UpdateScore(ScoreManager.instance?.score ?? 0f);
        UpdateTime(TimerManager.instance?.GetTimeRemaining() ?? 0f);

        // Listen for score updates
        if (ScoreManager.instance != null)
            ScoreManager.instance.OnScoreChanged += UpdateScore;

        // Listen for timer updates
        if (TimerManager.instance != null)
            TimerManager.instance.OnTimeChanged += UpdateTime;

        // Show/hide time based on game mode
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameModeChanged += HandleGameModeChanged;
            HandleGameModeChanged(GameManager.Instance.CurrentMode);
        }
    }

    private void HandleGameModeChanged(GameMode mode)
    {
        if (time != null)
            time.gameObject.SetActive(mode == GameMode.Timed);
    }

    public void UpdateTime(float timeValue)
    {
        if (time == null) return;
        int seconds = Mathf.CeilToInt(timeValue);
        time.text = "TIME: " + seconds;

        if(timeValue <= 0f)
        {
            GameManager.Instance.TimerEnding();
        }

    }

    public void UpdateScore(float scoreValue)
    {
        if (currentScore == null) return;
        currentScore.text = "Current Score: " + Mathf.FloorToInt(scoreValue);
    }

    private void OnDestroy()
    {
        if (ScoreManager.instance != null)
            ScoreManager.instance.OnScoreChanged -= UpdateScore;
        if (TimerManager.instance != null)
            TimerManager.instance.OnTimeChanged -= UpdateTime;
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameModeChanged -= HandleGameModeChanged;
    }
}


