using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Resources;
using USEN.Games.Common;

public interface IViewOperater
{
    void Show();
    void Build();
    void Hide();
    void Update();
    void Destroy();
    void OnThemeTypeChanged();
    void OnAndroidKeyDown(string keyName);
}

public class ViewManager : MonoBehaviourSingletonTemplate<ViewManager>
{
    private IViewOperater m_lastPopedView;
    private IViewOperater m_currentView;
    private Stack<IViewOperater> m_viewStack;
    private Transform m_rootCanvas;
    private Transform m_popupCanvas;
    private List<IViewOperater> m_allViewInstances = new();

    public Text m_keydownDebug;
    GameObject m_currentEventGO;
    public Image m_bg;
    public Image m_bgDecorate;
    public Loading m_Loading;
    private IViewOperater m_lastView;
    
    public GameObject PreviousSelectedGO => m_currentEventGO;

    public IViewOperater GetCurrentView() 
    {
        return m_currentView;
    }

    public IViewOperater GetLastPopedView()
    {
        return m_lastPopedView;
    }

    public Transform GetRootTransform()
    {
        return m_rootCanvas;
    }

    public Transform GetPopupTransform()
    {
        return m_popupCanvas;
    }

    async void Start()
    {
        Application.targetFrameRate = 30;
        AppConfig.Instance.ThemeSelectedIdx = AppConfig.Instance.ThemeSelectedIdx;
        m_viewStack = new Stack<IViewOperater>();
        m_rootCanvas = GameObject.FindGameObjectWithTag("Modal").transform;
        m_popupCanvas = GameObject.FindGameObjectWithTag("Popup").transform;
        var rootView = AppConfig.Instance.GetSceneRootViewType();
        Push(rootView);

        m_bg.sprite = await ThemeResManager.Instance.GetThemeBgTexture();
        m_bgDecorate.sprite = await ThemeResManager.Instance.GetThemeHomeBgDecorateTexture();
    }

    void Update()
    {
        // ThemeResManager.Instance.CheckAssetsLoaded();
        if (Input.anyKeyDown) {
            AudioManager.Instance.PlayKeyDownEffect();
            if (Input.GetKeyDown(KeyCode.T))
            {
                OnAndroidKeyDown("terminal");
            }
        }
        m_currentView.Update();
        OutputKeyDebugMsg();
    }

    void OutputKeyDebugMsg() {
        if (Input.anyKeyDown) {
            foreach (KeyCode CurretkeyCode in Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(CurretkeyCode))
				{
                    ShowDebugInfo($"[{CurretkeyCode.ToString()}] pressed!");
				}
			}
        }
    }

    public void ShowDebugInfo(string text) {
        m_keydownDebug.text = text;
    }

    public void Push(IViewOperater view)
    {
        view.Build();
        if (!m_allViewInstances.Contains(view)) {
            m_allViewInstances.Add(view);
        }
        if (m_viewStack.Count > 0)
        {
            m_lastView = m_viewStack.Peek();
            if (m_lastView != null) {
                m_lastView.Hide();
                StartCoroutine(DestroyLastPopView());
            }
        }

        if (!m_viewStack.Contains(view))
            m_viewStack.Push(view);
        m_currentView = view;
        m_currentView.Show();
    }

    IEnumerator<WaitForEndOfFrame> DestroyLastPopView()
    {
        yield return new WaitForEndOfFrame();
        m_lastView.Destroy();
    }

    public void Hided(IViewOperater view) 
    {
        m_lastPopedView = m_viewStack.Pop();
        if (m_viewStack.Count == 0) {
            USENSceneManager.Instance.LoadScene("GameEntries");
            return;
        }
        m_currentView = m_viewStack.Peek();
        if (m_currentView != null) {
            m_currentView.Build();
            m_currentView.Show();
            if (m_currentEventGO != null)
            {
                EventSystem.current.SetSelectedGameObject(m_currentEventGO);
            }
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        ShowDebugInfo($"Android key:{keyName}");
        // Debug.Log($"Call from android key:{keyName}");
        m_currentView.OnAndroidKeyDown(keyName);

        if (keyName == "terminal")
        {
            m_currentEventGO = EventSystem.current.currentSelectedGameObject;
            Push(TerminalView.Instance);
        }
        
        switch (keyName)
        {
            case "red":
                UsenEvents.OnRemoconRedButtonClicked.Invoke(this, EventArgs.Empty);
                break;
            case "blue":
                UsenEvents.OnRemoconBlueButtonClicked.Invoke(this, EventArgs.Empty);
                break;
            case "green":
                UsenEvents.OnRemoconGreenButtonClicked.Invoke(this, EventArgs.Empty);
                break;
            case "yellow":
                UsenEvents.OnRemoconYellowButtonClicked.Invoke(this, EventArgs.Empty);
                break;
            case "terminal":
                UsenEvents.OnRemoconTerminalButtonClicked.Invoke(this, EventArgs.Empty);
                break;
        }
    }


    public void OnAudioFocusChanged(string eventType) {
        // Debug.LogWarning(eventType);
        // ShowDebugInfo(eventType);
    }

    public async void SendChangeThemeTypeEvent() {
        m_bg.sprite = await ThemeResManager.Instance.GetThemeBgTexture();
        m_bgDecorate.sprite = await ThemeResManager.Instance.GetThemeHomeBgDecorateTexture();
        foreach (var view in m_allViewInstances)
        {
            view.OnThemeTypeChanged();
        }
    }

    public void ShowLoading() {
        m_Loading.ShowLoading();
    }

    public void HideLoading() {
        m_Loading.HideLoading();
    }

    public bool IsLoadingShow() {
        return m_Loading.gameObject.activeSelf;
    }
}
