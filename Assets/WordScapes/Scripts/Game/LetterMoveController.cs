using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LetterMoveController : MonoBehaviour
{
    public TextMeshPro textmeshpro;
    public char letter;

    [Header("Jump values")]
    public float jumpPower;
    public int numJump;
    public float duration;

    public void SetLetter(char _letter, Vector3 pos, Vector3 toPos)
    {
        transform.localScale = Vector3.one;
        letter = _letter;
        transform.position = pos;
        textmeshpro.text = letter.ToString();
        gameObject.SetActive(true);

        transform.DOJump(toPos, jumpPower, numJump, duration, false).SetEase(Ease.InOutQuad);
        transform.DOScale(GridBoardManager.Instance.transform.localScale * 3.6f, duration).SetEase(Ease.InOutQuad);

        StartCoroutine("DisableLetter");
    }
    
    IEnumerator DisableLetter()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
