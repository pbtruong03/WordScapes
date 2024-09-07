using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeMoveObject
{
    Coin,
    Diamond
}

public class ObjectMoveController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject coinMovePrefabs;
    public GameObject diamondMovePrefabs;

    [Header("Container")]
    public Transform coinMoveContainer;
    public Transform diamondMoveContainer;

    [Header("List Object Spawned")]
    private Queue<GameObject> listCoinMove;
    private Queue<GameObject> listDiamondMove;

    [Header("End ReactTransform")]
    public RectTransform rectCoinTo;
    public RectTransform rectDiamondTo;
    [Space]
    private Vector3 positionFrom;


    [Header("Jump Properties")]
    public float jumpPower;
    public int numJump;
    public float duration;


    public void Start()
    {
        listCoinMove = new Queue<GameObject>();
        listDiamondMove = new Queue<GameObject>();

        for(int i = 0; i < 5; i++)
        {
            var coinObj = Instantiate(coinMovePrefabs, coinMoveContainer);
            coinObj.SetActive(false);
            listCoinMove.Enqueue(coinObj);

            var diamondObj = Instantiate(diamondMovePrefabs, diamondMoveContainer);
            diamondObj.SetActive(false);
            listDiamondMove.Enqueue(diamondObj);
        }
    }

    public void CreateObjectMove(TypeMoveObject typeObj, Vector3 from, bool multiple)
    {
        positionFrom = from;
        StartCoroutine(IESpawner(typeObj, multiple));
    }

    IEnumerator IESpawner(TypeMoveObject typeObj, bool multiple)
    {
        Queue<GameObject> queueObj;
        GameObject prefabs;
        Vector3 positionTo;

        if (typeObj == TypeMoveObject.Coin)
        {
            queueObj = listCoinMove;
            prefabs = coinMovePrefabs;
            positionTo = rectCoinTo.position;
        }
        else
        {
            queueObj = listDiamondMove;
            prefabs = diamondMovePrefabs;
            positionTo = rectDiamondTo.position;
        }

        int numSpawn = multiple ? 5 : 1;
        for(int i = 0; i < numSpawn; i++)
        {
            if (queueObj.Count > 0)
            {
                GameObject go = queueObj.Dequeue();
                go.SetActive(true);
                go.transform.position = positionFrom;
                go.transform.DOJump(positionTo, jumpPower, numJump, duration)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => 
                    {
                        go.SetActive(false);
                        queueObj.Enqueue(go);
                    });
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
