using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.value = 0;
        StartLoading();
    }

    private void StartLoading()
    {
        slider.DOValue(slider.maxValue, 3.5f).SetEase(Ease.InOutCirc);

        DOVirtual.DelayedCall(4.5f, () => { SceneManager.LoadScene(1); });
    }
}
