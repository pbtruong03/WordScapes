using DG.Tweening;
using EnhancedUI.EnhancedScroller;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlickSnapScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public EnhancedScroller scroller;
    public EnhancedScroller.TweenType snapTweenType;

    public float snapTweenTime;
    public int MaxDataElements { get; set; }
    private bool _isDragging = false;

    private Vector2 _dragStartPosition = Vector2.zero;
    private int _currentIndex = -1;

    [Header("UI Component")]
    public TextMeshProUGUI headerWordText;
    public Button prevButton;
    public Button nextButton;


    private void Start()
    {
        prevButton.onClick.AddListener(() => { OnCtrlButtonClick(-1); });
        nextButton.onClick.AddListener(() => { OnCtrlButtonClick(1); });
    }

    private void OnCtrlButtonClick(int increase)
    {
        _currentIndex = scroller.StartDataIndex;
        int jumpToIndex = _currentIndex + increase;

        if (jumpToIndex != -1)
        {
            scroller.JumpToDataIndex(Mathf.Clamp(jumpToIndex, 0, MaxDataElements - 1), tweenType: snapTweenType, tweenTime: snapTweenTime);
            DOVirtual.DelayedCall(snapTweenTime + 0.05f, () => { UpdateUI(); });
        }
    }

    private void UpdateUI()
    {
        if (MaxDataElements == 0)
        {
            prevButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            return;
        }

        _currentIndex = scroller.StartDataIndex;

        MeaningWordCell curCell = scroller.GetCellViewAtDataIndex(_currentIndex) as MeaningWordCell;
        string curWord = curCell.textWord.text.ToUpper();
        headerWordText.text = curWord;

        prevButton.gameObject.SetActive(_currentIndex != 0);
        nextButton.gameObject.SetActive(_currentIndex != MaxDataElements - 1);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (scroller.IsTweening)
        {
            return;
        }

        _isDragging = true;

        _dragStartPosition = data.position;

        _currentIndex = scroller.StartDataIndex;
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (_isDragging)
        {
            _isDragging = false;

            var delta = data.position - _dragStartPosition;
            var jumpToIndex = -1;

            if (delta.x < 0)
            {
                jumpToIndex = _currentIndex + 1;
            }
            else if (delta.x > 0)
            {
                jumpToIndex = _currentIndex - 1;
            }

            if (jumpToIndex != -1)
            {
                scroller.JumpToDataIndex(Mathf.Clamp(jumpToIndex, 0, MaxDataElements - 1), tweenType: snapTweenType, tweenTime: snapTweenTime);
                UpdateUI();
            }
        }
    }

    private void OnEnable()
    {
        GameEvent.displayDictionary += UpdateUI;
    }

    private void OnDisable()
    {
        GameEvent.displayDictionary -= UpdateUI;
    }
}