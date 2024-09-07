using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LetterManager : MonoBehaviour
{
    [Header("Letter")]
    public GameObject letterCellPrefabs;
    public List<GameObject> listLetterCell;
    public string letters;

    [Header("Circle Properties")]
    public float radius;
    public int numLetter;
    public float angle;
    public List<Vector3> lettersPosition = new List<Vector3>();
    [Space]
    private Vector3 scaleLetter;



    public void LoadNewLevel(string _letters)
    {
        letters = _letters;
        lettersPosition.Clear();

        numLetter = letters.Length;
        angle = (Mathf.PI * 2) / numLetter;
        float scaleValue = (numLetter > 5) ? 0.45f : 0.55f;
        scaleLetter = new Vector3(scaleValue, scaleValue, 1f);
        SpawnLetter();
    }

    private void SpawnLetter()
    {
        while (listLetterCell.Count < letters.Length)
        {
            var cell = Instantiate(letterCellPrefabs, this.transform);
            listLetterCell.Add(cell);
        }
        
        foreach(var letterCell in listLetterCell)
        {
            letterCell.SetActive(false);
        }

        for (int i = 0; i < letters.Length; i++)
        {
            var cell = listLetterCell[i];
            SetCell(cell, i);
        }
        StartCoroutine(IELoadPositionLetters());
    }

    IEnumerator IELoadPositionLetters()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < letters.Length; i++)
        {
            var cell = listLetterCell[i];
            lettersPosition.Add(cell.transform.position);
        }
    }

    private void SetCell(GameObject cell, int index)
    {
        Vector3 position = new Vector3(Mathf.Cos(angle * index + Mathf.PI / 2), Mathf.Sin(angle * index + Mathf.PI / 2), 0) * radius;
        cell.transform.SetLocalPositionAndRotation(position, Quaternion.identity);
        cell.transform.localScale = scaleLetter;
        cell.GetComponent<InputCell>()?.SetLetter(letters[index].ToString());
        cell.SetActive(true);
    }

    public void ConvertLetter()
    {
        //int numConvert = UnityEngine.Random.Range(1, LetterManager.Instance.listLetterCell.Count +1);
        int numConvert = listLetterCell.Count;
        while (numConvert > 0)
        {
            int index1 = Random.Range(0, listLetterCell.Count);
            int index2 = Random.Range(0, listLetterCell.Count);

            Vector3 tmp = lettersPosition[index1];
            lettersPosition[index1] = lettersPosition[index2];
            lettersPosition[index2] = tmp;

            numConvert--;
        }

        for (int i = 0; i < listLetterCell.Count; i++)
        {
            if (listLetterCell[i].transform.position == lettersPosition[i]) continue;

            listLetterCell[i].transform.DOJump(lettersPosition[i], 0.2f, 0, 0.25f, false);
        }
    }
}
