using System;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string GAME_ID = "5995681"; //replace with your gameID from dashboard. note: will be different for each platform.
    private string REWARDED_VIDEO_PLACEMENT = "rewardedVideo";//replace with your Rewarded_Android for live ads

    public GameOverMenu gameOverMenu;

    private bool testMode = true;

    public void Initialize()
    {
        if(Application.platform ==RuntimePlatform.IPhonePlayer)
        {
            GAME_ID = "5995680"; //iOS gameID

            REWARDED_VIDEO_PLACEMENT = "Rewarded_iOS";
        }
        if (Advertisement.isSupported)
        {
            DebugLog(Application.platform + " supported by Advertisement");
        }
        Advertisement.Initialize(GAME_ID, testMode, this);
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        DebugLog("Init Success");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        DebugLog($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        DebugLog($"Load Success: {placementId}");
        if (placementId.Equals(REWARDED_VIDEO_PLACEMENT))
        {
            Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
        }
        else if(placementId.Equals("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo", this);
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        DebugLog($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        DebugLog($"OnUnityAdsShowFailure: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        DebugLog($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        DebugLog($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        DebugLog($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");

        if(placementId.Equals(REWARDED_VIDEO_PLACEMENT) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            DebugLog("Reward the player");
            //reward the player
            if (gameOverMenu != null)
            {
                gameOverMenu.RevivePlayer();
            }
        }
    }
    #endregion

    public void OnGameIDFieldChanged(string newInput)
    {
        GAME_ID = newInput;
    }

    public void ToggleTestMode(bool isOn)
    {
        testMode = isOn;
    }

    //wrapper around debug.log to allow broadcasting log strings to the UI
    void DebugLog(string msg)
    {
        Debug.Log(msg);
    }
}
