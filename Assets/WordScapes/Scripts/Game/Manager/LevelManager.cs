using DG.Tweening;
using Sonat;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : SingletonBase<LevelManager>
{
    [Header("Other Script")]
    public GridBoardManager boardManager;
    public LetterManager letterBoard;
    public InputHandle inputHandle;
    public LineController lineController;
    public DictionaryController dictionaryController;
    public BonusWordController bonusWordController;

    public ExtraWordData extraWordData;
    public List<string> extraWordList;

    public LevelData levelData;

    [Header("Text UI")]
    public TextMeshProUGUI textLevelID;
    public TextMeshProUGUI textCategoryOrder;

    [Header("Level")]
    public int curLevel;
    public Dictionary<string, Word> wordList = new Dictionary<string, Word>();
    public List<string> slovedWordList;

    [Header("Bonus Word")]
    public TextMeshPro textWordBonus;
    public RectTransform bonusWordButton;
    public List<string> listBonusWord;

    public Transform lettersMoveContainers;
    public GameObject letterMovePrefab;
    public Word curWord;

    private int ideaBoosterUsed;
    private int pointBoosterUsed;
    private int rocketBoosterUsed;

    public void Start()
    {
        levelData = new LevelData();
        extraWordList = new List<string>(extraWordData.listWords);
    }

    public void SetLevel(LevelData _levelData)
    {
        if (_levelData == null) return;

        this.levelData = _levelData;
        curLevel = GameManager.Instance.currentLevel;

        var cateOfLevel = DataManager.cateOfLevelID[curLevel];

        textLevelID.text = $"LEVEL {curLevel}";
        textCategoryOrder.text = $"{cateOfLevel.Item1.name} {cateOfLevel.Item2 + 1}";

        ResetData();

        foreach (var word in levelData.words)
        {
            wordList.Add(word.word, word);
        }

        boardManager.LoadNewLevel(levelData);

        dictionaryController.LoadNewLevel(curLevel);

        letterBoard.LoadNewLevel(levelData.letters);
        lineController.InitLine(levelData.letters.Length - 1);

        while (lettersMoveContainers.childCount < levelData.letters.Length)
        {
            Instantiate(letterMovePrefab, lettersMoveContainers).SetActive(false);
        }
    }

    private void ResetData()
    {
        wordList.Clear();
        slovedWordList.Clear();
        listBonusWord.Clear();

        ideaBoosterUsed = 0;
        pointBoosterUsed = 0;
        rocketBoosterUsed = 0;
    }

    public bool CheckWord(string wordstr)
    {
        if (wordList.ContainsKey(wordstr))
        {
            curWord = wordList[wordstr];

            if (!slovedWordList.Contains(wordstr))
            {
                // Slove New Word
                StartCoroutine(CreateMoveLetter());
                NewWordSloved(wordstr);
                dictionaryController.AddNewData(wordstr);// test
            }
            else
            {
                boardManager.SlovedWordAgain(curWord);
                // Word has been solved
                AudioManager.Instance.PlaySFX(AudioType.ExistWordFx);
            }
            return true;
        }
        else if (extraWordList.Contains(wordstr))
        {
            if (!listBonusWord.Contains(wordstr))
            {
                // Dont exist word in level, but word is correct => bonus
                listBonusWord.Add(wordstr);
                StartCoroutine(IEBonusWord(wordstr));
                dictionaryController.AddNewData(wordstr);// test
                bonusWordController.AddNewData(wordstr);

                AudioManager.Instance.PlaySFX(AudioType.BonusWordFx);
            }
            else
            {
                bonusWordButton.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1);
                AudioManager.Instance.PlaySFX(AudioType.ExistWordFx);
            }
            return true;
        }

        AudioManager.Instance.PlaySFX(AudioType.WrongWordFx);

        return false;
    }
    
    public void NewWordSloved(string wordstr)
    {
        AudioManager.Instance.PlaySFX(AudioType.CorrectWordFx);

        slovedWordList.Add(wordstr);
        if (slovedWordList.Count == levelData.words.Count)
        {
            StartCoroutine(IEWin());
        }
    }

    IEnumerator IEBonusWord(string wordStr)
    {
        textWordBonus.text = wordStr;
        textWordBonus.transform.position = inputHandle.wordInput.transform.position;
        textWordBonus.transform.localScale = Vector3.one;
        textWordBonus.gameObject.SetActive(true);

        textWordBonus.transform.DOScale(Vector3.one * 0.8f, 0.7f);
        yield return textWordBonus.transform.DOJump(bonusWordButton.position, -0.2f, 0, 0.7f).WaitForCompletion();

        textWordBonus.gameObject.SetActive(false);
        bonusWordButton.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1);
    }

    IEnumerator CreateMoveLetter()
    {
        int index = curWord.startRowIndex * levelData.numCol + curWord.startColIndex;
        int incrementIndex = (curWord.dir == DirectionType.H) ? 1 : levelData.numCol;
        int wordLength = curWord.word.Length;

        for (int i = 0; i < wordLength; i++)
        {
            var letterMove = lettersMoveContainers.GetChild(i);

            letterMove.GetComponent<LetterMoveController>()?.
                SetLetter(curWord.word[i], inputHandle.wordInput.listPosLetters[i], boardManager.cellDic[index].transform.position);
            
            index += incrementIndex;
            yield return new WaitForSeconds(0.04f);
        }
        yield return new WaitForSeconds(0.3f);
        boardManager.SolvedNewWord(curWord);
    }

    #region Booster
    private void UseConvertBooster()
    {
        letterBoard.ConvertLetter();
        AudioManager.Instance.PlaySFX(AudioType.ConvertBoosterFx);
    }

    private void UseIdeaBooster()
    {
        if (DataManager.Instance.SpentIdeaBooster())
        {
            boardManager.RandomIndexHidden();
            ideaBoosterUsed++;
        }
        else
        {
            PopupManager.Instance.OpenShop();
        }
    }

    private void UseRocketBooster()
    {
        if (DataManager.Instance.SpentRocketBooster())
        {
            boardManager.VisibleFiveCell();
            rocketBoosterUsed++;
        }
        else
        {
            PopupManager.Instance.OpenShop();
        }
    }

    private void UsePointBooster()
    {
        if (DataManager.Instance.EnoughPointBooster())
        {
            GameEvent.onPointerHint?.Invoke(true);
        }
        else
        {
            PopupManager.Instance.OpenShop();
        }
    }

    private void UsedPoint(bool used)
    {
        if (used)
        {
            pointBoosterUsed++;
        }
    }
    #endregion

    IEnumerator IEWin()
    {
        yield return new WaitForSeconds(0.6f);

        if (curLevel == DataManager.unlockedLevel)
        {
            bonusWordController.EarnBonusReward(bonusWordButton.position);
            yield return new WaitForSeconds(1f);
        }

        bonusWordController.ResetData();

        EndLevel(true);

        GameManager.Instance.WinGame();
    }

    public void EndLevel(bool isSuccess)
    {
        var log = new SonatLogLevelEnd()
        {
            level = curLevel,
            success = isSuccess
        };

        log.SetExtraParameter(new[]
        {
            new Sonat.LogParameter("idea_booster_used", ideaBoosterUsed),
            new Sonat.LogParameter("point_booster_used", pointBoosterUsed),
            new Sonat.LogParameter("rocket_booster_used", rocketBoosterUsed)
        });

        log.Post();
    }

    private void OnEnable()
    {
        GameEvent.onClickConvertLetters += UseConvertBooster;
        GameEvent.onClickIdea += UseIdeaBooster;
        GameEvent.onClickRocket += UseRocketBooster;
        GameEvent.onClickPoint += UsePointBooster;
        GameEvent.onPointerHint += UsedPoint;
    }

    private void OnDisable()
    {
        GameEvent.onClickConvertLetters -= UseConvertBooster;
        GameEvent.onClickIdea -= UseIdeaBooster;
        GameEvent.onClickRocket -= UseRocketBooster;
        GameEvent.onClickPoint -= UsePointBooster;
        GameEvent.onPointerHint -= UsedPoint;
    }
}
