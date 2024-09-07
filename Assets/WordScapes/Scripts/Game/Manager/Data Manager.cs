using Firebase.RemoteConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : KernelLoadedView
{
    private static DataManager _instance;
    public static DataManager Instance {  get { return _instance; } }


    [Header("Game Data")]
    public GameData gameData;
    public ExtraWordData extraWordData;

    [Header("Data Process")]
    public static int numLevels;
    public static List<string> extraWords;
    public static List<int> listLevelID;
    public static Dictionary<int,Tuple<ChildCategory, int>> cateOfLevelID;
    public Dictionary<ChildCategory, int> dicLevelIdStart = new Dictionary<ChildCategory, int>();

    [Header("Player Data")]
    public static PlayerData playerData;
    public string playerDataKey = "playerData";

    public static int unlockedLevel;
    public static int coin;
    public static int diamond;
    public static int brilliance;

    public static bool musicOn;
    public static bool soundOn;

    [Header("Booster Data")]
    public static int numIdea;
    public static int numPoint;
    public static int numRocket;

    [Space]
    public int costIdea = 100;
    public int costPoint = 200;
    public int costRocket = 300;

    public string stateCurLvKey = "stateCurrentLevel";


    protected override void OnKernelLoaded()
    {
        base.OnKernelLoaded();
        // Singleton
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(this);
            }
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);

        // Init 
        listLevelID = new List<int>();
        cateOfLevelID = new Dictionary<int, Tuple<ChildCategory, int>>();

        LeaderBoardManager.Load();
        LoadNProcessGameData();
        LoadPlayerData();

        GameEvent.coinChanged?.Invoke(coin);
        GameEvent.diamondChanged?.Invoke(diamond);

        costIdea = (int)FirebaseRemoteConfig.DefaultInstance.GetValue("cost_idea").LongValue;
        costPoint = (int)FirebaseRemoteConfig.DefaultInstance.GetValue("cost_point").LongValue;
        costRocket = (int)FirebaseRemoteConfig.DefaultInstance.GetValue("cost_rocket").LongValue;

        Debug.Log($"--------- FireBaseRemote: {costIdea}");
    }

    private void LoadNProcessGameData()
    {
        extraWords = new List<string>(extraWordData.listWords);

        List<ParentCategory> parents = new List<ParentCategory>(gameData.listParent);
        int numParents = parents.Count;

        int levelIdStart = 1;
        foreach(ParentCategory parent in parents)
        {
            foreach(ChildCategory child in parent.listChild)
            {
                dicLevelIdStart.Add(child, levelIdStart);
                levelIdStart += child.listLevelID.Count;

                for(int i = 0; i < child.listLevelID.Count; i++)
                {
                    listLevelID.Add(child.listLevelID[i]);
                    cateOfLevelID.Add(listLevelID.Count, new Tuple<ChildCategory, int>(child, i));
                }
            }
        }
        numLevels = levelIdStart - 1;
    }

    private void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey(playerDataKey))
        {
            string json = PlayerPrefs.GetString(playerDataKey);
            playerData = JsonConvert.DeserializeObject<PlayerData>(json);
        }
        else
        {
            playerData = new PlayerData();

            string json = JsonConvert.SerializeObject(playerData);
            PlayerPrefs.SetString(playerDataKey, json);
        }

        unlockedLevel = playerData.unlockedLevel;
        coin = playerData.coin;
        diamond = playerData.diamond;
        brilliance = playerData.brilliance;

        numIdea = playerData.numIdea;
        numPoint = playerData.numPoint;
        numRocket = playerData.numRocket;

        musicOn = playerData.musicOn;
        soundOn = playerData.soundOn;
    }

    private void SavePlayerData()
    {
        playerData.SetData(unlockedLevel, coin, diamond, brilliance, numIdea, numPoint, numRocket, musicOn, soundOn);

        string json = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        PlayerPrefs.SetString(playerDataKey, json);
    }

    #region ResourcesManager 
    public static void RankUp(int increase)
    {
        brilliance += increase;
        LeaderBoardManager.UpdateRankData();
    }

    public static void EarnCoin(int amount)
    {
        coin += amount;
        GameEvent.coinChanged.Invoke(coin);

        AudioManager.Instance.PlaySFX(AudioType.EarnCoinFx);
    }

    public static bool SpentCoint(int amount)
    {
        if (amount > coin) return false;

        coin -= amount;
        GameEvent.coinChanged?.Invoke(coin);
        return true;
    }

    public static void EarnDiamond(int amount)
    {
        diamond += amount;
        GameEvent.diamondChanged?.Invoke(diamond);
    }

    public static bool SpentDiamond(int amount)
    {
        if (amount > coin) return false;

        diamond -= amount;
        GameEvent.diamondChanged?.Invoke(diamond);
        return true;
    }
    #endregion


    #region Booster Manager
    public void EarnIdeaBooster(int num)
    {
        numIdea += num;
        GameEvent.amountIdeaChanged?.Invoke(numIdea);
    }

    public bool SpentIdeaBooster()
    {
        if(numIdea > 0)
        {
            numIdea--;
            GameEvent.amountIdeaChanged?.Invoke(numIdea);
            return true;
        }

        return SpentCoint(costIdea);
    }

    public void EarnPointBooster(int num)
    {
        numPoint += num;

        GameEvent.amountPointChanged?.Invoke(numPoint);
    }


    public bool EnoughPointBooster()
    {
        if(numPoint > 0 || coin >= costPoint)
        {
            return true;
        }
        return false;
    }

    public void SpentPointBooster()
    {
        if(numPoint > 0)
        {
            numPoint--;
            GameEvent.amountPointChanged?.Invoke(numPoint);
        }
        else
        {
            SpentCoint(costPoint);
        }
    }

    public void EarnRocketBooster(int num)
    {
        numRocket += num;
        GameEvent.amountRocketChanged?.Invoke(numRocket);
    }

    public bool SpentRocketBooster()
    {
        if (numRocket > 0)
        {
            numRocket--;
            GameEvent.amountRocketChanged?.Invoke(numRocket);
            return true;
        }

        return SpentCoint(costRocket);
    }

    #endregion

    public StateCurrentLevel LoadStateCurLevel()
    {
        if (PlayerPrefs.HasKey(stateCurLvKey))
        {
            string json = PlayerPrefs.GetString(stateCurLvKey);
            return JsonConvert.DeserializeObject<StateCurrentLevel>(json);
        }
        return null;
    }

    public void SaveStateCurLevel(StateCurrentLevel stateCurrentLevel)
    {
        string json = JsonConvert.SerializeObject(stateCurrentLevel);
        PlayerPrefs.SetString(stateCurLvKey, json);
    }

    public void ResetData()
    {
        playerData = new PlayerData();
        string json = JsonConvert.SerializeObject(playerData);
        PlayerPrefs.SetString(playerDataKey, json);
        
        LoadPlayerData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SavePlayerData();
        }
    }

    private void OnDisable()
    {
        SavePlayerData();
    }
}


public class StateCurrentLevel
{
    public int levelIndex;
    public List<int> indexVisible;
}