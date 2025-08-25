using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeMenu : BaseMenu
{
    public Button TimedMode;
    public Button EndlessMode;
    public TMP_Text ModeTitle;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.Mode;
        if (TimedMode)
        {
            TimedMode.onClick.AddListener(() =>
            {
                GameManager.Instance.UpdateGameMode(GameMode.Timed);
                SceneManager.LoadScene("InGame");
            });
        }
        if (EndlessMode)
        {
            EndlessMode.onClick.AddListener(() =>
            {
                GameManager.Instance.UpdateGameMode(GameMode.Endless);
                SceneManager.LoadScene("InGame");
            });
        }
        if (ModeTitle) ModeTitle.text = "SELECT GAME MODE";
    }
}
