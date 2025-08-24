using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    public Button ResumeButton;
    public Button settingsButton;
    public Button MainMenuButton;
    public TMP_Text MenuTitle;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.Pause;

        if (ResumeButton) ResumeButton.onClick.AddListener(() => SetNextMenu(MenuStates.InGame));
        if (settingsButton) settingsButton.onClick.AddListener(() => SetNextMenu(MenuStates.Settings));
        if (MainMenuButton) MainMenuButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        if (MenuTitle) MenuTitle.text = "Pause Menu"; // Set the title for the pause menu
    }


    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 0f; // Pause the game
    }

    public override void ExitState()
    {
        base.ExitState();
        Time.timeScale = 1f; // Resume the game
    }
}
