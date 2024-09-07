using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    public GameObject settingsPanel;
    public Image blurImage;

    public void OpenPanel()
    {
        blurImage.DOFade(0.6f, 0.15f);
        settingsPanel.transform.localScale = Vector3.zero;
        settingsPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void DisablePanel()
    {
        blurImage.DOFade(0, 0.15f);
        settingsPanel.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.OutFlash);
        StartCoroutine(WaitDisable());
    }

    IEnumerator WaitDisable()
    {
        yield return new WaitForSeconds(0.15f);
        gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        OpenPanel();
    }
    private void OnDisable()
    {
        
    }
}
