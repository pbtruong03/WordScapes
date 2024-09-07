using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : Component
{
    protected static T _instance;
    public static T Instance {  get { return _instance; } }

    protected virtual void Awake()
    {

        if(_instance != null)
        {
            if(_instance != this)
            {
                Destroy(this);
            }
        } else
        {
            _instance = FindObjectOfType<T>();
        }
    }
}
