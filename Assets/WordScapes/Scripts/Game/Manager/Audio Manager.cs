using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;


public enum AudioType
{
    HomeMusic,
    GamePlayMusic,

    ClickSoundFx,
    AddCharSoundFx,
    RemoveCharSoundFx,

    CorrectWordFx,
    WrongWordFx,
    BonusWordFx,
    ExistWordFx,

    ConvertBoosterFx,
    IdeaBoosterFx,
    RocketBoosterFx,
    PointBoosterFx,

    EarnCoinFx,
    VisibleCell,
    LevelComplete
}


public class AudioManager : SingletonBase<AudioManager>
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSource;


    public AudioList[] audioLists;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.mute = !DataManager.musicOn;
        sfxSource.mute = !DataManager.soundOn;

        PlayMusic(AudioType.HomeMusic);
    }

    public void PlayMusic(AudioType audioType)
    {
        if(musicSource.mute) return;

        AudioClip[] clips = audioLists[(int)audioType].Audios;
        AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];

        StartCoroutine(IEChangeMusic(clip));
    }

    IEnumerator IEChangeMusic(AudioClip clip)
    {
        yield return musicSource.DOFade(0.1f, 0.6f).WaitForCompletion();

        musicSource.clip = clip;
        musicSource.Play();

        musicSource.DOFade(0.5f, 0.6f);
    }

    public void PlaySFX(AudioType audioType)
    {
        AudioClip[] clips = audioLists[(int)audioType].Audios;

        sfxSource.PlayOneShot(clips[0]);
    }

    public void PlaySFX(AudioType audioType, int index)
    {
        AudioClip[] clips = audioLists[(int)audioType].Audios;

        sfxSource.PlayOneShot(clips[index]);
    }

    private void Reset()
    {
        string[] names = Enum.GetNames(typeof(AudioType));
        Array.Resize(ref audioLists, names.Length);

        for (int i = 0; i < names.Length; i++)
        {
            audioLists[i].name = names[i];
        }
    }


    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }
}

[Serializable]
public struct AudioList
{
    public AudioClip[] Audios { get => audios; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] audios;
}
