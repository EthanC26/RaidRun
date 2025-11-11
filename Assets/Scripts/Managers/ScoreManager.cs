using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public float score = 0f;

    // Event to notify UI
    public event Action<float> OnScoreChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Reset score whenever a new gameplay scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Game")) // Change to your gameplay scene name
        {
            ResetScore();
        }
    }

    public void AddDistance(float distance)//setting the score based on distance
    {
        score += distance;
        OnScoreChanged?.Invoke(score);
    }

    public void ResetScore()
    {
        score = 0f;
        OnScoreChanged?.Invoke(score);
    }

    public int GetScore() => Mathf.FloorToInt(score);
}

