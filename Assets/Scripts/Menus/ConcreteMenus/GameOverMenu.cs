using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameOverMenu : BaseMenu
{
    public UnityAdsManager unityAdsManager;
    public Button QuitBtn;
    public Button MainMenuBtn;
    public Button ReviveBtn;
    public TMP_Text TitleText;
    public TMP_Text ScoreText;

    private void Awake()
    {
        //if you didn't assign in the inspector
        if (unityAdsManager == null)
        {
#if UNITY_6000_0_OR_NEWER
            unityAdsManager = FindFirstObjectByType<UnityAdsManager>();
#else
                unityAdsManager = FindObjectOfType<UnityAdsManager>();
#endif
        }

        unityAdsManager.Initialize();
    }

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.GameOver;

        if (QuitBtn) QuitBtn.onClick.AddListener(QuitGame);

        if(MainMenuBtn) MainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        if (ReviveBtn) ReviveBtn.onClick.AddListener(() => unityAdsManager.LoadRewardedAd());

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

    public void RevivePlayer()
    {
        SetNextMenu(MenuStates.InGame);
    }

    private void OnDestroy() => Time.timeScale = 1.0f;
}
