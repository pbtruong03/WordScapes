using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    public Button button;
    public GameObject creator;
    public GameObject category;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Switch());
    }
    private void Switch()
    {
        creator.SetActive(!creator.activeSelf);
        category.SetActive(!category.activeSelf);
        if (category.activeSelf)
        {
            CategoryManager.Instance.FindAvailabelIdLevel();
        }
    }
}
