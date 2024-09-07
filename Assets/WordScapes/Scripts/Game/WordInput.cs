using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordInput : MonoBehaviour
{
    public string wordString;
    public TextMeshPro inputWord;

    public Vector3 wordPosition;
    public SpriteRenderer bgRenderer;
    public List<Vector3> listPosLetters = new List<Vector3>();

    private void Start()
    {
        ResetWord();
        wordPosition = transform.localPosition;
    }

    public void AddNewLetter(string letter)
    {
        AudioManager.Instance.PlaySFX(AudioType.AddCharSoundFx, wordString.Length);

        wordString += letter;
        SetDisplayWord();
    }

    public void RemoveLastLetter()
    {
        AudioManager.Instance.PlaySFX(AudioType.RemoveCharSoundFx, wordString.Length);

        wordString = wordString.Remove(wordString.Length - 1, 1);
        SetDisplayWord();
    }

    private void SetDisplayWord()
    {
        inputWord.text = wordString;

        float minWidth = 1.3f;
        bgRenderer.size = new Vector2(Mathf.Max(inputWord.preferredWidth * 2f + 0.5f, minWidth), 1.16f);
        bgRenderer.gameObject.SetActive(wordString != "");
    }

    public void FinishWord()
    {
        if (wordString.Length == 1)
        {
            RemoveLastLetter();
            return;
        }

        listPosLetters.Clear();
        for (int i = 0; i < inputWord.textInfo.characterCount; i++)
        {
            var charInfo = inputWord.textInfo.characterInfo[i];
            float posX = (charInfo.bottomLeft.x + charInfo.bottomRight.x) / 2f;
            listPosLetters.Add(new Vector3(posX, inputWord.transform.position.y, 0f));
        }

        if (LevelManager.Instance.CheckWord(wordString))
        {
            transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1);
        }
        else
        {
            transform.DOPunchPosition(Vector3.right * 0.3f, 0.3f, 22, 1);
        }
        ResetWord();
    }

    public void ResetWord()
    {
        transform.localScale = Vector3.one;
        wordString = string.Empty;

        DOVirtual.DelayedCall(0.3f, () => {
            transform.localPosition = wordPosition;
            inputWord.text = wordString;
            if(wordString == string.Empty)
            {
                bgRenderer.gameObject.SetActive(false);  
            }
        });
    }
}
