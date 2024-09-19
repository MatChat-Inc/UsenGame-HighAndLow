using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSingletonTemplate<AudioManager>
{
    public AudioSource audioSource;

    public AudioSource bingoEffectAudioSource;
    public AudioSource effectAudioSource;

    public AudioSource keydownAudioSource;
    public AudioSource numberRotateAudioSource;

    private bool fadeIn = false;
    private bool fadeOut = false;
    private float fadeCostTime = 0f;
    private float fadeInterval = 0f;
    public bool m_isWillReach = false;

    public static void InitVolume() 
    {
        var bgmVolume = AppConfig.Instance.BGMVolume;
        AudioManager.Instance.SetBgmVolume(bgmVolume);
        var effectVolume = AppConfig.Instance.EffectVolume;
        AudioManager.Instance.SetEffectVolume(effectVolume);
    }

    void Update()
    {
        if (fadeIn)
        {
            fadeInterval += Time.deltaTime;
            if (fadeInterval >= fadeCostTime)
            {
                fadeInterval = 0;
                audioSource.volume = 1;
                fadeIn = false;
            }else
            {
                audioSource.volume = fadeInterval / fadeCostTime;
            }
        }
        else if (fadeOut)
        {
            fadeInterval += Time.deltaTime;
            if (fadeInterval >= fadeCostTime)
            {
                fadeInterval = 0;
                audioSource.volume = 0;
                fadeOut = false;
                PauseBgm();
            }else
            {
                audioSource.volume =  1 - fadeInterval / fadeCostTime;
            }
        }
    }

    public void SetBgmVolume(int volume)
    {
        audioSource.volume = volume * 0.1f;
    }

    public async void PlayWillReachBgm(float delay = 0f) {
        m_isWillReach = true;
        audioSource.clip = await ThemeResManager.Instance.GetWillReachBgmAudioClip();
        PlayBgm(delay);
    }

    public async void PlayDefaultBgm(float delay = 0f) {
        m_isWillReach = false;
        audioSource.clip = await ThemeResManager.Instance.GetBgmAudioClip();
        PlayBgm(delay);
    }

    public void StopBgm() {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    public void PauseBgm()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    public void UnPauseBgm()
    {
        if (!audioSource.isPlaying)
            audioSource.UnPause();
    }

    public void PlayBgm(float delay = 0f) {
        if (!audioSource.isPlaying)
            audioSource.PlayDelayed(delay);
    }


    public void SetEffectVolume(int volume)
    {
        bingoEffectAudioSource.volume = volume * 0.1f;
        effectAudioSource.volume = volume * 0.1f;
        numberRotateAudioSource.volume = volume * 0.1f;
        keydownAudioSource.volume = volume * 0.1f;
    }

    public async void PlayBingoEffect() {
        fadeCostTime = 1.0f;
        fadeOut = true;
        bingoEffectAudioSource.clip = await ThemeResManager.Instance.GetBingoAudioClip();

        StartCoroutine(DelayPlayBingoEffect());
    }

    IEnumerator DelayPlayBingoEffect()
    {
        yield return new WaitForSeconds(fadeCostTime);
        bingoEffectAudioSource.Play();
        StartCoroutine(ResumePlayBgm());
    }

    IEnumerator ResumePlayBgm()
    {
        yield return new WaitForSeconds(3.0f);
        UnPauseBgm();
        fadeIn = true;
    }

    public async void PlayNumberCheckEffect() {
        numberRotateAudioSource.clip = await ThemeResManager.Instance.GetNumberCheckDownAudioClip();
        numberRotateAudioSource.loop = false;
        numberRotateAudioSource.Play();
    }

    public async void PlayNumberRotateEffect() {
        numberRotateAudioSource.clip = await ThemeResManager.Instance.GetNumberRotateAudioClip();
        numberRotateAudioSource.loop = true;
        numberRotateAudioSource.Play();
    }

    public async void PlayNumberRotateEffectWithoutLoop() {
        numberRotateAudioSource.clip = await ThemeResManager.Instance.GetNumberRotateAudioClip();
        numberRotateAudioSource.loop = false;
        numberRotateAudioSource.Play();
    }

    public void StopNumberRotateEffect() {
        numberRotateAudioSource.Stop();
    }

    public async void PlayReachClickEffect() {
        bingoEffectAudioSource.clip = await ThemeResManager.Instance.GetReachAudioClip();
        bingoEffectAudioSource.loop = false;
        bingoEffectAudioSource.Play();
    }


    public async void PlayEntrySelectedEffect() {
        keydownAudioSource.clip = await AudioResManager.Instance.GetKeySelectedAudioPath();
        keydownAudioSource.loop = false;
        keydownAudioSource.Play();
    }

    public async void PlayLowAndHighBGM() {
        audioSource.clip = await AudioResManager.Instance.GetHighAndLowBGMAudioPath();
        PlayBgm();
    }

    public async void PlayKeyBackEffect() {
        keydownAudioSource.clip = await AudioResManager.Instance.GetKeyBackAudioPath();
        keydownAudioSource.loop = false;
        keydownAudioSource.Play();
    }

    public async void PlayKeyStartEffect() {
        keydownAudioSource.clip = await AudioResManager.Instance.GetKeyStartAudioPath();
        keydownAudioSource.loop = false;
        keydownAudioSource.Play();
    }

    public async void PlayHighEffect() {
        effectAudioSource.clip = await AudioResManager.Instance.GetHighAudioPath();
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public async void PlayLowEffect() {
        effectAudioSource.clip = await AudioResManager.Instance.GetLowAudioPath();
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public async void PlayHighAndLowTimerEffect() {
        effectAudioSource.clip = await AudioResManager.Instance.GetTimerAudioPath();
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public async void Play10SecondTimerEffect() {
        effectAudioSource.clip = await AudioResManager.Instance.Get10SecondsAudioPath();
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public async void Play20SecondTimerEffect() {
        effectAudioSource.clip = await AudioResManager.Instance.Get20SecondsAudioPath();
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public async void Play30SecondTimerEffect() {
        effectAudioSource.clip = await AudioResManager.Instance.Get30SecondsAudioPath();
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public void StopEffectAudio() {
        effectAudioSource.Stop();
    }

    public async void PlaySendPokerEffect() {
        effectAudioSource.clip = await AudioResManager.Instance.GetSendPokerAudioPath();
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public async void PlayTimerStartEffect() {
        keydownAudioSource.clip = await AudioResManager.Instance.GetTimerStartAudioPath();
        keydownAudioSource.loop = false;
        keydownAudioSource.Play();
    }

    public void PlayKeyDownEffect() {
        keydownAudioSource.Play();
    }

    public void SendChangeThemeTypeEvent() {
        if (m_isWillReach) {
            PlayWillReachBgm();
        }else {
            PlayDefaultBgm();
        }
    }

    public async void PlayFinishEffect() {
        numberRotateAudioSource.clip = await AudioResManager.Instance.GetFinishAudioPath();
        numberRotateAudioSource.loop = false;
        numberRotateAudioSource.Play();
    }
}
