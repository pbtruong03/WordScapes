using DG.Tweening;
using UnityEngine;

public abstract class UIPopupBase : MonoBehaviour
{
    public virtual void OnEnablePopup()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnDisablePopup()
    {

        DOVirtual.DelayedCall(0.5f, () => { gameObject.SetActive(false); });
    }
}
