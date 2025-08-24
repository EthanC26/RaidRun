using TMPro;
using UnityEngine.UI;

public class ContolsMenu : BaseMenu
{
    public Button backBtn;
    public TMP_Text controlsTitleTxt;


    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.Controls;

        if (backBtn) backBtn.onClick.AddListener(() => SetNextMenu(MenuStates.MainMenu));
        controlsTitleTxt.text = "CONTROLS";
    }

}
