using UnityEngine;

public class TopUIManager : MonoBehaviour
{
    public GameObject blurLayer;

    public GameObject pointSupport;

    public void OnPointSupport(bool value)
    {
        blurLayer.SetActive(value);
        pointSupport.SetActive(value);
    }

    private void OnEnable()
    {
        GameEvent.onPointerHint += OnPointSupport;
    }
    private void OnDisable()
    {
        GameEvent.onPointerHint -= OnPointSupport;
    }
}
