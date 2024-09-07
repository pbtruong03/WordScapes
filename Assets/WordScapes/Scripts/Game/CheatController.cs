using Sonat;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatController : MonoBehaviour
{
    [Header("UI Component")]
    public Button titleSettings;
    [Space]
    public GameObject loginCheatPanel;
    public TMP_InputField inputPw;
    public Button confirmBtn;
    public Button closeButton;
    [Space]
    public GameObject cheatContainer;
    public Button openCheat;
    public GameObject cheatPanel;
    [Space]
    public TMP_InputField inputLevel;
    public TextMeshProUGUI textSetLevel;
    public Button setLevelBtn;
    [Space]
    public Button testWinBtn;
    public Button buffCoinBtn;
    public Button buffDiamondBtn;
    public Button buffIdeaBtn;
    public Button buffPointBtn;
    public Button buffRocketBtn;
    public Button resetDataBtn;

    public Button noAdsBtn;
    public TextMeshProUGUI noAdsText;
    public Button showInterAds;
    public Button showVideoAds;

    public Button debugWindowBtn;

    public Button setDummyBtn;
    public TextMeshProUGUI dummyText;

    private int counter;
    private int timeWait;
    


    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        timeWait = 0;

        titleSettings.onClick.AddListener(TryOpenCheat);
        confirmBtn.onClick.AddListener(CheckPassword);
        testWinBtn.onClick.AddListener(TestWin);

        openCheat.onClick.AddListener(SetCheatPanel);

        setLevelBtn.onClick.AddListener(SetLevel);

        buffCoinBtn.onClick.AddListener(() => DataManager.EarnCoin(10000));
        buffDiamondBtn.onClick.AddListener(() => DataManager.EarnDiamond(10000));
        buffIdeaBtn.onClick.AddListener(() => DataManager.Instance.EarnIdeaBooster(30));
        buffPointBtn.onClick.AddListener(() => DataManager.Instance.EarnPointBooster(30));
        buffRocketBtn.onClick.AddListener(() => DataManager.Instance.EarnRocketBooster(30));

        resetDataBtn.onClick.AddListener(ResetData);

        noAdsBtn.onClick.AddListener(SetAds);
        showInterAds.onClick.AddListener(TestShowInterAds);
        showVideoAds.onClick.AddListener(TestShowVideoAds);

        debugWindowBtn.onClick.AddListener(() => { Kernel.kernel.AddOnScreenDebugLog(); });
        setDummyBtn.onClick.AddListener(SetDummy);
    }

    private void TryOpenCheat()
    {
        if (DateTime.Now.Second - timeWait <= 5)
        {
            counter++;
            if (counter >= 5)
            {
                loginCheatPanel.SetActive(true);
            }
        }
        else
        {
            counter = 0;
            timeWait = DateTime.Now.Second;
        }
    }

    private void CheckPassword()
    {
        string pw = inputPw.text;
        if (pw == "pbtruong03")
        {
            cheatContainer.SetActive(true);
        }

        loginCheatPanel.SetActive(false);
    }

    private void SetCheatPanel()
    {
        cheatPanel.SetActive(!cheatPanel.activeSelf);
    }

    private void TestWin()
    {

    }

    private void SetLevel()
    {
        if (inputLevel.text.Length == 0) return;

        int level = int.Parse(inputLevel.text);

        if (level > 0 && level <= DataManager.listLevelID.Count)
        {
            DataManager.unlockedLevel = level;
            textSetLevel.text = "Set Level: v";
        }
        else
        {
            textSetLevel.text = "Set Level: x";
        }
    }

    private void ResetData() 
    { 
        DataManager.Instance.ResetData();
    }

    private void SetAds()
    {
        if (Kernel.Resolve<AdsManager>().IsNoAds())
        {
            Kernel.Resolve<AdsManager>().DisableNoAds();
            noAdsText.text = "noAds = False";
        }
        else
        {
            Kernel.Resolve<AdsManager>().EnableNoAds();
            noAdsText.text = "noAds = True";
        }
    }

    private void TestShowInterAds()
    {
        var log = "Test".CreateDefaultLogInterstitial();
        Kernel.Resolve<AdsManager>().ShowInterstitial(log);
    }

    private void TestShowVideoAds()
    {
        var log = new SonatLogVideoRewarded();
        Kernel.Resolve<AdsManager>().ShowVideoAds(() => { }, log);
    }

    private void SetDummy()
    {
        bool dummy = Kernel.Resolve<BasePurchaser>().Dummy;
        Kernel.Resolve<BasePurchaser>().SetDummy(!dummy);
        dummyText.text = $"Dummy = {!dummy}";
    }
}
