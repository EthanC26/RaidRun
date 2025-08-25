using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : BaseMenu
{
    public AudioMixer mixer;

    public Button backBtn;

    public Slider MasterVolSlider;
    public TMP_Text MasterVolSliderText;

    public Slider MusicVolSlider;
    public TMP_Text MusicVolSliderText;

    public Slider sfxVolSlider;
    public TMP_Text sfxVolSliderText;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.Settings;

        if (backBtn) backBtn.onClick.AddListener(JumpBack);

        setupSliderInformation(MasterVolSlider, MasterVolSliderText, "Master");
        setupSliderInformation(MusicVolSlider, MusicVolSliderText, "music");
        setupSliderInformation(sfxVolSlider, sfxVolSliderText, "SFX");
    }

    void setupSliderInformation(Slider slider, TMP_Text sliderText, string parameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, sliderText, parameterName));
        OnSliderValueChanged(slider.value, slider, sliderText, parameterName);
    }

    void OnSliderValueChanged(float value, Slider slider, TMP_Text sliderText, string parameterName)
    {
        value = (value == 0.0f) ? -80.0f : 20.0f * Mathf.Log10(slider.value);
        sliderText.text = (value == -80.0f) ? "0%" : $"{(int)(slider.value * 100)}%";
        mixer.SetFloat(parameterName, value);
    }
}