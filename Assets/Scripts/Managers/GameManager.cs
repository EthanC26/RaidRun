using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    public InGameMenu InGameMenu;

    public static float TimeLimit = 60f;
    public static float timer;

    public GameMode CurrentMode;
    public event System.Action<GameMode> OnGameModeChanged;

    private MenuController currentMenuController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return;
        }
    }
    private void Start()
    {
        if(CurrentMode == GameMode.Timed)
        {
            timer = TimeLimit;
        }
        else
        {
            timer = 0f; // Reset timer for Endless mode
        }
    }

    private void Update()
    {
        if (CurrentMode == GameMode.Timed && timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0f) timer = 0f; // Ensure timer doesn't go negative
            if (InGameMenu != null)
            {
                InGameMenu.UpdateTime(timer);
            }
            if(timer == 0f)
            {
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over! Timer has reached zero.");
        SceneManager.LoadScene("MainMenu"); // Replace with your actual game over scene
    }

    public void SetMenuController(MenuController newMenuController) => currentMenuController = newMenuController;

    public void UpdateGameMode(GameMode newMode)
    {
        CurrentMode = newMode;
        Debug.Log($"Game mode updated to: {newMode}");

        OnGameModeChanged?.Invoke(newMode);


    }
}


public enum GameMode
{
   Timed,
   Endless
}
