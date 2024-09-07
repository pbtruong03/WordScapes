using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class LeaderBoardManager
{
    private static string leaderBoardDataKey = "leaderBoardDataKey";
    private static string fileNames = "leaderboard-default";
    private static int maxBrilliance;

    [Serializable]
    public class LeaderBoardData
    {
        public List<RankData> rankDatas = new List<RankData>();
    }

    public class ListNameData
    {
        public List<string> names = new List<string>();
    }

    public static LeaderBoardData leaderBoardData
    {
        get
        {
            if (PlayerPrefs.HasKey(leaderBoardDataKey))
            {
                string json = PlayerPrefs.GetString(leaderBoardDataKey);
                return JsonConvert.DeserializeObject<LeaderBoardData>(json);
            } 
            else
            {
                var defaultLeaderBoard = new LeaderBoardData();
                var defaultNames = new ListNameData();

                string json = Resources.Load<TextAsset>(fileNames).text;
                defaultNames = JsonConvert.DeserializeObject<ListNameData>(json);
                int numberPlayers = defaultNames.names.Count;

                int curBrilliance = DataManager.brilliance;
                defaultLeaderBoard.rankDatas.Add(new RankData("You", curBrilliance));

                maxBrilliance = DataManager.numLevels * 13;

                for (int i = 0; i < numberPlayers; i++)
                {
                    int randBrilliance = curBrilliance + UnityEngine.Random.Range(-200, maxBrilliance - curBrilliance);
                    if (randBrilliance < 5)
                        randBrilliance = 5;

                    defaultLeaderBoard.rankDatas.Add(new RankData(defaultNames.names[i], randBrilliance));
                }

                defaultLeaderBoard.rankDatas.Sort();

                json = JsonConvert.SerializeObject(defaultLeaderBoard);
                PlayerPrefs.SetString(leaderBoardDataKey, json);

                return defaultLeaderBoard;
            }
        }

        set
        {
            PlayerPrefs.SetString(leaderBoardDataKey, JsonConvert.SerializeObject(value));
        }
    }

    public static LeaderBoardData saveLeaderBoard;

    public static string SaveLastDay
    {
        get
        {
            if (PlayerPrefs.HasKey("SaveLastDayLeaderboard"))
            {
                return PlayerPrefs.GetString("SaveLastDayLeaderboard");
            }
            else
            {
                DateTime dateCurrent = DateTime.Now;
                string json = JsonConvert.SerializeObject(dateCurrent);
                PlayerPrefs.SetString("SaveLastDayLeaderboard", json);
                return json;
            }
        }
        set
        {
            PlayerPrefs.SetString("SaveLastDayLeaderboard", value);
        }
    }

    public static void Save()
    {
        leaderBoardData = saveLeaderBoard;
    }

    public static void Load()
    {
        saveLeaderBoard = leaderBoardData;

        DateTime dateCurrent = DateTime.Now;
        DateTime dateLastSave = JsonConvert.DeserializeObject<DateTime>(SaveLastDay);

        int days = (dateLastSave - dateCurrent).Days;
        if (days > 0)
        {
            UpdateAllLeaderboard(true);
            SaveLastDay = JsonConvert.SerializeObject(dateCurrent);
        }
    }

    public static int FindPlayerIndex()
    {
        int index = saveLeaderBoard.rankDatas.FindIndex(player => player.playerName == "You");
        return index;
    }

    public static void UpdateRankData()
    {
        int indexPlayer = FindPlayerIndex();

        saveLeaderBoard.rankDatas[indexPlayer].totalBrilliance = DataManager.brilliance;

        UpdateAllLeaderboard(false);
    }


    public static void UpdateAllLeaderboard(bool isByDay)
    {
        maxBrilliance = DataManager.numLevels * 13;

        for (int i = 0; i < saveLeaderBoard.rankDatas.Count; i++)
        {
            var player = saveLeaderBoard.rankDatas[i];

            if (player.playerName == "You") continue;

            System.Random random = new System.Random();

            var (minn, maxx) = isByDay ? RandomIncreaseLevelByDay() : RandomIncreaseLevelByRuntime();

            saveLeaderBoard.rankDatas[i].totalBrilliance += random.Next(minn, maxx);
            if (saveLeaderBoard.rankDatas[i].totalBrilliance > maxBrilliance)
            {
                saveLeaderBoard.rankDatas[i].totalBrilliance = maxBrilliance;
            }
        }

        saveLeaderBoard.rankDatas.Sort();

        Save();
    }

    private static (int, int) RandomIncreaseLevelByDay()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(1, 101);

        int minIncrease, maxIncrease;

        if (randomNumber <= 1)
        {
            minIncrease = 275;
            maxIncrease = 350;
        }
        else if (randomNumber <= 3)
        {
            minIncrease = 185;
            maxIncrease = 275;
        }
        else if (randomNumber <= 6)
        {
            minIncrease = 100;
            maxIncrease = 185;
        }
        else if (randomNumber <= 10)
        {
            minIncrease = 75;
            maxIncrease = 100;
        }
        else if (randomNumber <= 30)
        {
            minIncrease = 40;
            maxIncrease = 75;
        }
        else if (randomNumber <= 60)
        {
            minIncrease = 10;
            maxIncrease = 40;
        }
        else
        {
            minIncrease = 3;
            maxIncrease = 10;
        }

        return (minIncrease, maxIncrease);
    }

    private static (int, int) RandomIncreaseLevelByRuntime()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(1, 101);

        int minIncrease, maxIncrease;

        if (randomNumber <= 40)
        {
            minIncrease = 3;
            maxIncrease = 12;
        }
        else
        {
            minIncrease = 0;
            maxIncrease = 0;
        }

        return (minIncrease, maxIncrease);
    }
}
