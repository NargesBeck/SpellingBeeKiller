using UnityEngine;

public class HomeMenu : Menu<HomeMenu>
{
    [SerializeField] private TMPro.TextMeshProUGUI UsernameText;

    private void Start()
    {
        OnActivated();
    }

    public override void OnActivated()
    {
        UsernameText.text = PersistentProfile.Instance.UserName;
    }

    public override void OnBackPressed()
    {
        Hide();
    }

    public void PlayClassicClick()
    {
        ClassicLevelMenu.Instance.MockShow();
    }
}
