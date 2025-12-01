using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;
    public float TimeLimit = 60f;
    private float timer;

    public event Action<float> OnTimeChanged;

    private bool isRunning = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start timer only in gameplay scenes
        if (scene.name.Contains("Game"))
        {
            ResetTimer();
            StartTimer();
        }
        else
        {
            StopTimer();
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = 0f;
            StopTimer();
        }

        OnTimeChanged?.Invoke(timer);
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;

    public void ResetTimer()
    {
        timer = TimeLimit;
        OnTimeChanged?.Invoke(timer);
    }

    public float GetTimeRemaining() => timer;

    public void AddTime(float extraSeconds)
    {
        timer += extraSeconds; // Add the extra seconds
        OnTimeChanged?.Invoke(timer); // Update UI or listeners
        if (!isRunning && timer > 0f) // Restart the timer if it was stopped at zero
            StartTimer();
    }

}
