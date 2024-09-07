using DG.Tweening;
using UnityEngine;

public enum CellState
{
    hidden,
    visible,
    solved
}

public class GridCell : Cell
{
    [Header("Components")]
    public int indexCell;
    public SpriteRenderer squarePink;
    public SpriteRenderer squareWhite;
    public GameObject squarePress;
    public SpriteRenderer bonusIcon;

    public bool isBonusCell;

    public TMPro.TextMeshPro letterUI;
    public CellState state;

    public Color pinkColor;
    public Color VisibleColor;


    public BoxCollider2D colider;

    public void SetLetter(string _letter, int indexCell, bool isBonus)
    {
        squarePink.DOFade(1, 0);
        squareWhite.DOFade(1, 0);
        letterUI.DOFade(1, 0);
        bonusIcon.DOFade(1, 0);

        this.indexCell = indexCell;
        letterUI.text = _letter;
        isBonusCell = isBonus;
        bonusIcon.gameObject.SetActive(isBonus);
        OnHidden();
    }

    public void OnHidden()
    {
        state = CellState.hidden;
        squarePink.gameObject.SetActive(false);
        squareWhite.gameObject.SetActive(true);
        letterUI.gameObject.SetActive(false);
    }

    public void OnVisible()
    {
        state = CellState.visible;
        letterUI.DOColor(VisibleColor, 0.1f);
        letterUI.gameObject.SetActive(true);

        AudioManager.Instance.PlaySFX(AudioType.VisibleCell);

        EarnBonus();
    }

    public void OnSolved()
    {
        state = CellState.solved;
        squarePink.gameObject.SetActive(true);
        squareWhite.gameObject.SetActive(false);
        if(!letterUI.gameObject.activeSelf)
            letterUI.gameObject.SetActive(true);

        EarnBonus();

        letterUI.DOColor(Color.white, 0.1f);
        this.gameObject.transform.localScale = Vector3.zero;
        this.gameObject.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
    }

    private void EarnBonus()
    {
        if (isBonusCell)
        {
            UIManager.Instance.objMoveCtrl.CreateObjectMove(TypeMoveObject.Coin, transform.position, false);

            DataManager.EarnCoin(1);

            isBonusCell = false;
            bonusIcon.gameObject.SetActive(false);
        }
    }

    public void OnMouseDown()
    {
        GameEvent.onPointerHint?.Invoke(false);
        GameEvent.visibleCellIndex?.Invoke(indexCell);
        DataManager.Instance.SpentPointBooster();

        AudioManager.Instance.PlaySFX(AudioType.PointBoosterFx);
    }

    public void ReadyToPoint(bool value)
    {
        if(state == CellState.hidden)
        {
            squarePress.SetActive(value);
            colider.enabled = value;
        }
    }

    private void OnEnableUI(bool isEnable)
    {
        if (isEnable)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.4f);
        }
    }

    public void DisableGamePlay(float w)
    {
        float endValue = 1 - w * 2.2f;
        squarePink.DOFade(endValue, 0.2f);
        squareWhite.DOFade(endValue, 0.2f);
        letterUI.DOFade(endValue, 0.2f);
        bonusIcon.DOFade(endValue, 0.2f);

        transform.DOMoveY(transform.position.y - w * 4f, 0.2f);
    }

    private void OnEnable()
    {
        GameEvent.onPointerHint += ReadyToPoint;
        GameEvent.inGameplay += OnEnableUI;
    }
    private void OnDisable()
    {
        GameEvent.onPointerHint -= ReadyToPoint;
        GameEvent.inGameplay -= OnEnableUI;
    }
}
