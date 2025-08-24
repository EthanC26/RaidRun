using TMPro;
using UnityEngine.UI;

public class InGameMenu : BaseMenu
{
    public Button PauseButton;
    public TMP_Text currentScore;
    public TMP_Text highScore;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.InGame;
        if (PauseButton) PauseButton.onClick.AddListener(() => SetNextMenu(MenuStates.Pause));
        if (currentScore) currentScore.text = "Current Score: 0"; // Placeholder, update with actual score logic
        if (highScore) highScore.text = "High Score: 0"; // Placeholder, update with actual high score logic
    }
}
