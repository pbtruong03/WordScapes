#if UNITY_EDITOR

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//public enum WordType
//{
//    available,
//    selected,
//    bonusWord
//}


public class WordButton : MonoBehaviour, IPointerDownHandler
{
    public string word;
    public bool isSelected;
    //public WordType type;

    public TextMeshProUGUI textWord;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnWordClick);
    }
    public void SetWord(string word)
    {
        this.word = word;
        //type = WordType.available;
        isSelected = false;
        textWord.text = word;
    }

    private void OnWordClick() 
    {
        isSelected = !isSelected;
        GetComponentInParent<ListWordScroll>()?.RemoveWord(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (isSelected)
            {
                CreatorEvent.onBonusWord?.Invoke(word);
            }
            else
            {
                isSelected = !isSelected;
                GetComponentInParent<ListWordScroll>()?.RemoveWord(gameObject);
                CreatorEvent.onBonusWord?.Invoke(word);
            }
        }
    }
}


#endif