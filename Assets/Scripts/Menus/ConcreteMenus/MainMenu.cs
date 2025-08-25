using TMPro;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    public Button playBtn;
    public Button controlsBtn;
    public Button creditsBtn;
    public Button SettingsBtn;
    public TMP_Text Title;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.MainMenu;
        if(playBtn) playBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Mode));
        if (controlsBtn) controlsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Controls));
        if(creditsBtn) creditsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Credits));
        if (SettingsBtn) SettingsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Settings));
        Title.text = "RAID RUN";
    }
}
