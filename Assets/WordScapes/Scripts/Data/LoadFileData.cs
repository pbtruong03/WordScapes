
#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;

using UnityEditor;


using UnityEngine;

public class LoadFileData : MonoBehaviour
{
    [Header("Path File")]
    public string fileWordName;
    public string filePackName;

    public ExtraWordData extraWordData;

    void Start()
    {
        LoadWordData();
    }

    public void LoadWordData()
    {
        TextAsset wordfile = Resources.Load<TextAsset>(fileWordName);
        string[] words = wordfile.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        var listWords = new List<string>(words.Select(m=> m.ToUpper()));

        List<string> listRemove = new List<string>();
        listWords.ForEach(word => { if(word.Length < 3) listRemove.Add(word); });

        listRemove.ForEach(word => listWords.Remove(word));

        listWords.Sort();
        extraWordData.listWords = listWords;

        EditorUtility.SetDirty(extraWordData);
    }
}
#endif