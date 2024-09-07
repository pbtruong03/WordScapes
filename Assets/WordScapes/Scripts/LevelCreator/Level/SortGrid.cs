#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum FlagDir
{
    // Direction of the letter have (nothing, horizontal, vertical, both horizontal and vertical)
    N, 
    H,
    V,
    HV,
}
public class SortGrid : MonoBehaviour
{
    //Optimal Solution
    public List<Word> optimalWords;
    private int optimalNumRows;
    private int optimalNumCols;
    private int minRowCol;

    // Data for level data
    private List<Word> words;
    private Dictionary<string, Word> dicWords;
    private int numRows;
    private int numCols;

    private Dictionary<char, List<Tuple<int, int>>> charPos;
    private FlagDir[,] flagPos;
    private char[,] charPlaced;
    private int n = 50;

    private List<string> listWord;
    private string bonusWord;

    private void Start()
    {
        optimalWords = new List<Word>();

        dicWords = new Dictionary<string, Word>();
        charPos = new Dictionary<char, List<Tuple<int, int>>>();
        flagPos = new FlagDir[n, n];
        charPlaced = new char[n, n];
        listWord = new List<string>();
        words = new List<Word>();
    }
    public bool SortGridWords()
    {
        minRowCol = 999;

        PreSortGrid();

        if (listWord.Count > 0)
        {
            // Try sort
            for(int i = 0; i < 500; i++)
            {
                PreSortGrid();
                PlaceFirstWord();
                if (PlaceListWord())
                {
                    PlaceBonusWord();
                    FomatGrid();
                    if (Mathf.Max(numCols, numRows) < minRowCol)
                    {
                        minRowCol = Mathf.Max(numCols, numRows); 

                        optimalNumCols = numCols;
                        optimalNumRows = numRows;
                        optimalWords = words.ToList();
                    }
                }
            }

            if (minRowCol < 999)
            {
                CreatorManager.Instance.numCols = optimalNumCols;
                CreatorManager.Instance.numRows = optimalNumRows;
                CreatorManager.Instance.words = optimalWords;
                return true;
            }

            Debug.LogWarning("No have solution for this list word, please click SortGrid Button Again");
        } else
        {
            Debug.LogWarning("word list is empty, please add more words and try again");
        }
        return false;
    }

    // Processing Pre Sort Grid
    private void PreSortGrid()
    {
        listWord.Clear();
        listWord = new List<string>(CreatorManager.Instance.selectedWords); // Load list word for level
        bonusWord = CreatorManager.Instance.bonusWord;

        if (bonusWord != "")
        {
            listWord.Remove(bonusWord);
        }

        words.Clear();
        dicWords.Clear();
        charPos.Clear();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                flagPos[i,j] = FlagDir.N;
                charPlaced[i, j] = ' ';
            }
        }
    }


    #region DEFAULT WORD
    // Random word in list and sort first word in the center grid
    private void PlaceFirstWord()
    {
        string word = listWord[UnityEngine.Random.Range(0, listWord.Count)]; // random word
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            TryPlaceWordHorizontal(word, 20, 20);
        }
        else
        {
            TryPlaceWordVertical(word, 20, 20);
        }
    }
    // Set Flag for cell [x,y] in matrix
    private void SetFlag(int row, int col, FlagDir flagType)
    {
        if (flagPos[row, col] == FlagDir.N)
        {
            flagPos[row, col] = flagType;
        } else
        {
            flagPos[row, col] = FlagDir.HV;
        }
    }
    // Try Place Word
    private bool TryPlaceWordHorizontal(string word, int startRow, int startCol)
    {
        // Check length
        if(word.Length + startCol >= n) { return false; }
        for(int i = 0; i < word.Length; i++)
        {
            // Check have letter ?
            if (charPlaced[startRow,startCol+i] != ' ' && charPlaced[startRow,startCol+i] != word[i]) return false;
            // Check dir ?
            if (flagPos[startRow, startCol + i] == FlagDir.H || flagPos[startRow, startCol + i] == FlagDir.HV) return false; 
        }


        // check start and end //fix bug 16/07
        if (startCol - 1 >= 0)
        {
            if (flagPos[startRow, startCol - 1] == FlagDir.V || flagPos[startRow, startCol - 1] == FlagDir.HV) { return false; }

            SetFlag(startRow, startCol - 1, FlagDir.HV);
        }

        if (startCol + word.Length < n)
        {
            if (flagPos[startRow, startCol + word.Length] == FlagDir.V || flagPos[startRow, startCol + word.Length] == FlagDir.HV) { return false; }

            SetFlag(startRow, startCol + word.Length, FlagDir.HV);
        }

        Word w = new Word();
        w.word = word;
        w.dir = DirectionType.H;
        w.startRowIndex = startRow;
        w.startColIndex = startCol;

        dicWords[w.word] = w;
        listWord.Remove(word);

        for(int i = 0; i < word.Length; i++)
        {
            // Add charPos
            Tuple<int, int> pos = new Tuple<int, int>(startRow, startCol + i);
            if (charPos.ContainsKey(word[i]))
            {
                charPos[word[i]].Add(pos);
            }
            else
            {
                charPos[word[i]] = new List<Tuple<int, int>> { pos };
            }
            // Add char placed and Flag pos
            charPlaced[startRow, startCol+i] = word[i];
            SetFlag(startRow, startCol + i, FlagDir.H);

            if (i > 0 && i < word.Length - 1)
            {
                if (startRow - 1 >= 0)
                {
                    SetFlag(startRow -1, startCol +i, FlagDir.H);
                }
                if (startRow + 1 < n)
                {
                    SetFlag(startRow + 1, startCol + i, FlagDir.H);
                }
            }
        }

        return true;
    }

    private bool TryPlaceWordVertical(string word, int startRow, int startCol)
    {
        // Check length
        if (word.Length + startRow >= n) { return false; }
        for (int i = 0; i < word.Length; i++)
        {
            // Check have letter ?
            if (charPlaced[startRow + i, startCol] != ' ' && charPlaced[startRow + i, startCol] != word[i]) return false;
            // Check dir ?
            if (flagPos[startRow + i, startCol] == FlagDir.V || flagPos[startRow + i, startCol] == FlagDir.HV) return false;
        }
        // check start and end
        if (startRow - 1 >= 0)
        {
            if (flagPos[startRow - 1, startCol] == FlagDir.H || flagPos[startRow -1, startCol] == FlagDir.HV) { return false; }

            SetFlag(startRow - 1, startCol, FlagDir.HV);
        }
        if (startRow + word.Length < n)
        {
            if (flagPos[startRow + word.Length, startCol] == FlagDir.H || flagPos[startRow + word.Length, startCol] == FlagDir.HV) { return false; }

            SetFlag(startRow + word.Length, startCol, FlagDir.HV);
        }


        Word w = new Word();
        w.word = word;
        w.dir = DirectionType.V;
        w.startRowIndex = startRow;
        w.startColIndex = startCol;

        dicWords[w.word] = w;
        listWord.Remove(word);

        for (int i = 0; i < word.Length; i++)
        {
            // Add charPos
            Tuple<int, int> pos = new Tuple<int, int>(startRow + i, startCol);
            if (charPos.ContainsKey(word[i]))
            {
                charPos[word[i]].Add(pos);
            }
            else
            {
                charPos[word[i]] = new List<Tuple<int, int>> { pos };
            }
            // Add char placed and Flag pos
            charPlaced[startRow + i, startCol] = word[i];
            SetFlag(startRow +i, startCol, FlagDir.V);
            if(i > 0 && i < word.Length - 1)
            {
                if(startCol-1 >= 0)
                {
                    SetFlag(startRow + i, startCol - 1, FlagDir.V);
                }
                if(startCol+1 < n)
                {
                    SetFlag(startRow + i, startCol + 1, FlagDir.V);
                }
            }
        }

        return true;
    }

    private bool PlaceWord(string word)
    {
        for (int charIndex = 0; charIndex < word.Length; charIndex++)
        {
            char letter = word[UnityEngine.Random.Range(0, word.Length)];
            if (charPos.ContainsKey(letter))
            {
                List<Tuple<int, int>> listPos = new List<Tuple<int, int>>(charPos[letter]);
                for (int i = 0; i < listPos.Count; i++)
                {
                    int r = listPos[i].Item1;
                    int c = listPos[i].Item2;
                    if (flagPos[r, c] == FlagDir.HV) continue;

                    if (flagPos[r, c] != FlagDir.H)
                    {
                        if (TryPlaceWordHorizontal(word, r, c - charIndex)) return true;
                    }
                    if (flagPos[r, c] != FlagDir.V)
                    {
                        if(TryPlaceWordVertical(word, r - charIndex, c)) return true;
                    }
                }
            }
        }
        return false;
    }

    private bool PlaceListWord()
    {
        int counter = 0;
        while (listWord.Count > 0)
        {
            string word = listWord[UnityEngine.Random.Range(0, listWord.Count)];
            if (!PlaceWord(word)){
                counter++;
                if (counter >= listWord.Count)
                {
                    return false;
                }
            }
            else
            {
                counter = 0;
            }
        }
        return true;
    }

    #endregion


    #region BONUS WORD

    bool[,] marksV = new bool[50, 50];
    bool[,] marksH = new bool[50, 50];  
    int bonusLen;

    private void UnMarkAll()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                marksV[i, j] = false;
                marksH[i, j] = false;
            }
        }
    }

    private void AddBonusWord(int startRow, int startCol, DirectionType directionType)
    {
        Word word = new Word();
        word.word = bonusWord;
        word.startRowIndex = startRow;
        word.startColIndex = startCol;
        word.dir = directionType;
        word.isBonusWord = true;

        dicWords.Add(bonusWord, word);

        if (directionType == DirectionType.H)
        {
            for (int i = 0; i < bonusLen; i++)
            {
                charPlaced[startRow, startCol + i] = bonusWord[i];
            }
        }
        else
        {
            for (int i = 0; i < bonusLen; i++)
            {
                charPlaced[startRow+i, startCol] = bonusWord[i];
            }
        }
    }

    private bool TryPlaceBonusHorizontal(int row, int startCol, int lastCol)
    {
        if (charPlaced[row-1, startCol - 1] != ' ' || charPlaced[row+1, startCol -1] != ' ') 
            return false;

        if (charPlaced[row - 1, startCol + bonusLen] != ' ' || charPlaced[row + 1, startCol + bonusLen] != ' ')
            return false;


        int counter = 0;
        for (int i = startCol; i < lastCol; i++)
        {
            marksH[row, i] = true;
            if (flagPos[row, i] != FlagDir.N) break;
            
            counter++;
            if (counter >= bonusLen)
            {
                AddBonusWord(row, startCol, DirectionType.H);
                return true;
            }
        }
        
        return false;
    }

    private bool TryPlaceBonusVertical(int col, int startRow, int lastRow)
    {
        if (charPlaced[startRow - 1, col - 1] != ' ' || charPlaced[startRow - 1, col + 1] != ' ')
            return false;

        if (charPlaced[startRow + bonusLen, col - 1] != ' ' || charPlaced[startRow + bonusLen, col + 1] != ' ')
            return false;

        int counter = 0;
        
        for (int i = startRow; i < lastRow; i++)
        {
            marksV[i, col] = true;
            if (flagPos[i, col] != FlagDir.N) break;

            counter++;
            if (counter >= bonusLen)
            {
                AddBonusWord(startRow, col, DirectionType.V);
                return true;
            }
        }

        return false;
    }


    private void PlaceBonusWord()
    {
        if (bonusWord == "") return;

        bonusLen = bonusWord.Length;

        int firstRow = n - 1;
        int lastRow = 0;
        int firstCol = n - 1;
        int lastCol = 0;

        // Update value firstRow, lastRow, firstCol, lastCol
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (charPlaced[i, j] != ' ')
                {
                    firstRow = Math.Min(firstRow, i);
                    firstCol = Math.Min(firstCol, j);
                    lastRow = Math.Max(lastRow, i);
                    lastCol = Math.Max(lastCol, j);
                }
            }
        }

        for (int k = 0; firstRow > 0 && firstCol > 0 && lastCol < n && lastRow < n; k++)
        {
            
            UnMarkAll();

            for (int i = firstRow; i <= lastRow; i++)
            {
                for (int j = firstCol; j <= lastCol; j++)
                {
                    if (flagPos[i, j] != FlagDir.N) continue;
                    
                    // Try
                    if (!marksH[i, j]){
                        if (TryPlaceBonusHorizontal(i, j, lastCol))
                        {
                            return;
                        }
                    }
                    // Try
                    if (!marksV[i, j]) 
                    {
                        if (TryPlaceBonusVertical(j, i, lastRow))
                        {
                            return;
                        }
                    }
                }
            }

            switch (k % 4)
            {
                case 0: firstRow--; break;
                case 1: firstCol--; break;
                case 2: lastRow++; break;
                case 3: lastCol++; break;
            }
        }

        Debug.LogWarning("Cannot add bonus word");
    }

    #endregion

    private void FomatGrid()
    {
        int firstRow = n - 1;
        int lastRow = 0;
        int firstCol = n - 1;
        int lastCol = 0;

        // Update value firstRow, lastRow, firstCol, lastCol
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (charPlaced[i, j] != ' ')
                {
                    firstRow = Math.Min(firstRow, i);
                    firstCol = Math.Min(firstCol, j);
                    lastRow = Math.Max(lastRow, i);
                    lastCol = Math.Max(lastCol, j);
                }
            }
        }
        numRows = lastRow - firstRow + 1;
        numCols = lastCol - firstCol + 1;

        foreach (var word in dicWords)
        {
            word.Value.startColIndex -= firstCol;
            word.Value.startRowIndex -= firstRow;
            words.Add(word.Value);
        }
    }
}


#endif