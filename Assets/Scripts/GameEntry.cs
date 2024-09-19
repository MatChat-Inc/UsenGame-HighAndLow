using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEntry : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] private Button m_entryButton;
    public CanvasGroup m_selectedBg;
    public int m_index;
    
    private Action<GameEntry> m_gameEntryDelegate;
    bool m_isWillQuitApplication;
    
    void Start()
    {
        m_entryButton.onClick.AddListener(OnGameEntryClicked);
    }

    void Update()
    {
        
    }

    void OnGUI()
    {

    }

    void OnGameEntrySelected(BaseEventData data)
    {
        // m_gameEntryDelegate(this);
        m_selectedBg.DOFade(1f, 0.2f).SetEase(Ease.InOutSine).SetLink(m_selectedBg.gameObject);
    }

    void OnGameEntryDeselected(BaseEventData data)
    {
        m_selectedBg.DOFade(0f, 0.2f).SetEase(Ease.InOutSine).SetLink(m_selectedBg.gameObject);
    }

    void OnGameEntryClicked()
    {
        // AudioManager.Instance.PlayEntrySelectedEffect();
        
        switch (m_index)
        {
            case 0:
                AppConfig.Instance.SelectedGameIndex = (int)RootViewType.Yamanotesen;
                SceneManager.LoadScene("Yamanotesen");
                break;
            case 1:
                AppConfig.Instance.SelectedGameIndex = (int)RootViewType.HighAndLow;
                AppConfig.Instance.HomeSceneRootViewType = RootViewType.HighAndLow;
                USENSceneManager.Instance.LoadScene("High&Low");
                break;
            case 2:
                LoadGame(RootViewType.Bingo);
                break;
            case 4:
                AppConfig.Instance.SelectedGameIndex = (int)RootViewType.Roulette;
                SceneManager.LoadScene("Roulette");
                break;
        }
    }
    
    private void LoadGame(RootViewType viewType)
    {
        AppConfig.Instance.SelectedGameIndex = (int)viewType;
        AppConfig.Instance.HomeSceneRootViewType = viewType;
        USENSceneManager.Instance.LoadScene("Home");
    }

    public void AddSelectListener(Action<GameEntry> d) 
    {
        m_gameEntryDelegate = d;
    }

    public void SetEventSystemSelected()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void ResetRotate() {
        (transform as RectTransform).pivot = new Vector2(0.5f, 0.5f);
        (transform as RectTransform).rotation = Quaternion.identity;
    }

    public void SetPositionX(float x) {
        (transform as RectTransform).localPosition = new Vector3(x, 0, 0);
    }

    public void SetPivot(Vector2 pivot) {
        (transform as RectTransform).pivot = pivot;
    }

    public void SetRotate(float rotateY) {
        (transform as RectTransform).DOLocalRotate(new Vector3(0, rotateY, 0), 0.5f).SetLink(gameObject);
    }

    public void SetSiblingIndex(int index) {
        transform.SetSiblingIndex(index);
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_selectedBg.DOFade(1f, 0.2f).SetEase(Ease.InOutSine).SetLink(m_selectedBg.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        m_selectedBg.DOFade(0f, 0.2f).SetEase(Ease.InOutSine).SetLink(m_selectedBg.gameObject);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        // OnGameEntryClicked();
    }
}
