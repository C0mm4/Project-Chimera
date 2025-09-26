using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string masterVolumeParam = "MasterVolume";
    [SerializeField] private string bgmVolumeParam = "BGMVolume";
    [SerializeField] private string sfxVolumeParam = "SFXVolume";

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;


    private readonly string MixerPath = "AudioMixer";

    public enum VolumeType
    {
        Master, Music, SFX
    }

    public float masterVolume { get; private set; }
    public float bgmVolume { get; private set; }
    public float sfxVolume { get; private set; }    
    private float SetMasterVolume(float volume) => masterVolume = Mathf.Log10(Mathf.Clamp01(volume)) * 20f;
    private float SetBGMVolume(float volume) => bgmVolume = Mathf.Log10(Mathf.Clamp01(volume)) * 20f;
    private float SetSFXVolume(float volume) => sfxVolume = Mathf.Log10(Mathf.Clamp01(volume)) * 20f;


    protected void Awake()
    {
        audioMixer = ResourceManager.Instance.Load<AudioMixer>(MixerPath);

        ObjectPoolManager.Instance.CreatePool("Pref_SFX", 10, transform);
    }

    public void SetVolume(float volume, VolumeType type = VolumeType.Master)
    {
        switch (type)
        {
            case VolumeType.Master:
                SetMasterVolume(volume);
                break;
            case VolumeType.Music:
                SetBGMVolume(volume);
                break;
            case VolumeType.SFX:
                SetSFXVolume(volume);
                break;
        }

        audioMixer.SetFloat(masterVolumeParam, masterVolume);
        audioMixer.SetFloat(bgmVolumeParam, bgmVolume);
        audioMixer.SetFloat(sfxVolumeParam, sfxVolume);

    }

    #region BGM

    public void PlayBGM(string path, bool loop = true, float fadeDuration = 1f)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        PlayBGM(clip, loop, fadeDuration);
    }

    public void PlayBGM(AudioClip newClip, bool loop = true, float fadeDuration = 1f)
    {
        if (bgmSource == null)
        {
            // 이 부분먼 Resource Manager 사용하게 변경하면 될거 같은데
            bgmSource = ResourceManager.Instance.Create<GameObject>("Pref_BGM").GetorAddComponent<AudioSource>();
        }

        if (bgmSource.clip == newClip) return;

        StopAllCoroutines();
        StartCoroutine(FadeAndSwitchBGM(newClip, loop, fadeDuration));
    }

    private IEnumerator FadeAndSwitchBGM(AudioClip newClip, bool loop, float fadeDuration)
    {
        float currentVolume;
        audioMixer.GetFloat(bgmVolumeParam, out currentVolume);
        currentVolume = Mathf.Pow(10, currentVolume / 20f); 

        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            float v = Mathf.Lerp(currentVolume, 0f, t / fadeDuration);
            audioMixer.SetFloat(bgmVolumeParam, Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);
            yield return null;
        }

        audioMixer.SetFloat(bgmVolumeParam, -80f); 


        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.loop = loop;
        bgmSource.Play();


        float targetDesbel = Mathf.Pow(10, bgmVolume / 20);
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            float v = Mathf.Lerp(0f, targetDesbel, t / fadeDuration);
            audioMixer.SetFloat(bgmVolumeParam, Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);
            yield return null;
        }

        audioMixer.SetFloat(bgmVolumeParam, bgmVolume); 
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }


    #endregion

    #region SFX

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        AudioSource source = GetAvailableSFXSource();
        source.clip = clip;
        source.volume = volume;
        source.Play();
        StartCoroutine(DisableWhenDone(source));
    }


    private AudioSource GetAvailableSFXSource()
    {
        GameObject obj = ObjectPoolManager.Instance.GetPool("Pref_SFX");

        return obj.GetComponent<AudioSource>();

/*
        if(sfxQueue.Count > 0)
        {
            var ret = sfxQueue.Dequeue();
            ret.gameObject.SetActive(true);
            return ret;
        }

        if (sfxSourcePrefab == null)
        {
            sfxSourcePrefab = Resources.Load<GameObject>(SFXPrefPath);
        }
        var newSource = Instantiate(sfxSourcePrefab, transform);
        return newSource.GetComponent<AudioSource>();*/
    }

    private IEnumerator DisableWhenDone(AudioSource src)
    {
        yield return new WaitUntil(() => !src.isPlaying);

        src.clip = null;

        ObjectPoolManager.Instance.ResivePool("Pref_SFX", src.gameObject);
/*
        if(sfxQueue.Count <= sfxPoolCount)
        {
            sfxQueue.Enqueue(src);
            src.gameObject.SetActive(false);
        }
        else
        { 
            Destroy(src.gameObject);
        }*/
    }


    #endregion
}
