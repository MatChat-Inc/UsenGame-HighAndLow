using System.Threading.Tasks;
using Luna;
using Luna.UI;
using USEN.Games.Roulette;

namespace USEN.Games.HighLow
{
    public class HighLowHomeView : Widget
    {
        private void OnEnable()
        {
            RouletteManager.Instance.Sync();
            
            // Audio volume
            BgmManager.Volume = AppConfig.Instance.BGMVolume * 0.1f;
            SFXManager.Volume = AppConfig.Instance.EffectVolume * 0.1f;
            
            API.GetRandomSetting().ContinueWith(task => {
                RoulettePreferences.DisplayMode = (RouletteDisplayMode) task.Result.random;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}