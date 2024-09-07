#if UNITY_EDITOR

using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatorManager : MonoBehaviour
{
    private static CreatorManager _instance;
    public static CreatorManager Instance {  get { return _instance; } }

    [Header("Data For Level")]
    public LevelData levelData;
    public int numRows;
    public int numCols;
    public string listLetter;
    public List<Word> words;

    [Header("Input Field")]
    public TMP_InputField inputNumRows;
    public TMP_InputField inputNumCols;
    public TMP_InputField inputMaxLetter;
    public TMP_InputField inputKeyword;

    [Header("Button")]
    public Button searchWordButton;
    public Button resetButton;
    public Button sortGridButton;

    [Header("List Words")]
    public GameObject objListLetter;
    public List<string> availableWords;
    public List<string> selectedWords;
    public string bonusWord;
    [Space]
    public ListWordScroll objSelectedWords;
    public ListWordScroll objavailableWords;

    public TextMeshProUGUI textBonusWord;

    [Header("Data")]
    public ExtraWordData extraWordData;

    public void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        } else
        {
            _instance = this;
        }
    }
    public void Start()
    {
        //levelData = new LevelDataDTO();
        words = new List<Word>();

        searchWordButton.onClick.AddListener(() => SearchWord());
        resetButton.onClick.AddListener(() => ResetData());
        sortGridButton.onClick.AddListener(() => SortGird());
    }

    // Choose the word with the appropriate number of letters
    public void Search()
    {
        int maxLetter;
        if(inputMaxLetter.text != "")
        {
            maxLetter = int.Parse(inputMaxLetter.text);
        } else
        {
            maxLetter = 5;
            inputMaxLetter.text = "5";
        }
        availableWords.Clear();

        Dictionary<char, int> letterCount = new Dictionary<char, int>();
        foreach(char c in listLetter)
        {
            if (letterCount.ContainsKey(c))
            {
                letterCount[c]++;
            }else
            {
                letterCount[c] = 1;
            }
        }

        Dictionary<char, int> tmp = new Dictionary<char, int>();
        foreach (string word in extraWordData.listWords)
        {
            if (word.Length <= maxLetter && !selectedWords.Contains(word))
            {
                foreach(var k in letterCount)
                {
                    tmp[k.Key] = k.Value;
                }
                int numLetterMiss = 0;
                foreach (char c in word)
                {
                    if (tmp.ContainsKey(c) && tmp[c] > 0)
                    {
                        tmp[c]--;
                    }
                    else
                    {
                        numLetterMiss++;
                    }
                }
                if (numLetterMiss <= maxLetter - listLetter.Length)
                {
                    availableWords.Add(word);
                }
            }
        }
    }

    public void SearchWord()
    {
        string keyword = (inputKeyword.text).ToUpper();
        var result = new List<string>();
        Search();
        foreach (string word in availableWords)
        {
            if (word.Contains(keyword))
            {
                result.Add(word);
            }
        }
        availableWords = result;

        for (int i = 0; i < availableWords.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, availableWords.Count);
            string tmp = availableWords[randIndex];
            availableWords[randIndex] = availableWords[i];
            availableWords[i] = tmp;
        }

        objavailableWords.UpdateList();
    }

    private void AddBonusWord(string word)
    {
        if (bonusWord == word)
        {
            bonusWord = "";
        }
        else
        {
            bonusWord = word;
        }
        textBonusWord.text = bonusWord;
    }



    public void UpdateListLetter()
    {
        selectedWords = objSelectedWords.wordList; //Update Selected Word

        Dictionary<char, int> letterCount = new Dictionary<char, int>();
        foreach (string word in selectedWords)
        {
            Dictionary<char, int> curLetterCount = new Dictionary<char, int>();
            foreach (char letter in word)
            {
                if(curLetterCount.ContainsKey(letter)){
                    curLetterCount[letter]++;
                } else
                {
                    curLetterCount[letter] = 1;
                }
            }
            foreach (var k in curLetterCount)
            {
                if (letterCount.ContainsKey(k.Key))
                {
                    letterCount[k.Key] = Math.Max(letterCount[k.Key], k.Value);
                } else
                {
                    letterCount[k.Key] = k.Value;
                }
            }
        }
        listLetter = "";
        foreach(var k in letterCount)
        {
            for(int i = 0; i < k.Value; i++)
            {
                listLetter += k.Key;
            }
        }
        objListLetter.GetComponent<TextMeshProUGUI>().text = listLetter;
        SearchWord();
    }

    public void SortGird()
    {
        SortGrid sort = gameObject.GetComponent<SortGrid>();
        if (sort.SortGridWords())
        {
            levelData = new LevelData();

            inputNumCols.text = numCols.ToString();
            inputNumRows.text = numRows.ToString();

            levelData.numCol = numCols;
            levelData.numRow = numRows;
            levelData.letters = new string(listLetter.OrderBy(c => Guid.NewGuid()).ToArray()); 

            levelData.words = sort.optimalWords;

            GridBoardManager.Instance.LoadNewLevel(levelData);
            GridBoardManager.Instance.DisplaySloved();
            levelData.brilliancePoint = GridBoardManager.Instance.cellDic.Count;
        } 
        else
        {
            levelData = null;
            GridBoardManager.Instance.levelData = null;
            GridBoardManager.Instance.ResetGridBoard();
        }
    }


    public void ResetData()
    {
        objSelectedWords.RemoveList();
        levelData = null;
        bonusWord = "";
        textBonusWord.text = bonusWord;

        GridBoardManager.Instance.levelData = null;
        GridBoardManager.Instance.ResetGridBoard();

    }


    private void OnEnable()
    {
        CreatorEvent.onBonusWord += AddBonusWord;
    }

    private void OnDisable()
    {
        CreatorEvent.onBonusWord -= AddBonusWord;
    }
}


#endif