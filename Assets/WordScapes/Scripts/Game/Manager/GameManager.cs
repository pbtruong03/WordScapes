using DG.Tweening;
using Newtonsoft.Json;
using Sonat;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>
{
    [Header("Component")]
    public int currentLevel;
    public LevelManager levelManager;
    public UIManager uiManager;
    public PopupManager popupManager;

    public Image backgroundGamePlay;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        GameEvent.inMainMenu?.Invoke(true);
    }


    public void WinGame()
    {
        if(currentLevel == DataManager.unlockedLevel)
        {
            ++ DataManager.unlockedLevel;
            uiManager.CloseAllUI();
            // Win new level in the Game
            DOVirtual.DelayedCall(0.2f, () => { popupManager.StartDisplayPopup(); });
        } else
        {
            // Re-win a previously completed level
            StartCoroutine(Wait1Second());
        }

        var log = new SonatLogShowInterstitial();
        Kernel.Resolve<AdsManager>().ShowInterstitial(log);
    }

    IEnumerator Wait1Second()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.DisplayMainMenu();
    }

    public void OnGamePlay(int levelNumber)
    {
        currentLevel = levelNumber;

        string path = $"Data/Level/{DataManager.listLevelID[levelNumber - 1]}";
        TextAsset fileLevel = Resources.Load<TextAsset>(path);
        if (fileLevel == null) return;

        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);

        var cateOfLevel = DataManager.cateOfLevelID[levelNumber];
        backgroundGamePlay.sprite = cateOfLevel.Item1.backgroundImage;
        levelManager.SetLevel(levelData);

        if (levelNumber <= 30)
        {
            Kernel.Resolve<FireBaseController>().LogEvent($"play_level_{levelNumber}");
        }
    }

    private void OnEnable()
    {
        GameEvent.playLevel += OnGamePlay;
    }

    private void OnDisable()
    {
        GameEvent.playLevel -= OnGamePlay;
    }
}
