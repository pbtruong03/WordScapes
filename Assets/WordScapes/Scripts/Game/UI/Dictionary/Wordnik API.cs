using UnityEngine;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

public class WordnikAPI : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();
    [SerializeField] private string apiKey;

    public class WordnikStruct
    {
        public string partOfSpeech;
        public string sourceDictionary;
        public string text;
    }

    private List<WordnikStruct> wiktionaryDic = new List<WordnikStruct>();
    private List<WordnikStruct> ahd5thDic = new List<WordnikStruct>();
    private List<WordnikStruct> centuryDic = new List<WordnikStruct>();
    private List<WordnikStruct> wordnetDic = new List<WordnikStruct>();

    public WordnikStruct[] wordMeans;
    public DictionaryController dictionaryController;

    public string[] elementTags = { "<xref>", "</xref>", "<em>", "</em>", "<strong>", "</strong>" };

    //data for Meaning Word Data
    string meanContent;
    string sourceDic;

    


    public async void findMeanWord(string wordStr)
    {
        string url = $"https://api.wordnik.com/v4/word.json/{wordStr}/definitions?api_key={apiKey}";

        wordMeans = null;
        wiktionaryDic.Clear();
        ahd5thDic.Clear();
        centuryDic.Clear();
        wordnetDic.Clear();

        //data for Meaning Word Data
        meanContent = "";
        sourceDic = "";

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            wordMeans = JsonConvert.DeserializeObject<WordnikStruct[]>(responseBody);

            DictionaryDivision();

            List<WordnikStruct> tmpDic;

            if (wiktionaryDic.Count > 0)
            {
                tmpDic = wiktionaryDic;
                sourceDic = "b\'Wiktionary\'";
            } 
            else if (ahd5thDic.Count > 0)
            {
                tmpDic = ahd5thDic;
                sourceDic = "b\'The American Heritage® Dictionary\'";
            }
            else if (centuryDic.Count > 0)
            {
                tmpDic = centuryDic;
                sourceDic = "b\'Century Dictionary\'";
            }
            else
            {
                tmpDic = wordnetDic;
                sourceDic = "b\'WordNet\'";
            }

            for(int i = 0; i < tmpDic.Count; i++)
            {
                meanContent += $"{i+1}. ({tmpDic[i].partOfSpeech}): {tmpDic[i].text}\n";
            }

            foreach(string tag in elementTags)
            {
                meanContent = meanContent.Replace(tag, "");
            }

            if(meanContent == "")
            {
                meanContent = "Sorry, no definitions found.";
                sourceDic = "";
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        MeaningWordData meaningData = new MeaningWordData(wordStr, meanContent, sourceDic);

        dictionaryController.AddElement(meaningData);

        //return meaningData;
    }

    private void DictionaryDivision()
    {
        for (int i = 0; i < wordMeans.Length; i++)
        {
            WordnikStruct wordMean = wordMeans[i];
            if (wordMean.text == null) continue;

            switch (wordMean.sourceDictionary)
            {
                case "wiktionary":
                    wiktionaryDic.Add(wordMean);
                    break;
                case "ahd-5":
                    ahd5thDic.Add(wordMean);
                    break;
                case "century":
                    centuryDic.Add(wordMean);
                    break;
                case "wordnet":
                    wordnetDic.Add(wordMean);
                    break;
            }
        }
    }

}
