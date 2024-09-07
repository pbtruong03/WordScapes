using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected RectTransform rectTransform;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClick());
    }
    protected void Reset()
    {
        this.LoadComponents();
    }
    protected virtual void LoadComponents()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
    }
    protected virtual void OnClick()
    {
        AudioManager.Instance.PlaySFX(AudioType.ClickSoundFx);
    }
    protected virtual void OnEnableButton(bool enable) { }

}
