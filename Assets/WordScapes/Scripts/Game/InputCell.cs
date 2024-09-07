using UnityEngine;

public class InputCell : Cell
{
    [Header("Component")]
    public GameObject squarePink;
    public TMPro.TextMeshPro letterUI;

    public Color selectedColor;
    public Color normalColor;

    public string letter;

    public void SetLetter(string _letter)
    {
        letter = _letter;
        letterUI.text = _letter;
    }

    public void SelectedLetter(bool selected)
    {
        squarePink.SetActive(selected);
        letterUI.color = selected ? selectedColor : normalColor;
    }

}
