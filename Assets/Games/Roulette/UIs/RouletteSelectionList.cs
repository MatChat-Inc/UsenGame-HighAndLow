using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RouletteSelectionList : MonoBehaviour, IEventSystemHandler
{
    protected ScrollRect listView;

    void Awake()
    {
        listView = GetComponent<ScrollRect>();
    }

    async Task OnEnable()
    {
        // Select first cell
        var firstCell = listView.content.GetChild(0).gameObject;
        if (firstCell != null)
        {
            await UniTask.NextFrame();
            EventSystem.current.SetSelectedGameObject(firstCell);
        }
    }

    public void SnapTo(RectTransform target)
    {
        var y = -target.anchoredPosition.y - ((RectTransform)listView.transform).sizeDelta.y;
        y = Mathf.Clamp(y, 0, listView.content.sizeDelta.y);
        var pos = new Vector2(listView.content.anchoredPosition.x, y);
        DOTween.To(() => listView.content.anchoredPosition, v => listView.content.anchoredPosition = v, pos, 0.5f);
    }
}
