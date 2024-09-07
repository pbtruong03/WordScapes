using TMPro;
using UnityEngine;

public class LevelButton : ButtonBase
{
    public GameObject textLetterPrefabs;
    public GameObject currentCircle;
    public float radius;

    public int levelId;
    public string letters;

    [Header("Component")]
    public TextMeshProUGUI textLevelId;
    public Transform lettersContainer;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        radius = rectTransform.rect.width / 2.8f;
    }

    // Locked Level
    public void SetLevel(int levelId)
    {
        this.levelId = levelId;
        this.letters = "";

        textLevelId.text = levelId.ToString();
        lettersContainer.gameObject.SetActive(false);
        currentCircle.gameObject.SetActive(false);

        button.interactable = false;
    }

    // Unlocked Level
    public void SetLevel(int levelId, string letters)
    {
        this.levelId = levelId;
        this.letters = letters;

        textLevelId.text = levelId.ToString();
        lettersContainer.gameObject.SetActive(true);
        currentCircle.SetActive(false);

        SetLetters();

        button.interactable = true;
    }

    // Current Level
    public void SetCurrentLevel()
    {
        // Curent Level
        currentCircle.SetActive(true);
    }

    private void SetLetters()
    {
        int numLetters = letters.Length;
        float angle = (Mathf.PI * 2) / numLetters;

        while (lettersContainer.childCount < numLetters) {
            Instantiate(textLetterPrefabs, lettersContainer);
        }

        for(int i = 0; i < lettersContainer.childCount; i++)
        {
            var letter = lettersContainer.GetChild(i);
            letter.gameObject.SetActive(false);
        }

        for(int i = 0; i < numLetters; i++)
        {
            var letter = lettersContainer.GetChild(i);
            letter.gameObject.GetComponent<TextMeshProUGUI>().text = letters[i].ToString();
            letter.gameObject.SetActive(true);

            Vector3 position = new Vector3(Mathf.Cos(angle * i + Mathf.PI / 2), Mathf.Sin(angle * i + Mathf.PI / 2), 0) * radius;
            letter.transform.SetLocalPositionAndRotation(position, Quaternion.identity);
        }
    }

    protected override void OnClick()
    {
        base.OnClick(); 

        UIManager.Instance.DisplayGamePlay();
        GameEvent.playLevel?.Invoke(levelId);
    }
}
