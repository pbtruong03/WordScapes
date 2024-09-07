using System.Collections;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    public GameObject mainMenuUI;
    public GameObject gamePlayUI;
    public GameObject levelSelectUI;

    public ObjectMoveController objMoveCtrl;


    // ---- Game Play ----
    public void DisplayGamePlay()
    {
        CloseAllUI();

        gamePlayUI.SetActive(true);
        GameEvent.inGameplay?.Invoke(true);

        AudioManager.Instance.PlayMusic(AudioType.GamePlayMusic);
    }

    IEnumerator IECLoseGamePlay()
    {
        GameEvent.inGameplay?.Invoke(false);
        yield return new WaitForSeconds(0.15f);
        gamePlayUI.SetActive(false);
    }

    // ---- Main Menu ----
    public void DisplayMainMenu()
    {
        AudioManager.Instance.PlayMusic(AudioType.HomeMusic);
        CloseAllUI();

        mainMenuUI.SetActive(true);
        GameEvent.inMainMenu?.Invoke(true);
    }

    IEnumerator IECloseMainMenu()
    {
        GameEvent.inMainMenu?.Invoke(false);
        yield return new WaitForSeconds(0.2f);
        mainMenuUI.SetActive(false);
    }

    // ---- Level Select ----
    public void DisplayLevelSelect()
    {
        CloseAllUI();

        levelSelectUI.SetActive(true);
        GameEvent.inSelectLevel?.Invoke(true);

        //levelSelectUI.transform.localScale = Vector3.zero;
        //levelSelectUI.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InFlash);
    }

    IEnumerator IECloseLevelSelect()
    {
        GameEvent.inSelectLevel?.Invoke(false);
        yield return new WaitForSeconds(0.2f);
        levelSelectUI.SetActive(false);
    }




    public void CloseAllUI()
    {
        if (gamePlayUI.activeSelf)
        {
            StartCoroutine(IECLoseGamePlay());
        }
        if (levelSelectUI.activeSelf)
        {
            StartCoroutine(IECloseLevelSelect());
        }
        if (mainMenuUI.activeSelf)
        {
            StartCoroutine(IECloseMainMenu());
        }
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
            
    }
}
