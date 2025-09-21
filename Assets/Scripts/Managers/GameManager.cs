using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ScoreManager), typeof(TimerManager), typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    AudioSource audioSource;
    public AudioClip DeathClip;

    public InGameMenu InGameMenu;

    public GameMode CurrentMode;
    public event System.Action<GameMode> OnGameModeChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fetch the UI from the new scene
        InGameMenu = GameObject.FindFirstObjectByType<InGameMenu>();

        // Reset score when gameplay scene loads
        if (scene.name.Contains("Game")) // adjust as needed
        {
            ScoreManager.instance?.ResetScore();
        }

        // Update UI immediately
        if (InGameMenu != null)
        {
            InGameMenu.UpdateScore(ScoreManager.instance?.score ?? 0f);
            InGameMenu.UpdateTime(TimerManager.instance?.GetTimeRemaining() ?? 0f);
        }
    }

    public void PlayerHit()
    {
        audioSource.PlayOneShot(DeathClip);
        EndGame(false);
    }

   

    private void EndGame(bool victory)
    {
        if (InGameMenu == null)
        {
            InGameMenu = GameObject.FindFirstObjectByType<InGameMenu>();
            if (InGameMenu == null) return;
        }

        InGameMenu.SetNextMenu(victory ? MenuStates.Victory : MenuStates.GameOver);
    }

    public void UpdateGameMode(GameMode newMode)
    {
        CurrentMode = newMode;
        OnGameModeChanged?.Invoke(newMode);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public enum GameMode
{
    Timed,
    Endless
}

