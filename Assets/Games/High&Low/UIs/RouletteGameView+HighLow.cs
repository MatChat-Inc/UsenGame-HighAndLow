using Luna;
using Luna.UI.Navigation;
using UnityEngine.SceneManagement;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public partial class RouletteGameView
    {
        private void PopupConfirmView()
        {
            Navigator.ShowModal<PopupOptionsView2>(
                builder: (popup) =>
                {
                    popup.onOption1 = () => Navigator.Pop();
                    popup.onOption2 = () =>
                    {
                        SFXManager.StopAll();
                        Navigator.PopToRoot();
                        //Navigator.PopUntil<RouletteStartView>();
                    }; 
                    popup.onOption3 = () => SceneManager.LoadScene("GameEntries");
                });
        }
    }
}