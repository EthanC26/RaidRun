using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : BaseMenu
{
    public Button PauseButton;
    public TMP_Text currentScore;
    public TMP_Text highScore;
    public TMP_Text time;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.InGame;
        if (PauseButton) PauseButton.onClick.AddListener(() => SetNextMenu(MenuStates.Pause));
        if (currentScore) currentScore.text = "Current Score: 0"; // Placeholder, update with actual score logic
        if (highScore) highScore.text = "High Score: 0"; // Placeholder, update with actual high score logic
        
       GameManager.Instance.OnGameModeChanged += HandleGameModeChanged;

        HandleGameModeChanged(GameManager.Instance.CurrentMode); // Initialize visibility based on current mode
    }

    private void HandleGameModeChanged(GameMode mode)
    {
       time.gameObject.SetActive(mode == GameMode.Timed);
    }

    public void UpdateTime(float timeValue)
    {
        int seconds = Mathf.CeilToInt(timeValue);
        if(time != null)
        {
            time.text = seconds.ToString();
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameModeChanged -= HandleGameModeChanged; // Unsubscribe to avoid memory leaks
    }

    
}
