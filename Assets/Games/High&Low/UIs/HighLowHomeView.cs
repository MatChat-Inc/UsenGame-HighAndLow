using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Luna;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine.Audio;
using USEN.Games.Roulette;

namespace USEN.Games.HighLow
{
    public class HighLowHomeView : Widget
    {
        public AudioMixerGroup bgmMixer; 
        public AudioMixerGroup sfxMixer;
        
        private void OnEnable()
        {
            RouletteManager.Instance.Sync();
            
            // Audio volume
            BgmManager.Volume = AppConfig.Instance.BGMVolume * 0.1f;
            BgmManager.Mixer = bgmMixer;
            SFXManager.Volume = AppConfig.Instance.EffectVolume * 0.1f;
            SFXManager.Mixer = sfxMixer;
            
            API.GetRandomSetting().ContinueWith(task => {
                RoulettePreferences.DisplayMode = (RouletteDisplayMode) task.Result.random;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void Start()
        {
            // Show loading indicator before necessary assets are loaded
            await UniTask.Yield(PlayerLoopTiming.PreLateUpdate);
            Navigator.ShowModal<RoundedCircularLoadingIndicator>();
            
            var bgm = await R.Audios.BgmHighLow.Load();
            BgmManager.Play(bgm);
            
            await Assets.Load(GetType().Namespace, "Audio");
            await Assets.Load("USEN.Games.Roulette", "Audio");
            
            Navigator.PopToRoot();
        }
    }
}