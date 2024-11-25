using System.Threading.Tasks;
using Luna.UI;
using USEN.Games.Roulette;

namespace USEN.Games.HighLow
{
    public class HighLowHomeView : Widget
    {
        private void OnEnable()
        {
            API.GetRandomSetting().ContinueWith(task => {
                RoulettePreferences.DisplayMode = (RouletteDisplayMode) task.Result.random;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}