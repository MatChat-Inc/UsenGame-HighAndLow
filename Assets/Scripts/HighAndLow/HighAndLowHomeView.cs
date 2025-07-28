using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Luna;
using Luna.UI.Navigation;
using USEN.Games.HighLow;

public class HighAndLowHomeView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowHomePanel";
    Button m_startBtn;
    Button m_settingsBtn;
    Button _exitButton;
    HighAndLowGameView m_gameView;
    HighAndLowSettingsView m_settingsView;

    GameObject _backgrond;
    
    GameObject m_resetPanel;
    Button m_resetBtn;
    Button m_cancelBtn;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());
        
        Navigator.Create(m_mainViewGameObject);

        m_startBtn = m_mainViewGameObject.transform.Find("StartBtn").GetComponent<Button>();
        m_startBtn.onClick.AddListener(OnClickStartButton);

        m_settingsBtn = m_mainViewGameObject.transform.Find("SettingsBtn").GetComponent<Button>();
        m_settingsBtn.onClick.AddListener(OnClickSettingsButton);

        m_resetPanel = m_mainViewGameObject.transform.Find("ResetPanel").gameObject;
        m_resetBtn = m_mainViewGameObject.transform.Find("ResetPanel/ResetBtn").GetComponent<Button>();
        m_resetBtn.onClick.AddListener(OnClickResetButton);
        m_cancelBtn = m_mainViewGameObject.transform.Find("ResetPanel/CancelBtn").GetComponent<Button>();
        m_cancelBtn.onClick.AddListener(OnClickCancelButton);
        
        _backgrond = m_mainViewGameObject.transform.Find("Bg").gameObject;
        
        _exitButton = m_mainViewGameObject.transform.Find("BottomPanel/ExitButton").GetComponent<Button>();
        _exitButton.onClick.AddListener(() => {
#if UNITY_ANDROID
            // Android.Back();
            Application.Quit();
#else
            Application.Quit();
#endif
        });
        
        AudioManager.Instance.SetBgmVolume(AppConfig.Instance.BGMVolume);
        AudioManager.Instance.SetEffectVolume(AppConfig.Instance.EffectVolume);
    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        
    }

    public void OnThemeTypeChanged()
    {
        
    }

    public void Show()
    {
        m_mainViewGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_startBtn.gameObject);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            // if (m_mainViewGameObject.activeInHierarchy)
            //     USENSceneManager.Instance.LoadScene("GameEntries");
#if UNITY_ANDROID
            if (m_mainViewGameObject.activeInHierarchy)
                Application.Quit();
                // Android.Back();
#endif
        }
    }

    public void OnClickStartButton()
    {
        if (AppConfig.Instance.CheckedPokers.Count > 0) {
            ShowResetAlertView();
            return;
        }

        if (m_gameView == null) {
            m_gameView = new HighAndLowGameView();
        }
        ViewManager.Instance.Push(m_gameView);
    }

    public async void OnClickSettingsButton()
    {
        await Navigator.Push<HighLowSettingsView>();
        EventSystem.current.SetSelectedGameObject(m_settingsBtn.gameObject);
    }

    void ShowResetAlertView() {
        m_resetPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_resetBtn.gameObject);
    }

    void HideResetAlertView() {
        m_resetPanel.SetActive(false);
        // EventSystem.current.SetSelectedGameObject(m_startBtn.gameObject);
    }

    void OnClickResetButton() {
        HideResetAlertView();
        AppConfig.Instance.CheckedPokers = new List<int>() { };
        OnClickStartButton();
        SFXManager.Play(R.Audios.SfxConfirm);
    }

    void OnClickCancelButton() {
        HideResetAlertView();
        if (m_gameView == null) 
            m_gameView = new HighAndLowGameView();
        ViewManager.Instance.Push(m_gameView);
        SFXManager.Play(R.Audios.SfxConfirm);
    }
}