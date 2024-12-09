using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : Menu<GameOverMenu>
{
    [SerializeField] private TMPro.TextMeshProUGUI CoinText;
    [SerializeField] private TMPro.TextMeshProUGUI XPText;
    [SerializeField] private TMPro.TextMeshProUGUI HeartText;
    [SerializeField] private GameObject WonAsset;
    [SerializeField] private GameObject LostAsset;

    public override void OnActivated()
    {
    }

    public override void OnBackPressed()
    {
        Hide();
    }

    public void Show(bool won, int coinRewardAmount, int xpRewardAmount, int attemptsLeft)
    {
        PersistentProfile.Instance.Coin += coinRewardAmount;
        PersistentProfile.Instance.XP += xpRewardAmount;
        PersistentProfile.Save();

        WonAsset.SetActive(won);
        LostAsset.SetActive(!won);

        CoinText.text = coinRewardAmount.ToString();
        XPText.text = xpRewardAmount.ToString();
        HeartText.text = attemptsLeft.ToString();
        base.Show();
    }
}
