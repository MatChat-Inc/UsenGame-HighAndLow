using UnityEngine;
using System.Threading.Tasks;

public class AudioResManager
{
    private static AudioResManager m_instance;

    public static AudioResManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new AudioResManager();
            }
            return m_instance;
        }
        set
        {
            m_instance = value;
        }
    }


    public async Task<AudioClip> GetKeySelectedAudioPath() {
        var audioPath = "Audio/entry_key_select";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetHighAndLowBGMAudioPath() {
        var audioPath = "Audio/entry_bgm";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetKeyBackAudioPath() {
        var audioPath = "Audio/key_back";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetKeyStartAudioPath() {
        var audioPath = "Audio/key_start";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetHighAudioPath() {
        var audioPath = "Audio/high";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetLowAudioPath() {
        var audioPath = "Audio/low";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetTimerAudioPath() {
        var audioPath = "Audio/key_timer";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> Get10SecondsAudioPath() {
        var audioPath = "Audio/10sec";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> Get20SecondsAudioPath() {
        var audioPath = "Audio/20sec";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> Get30SecondsAudioPath() {
        var audioPath = "Audio/30sec";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetSendPokerAudioPath() {
        var audioPath = "Audio/send_poker";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetTimerStartAudioPath() {
        var audioPath = "Audio/timer_start";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }

    public async Task<AudioClip> GetFinishAudioPath() {
        var audioPath = "Audio/finish";
        var audioClip = await ResourceLoader.LoadResourcesAsync<AudioClip>(audioPath);
        return audioClip;
    }
}