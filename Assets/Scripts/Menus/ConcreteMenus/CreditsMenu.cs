using TMPro;
using UnityEngine.UI;

public class CreditsMenu : BaseMenu
{
    public Button backBtn;
    public TMP_Text creditsTitleTxt;
    public TMP_Text creditsTxt;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.Credits;
        if (backBtn) backBtn.onClick.AddListener(() => SetNextMenu(MenuStates.MainMenu));
        creditsTitleTxt.text = "CREDITS";
        creditsTxt.text = "Game developed by Ethan Cullen\nThanks for playing!";
    }
}
