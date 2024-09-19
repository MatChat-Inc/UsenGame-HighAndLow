using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class HighAndLowSettingsView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowSettingsPanel";
    Slider m_bgmSlider;
    ToggleGroup m_bgmToggleGroup;
    List<Toggle> m_bgmToggles = new List<Toggle>();
    Slider m_BGMVolumeSlider;
    Slider m_EffectVolumeSlider;
    Text m_BGMVolumeText;
    Text m_EffectVolumeText;
    bool m_isPresettingEffectVolume;
    bool m_isInited;

    Slider m_backgroundToggleSlider;
    ToggleGroup m_backgroundToggleGroup;
    List<Toggle> m_backgroundToggles = new List<Toggle>();

    Button m_highAndLowTimerBtn;
    Text m_highAndLowTimerText;

    Button m_backButton;
    HighAndLowHomeView m_homeView;


    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        m_bgmToggleGroup = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings").GetComponent<ToggleGroup>();
        m_bgmSlider = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings").GetComponent<Slider>();
        m_bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        m_bgmToggles.Clear();
        for (int i = 1; i <= 2; i++)
        {
            var toggle = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings/BGM" + i).GetComponent<Toggle>();
            m_bgmToggles.Add(toggle);
        }
        m_bgmSlider.value = AppConfig.Instance.BgmSelectedIdx;


        m_BGMVolumeSlider = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/BGMVolumeSettings/Slider").GetComponent<Slider>();
        m_BGMVolumeText = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/BGMVolumeSettings/VolumeText").GetComponent<Text>();
        m_BGMVolumeSlider.onValueChanged.AddListener(OnBgmVolumeSliderValueChanged);
        m_BGMVolumeSlider.value = AppConfig.Instance.BGMVolume;

        m_EffectVolumeSlider = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/VolumeEffectSettings/Slider").GetComponent<Slider>();
        m_EffectVolumeText = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/VolumeEffectSettings/VolumeText").GetComponent<Text>();
        m_EffectVolumeSlider.onValueChanged.AddListener(OnVolumeEffectValueChanged);
        m_isPresettingEffectVolume = true;
        m_EffectVolumeSlider.value = AppConfig.Instance.EffectVolume;
        m_isPresettingEffectVolume = false;

        m_backgroundToggleGroup = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings").GetComponent<ToggleGroup>();
        m_backgroundToggleSlider = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings").GetComponent<Slider>();
        m_backgroundToggleSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);
        m_backgroundToggles.Clear();
        for (int i = 1; i <= 2; i++)
        {
            var toggle = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings/Backgound" + i).GetComponent<Toggle>();
            m_backgroundToggles.Add(toggle);
        }
        m_backgroundToggleSlider.value = AppConfig.Instance.ThemeSelectedIdx;

        m_highAndLowTimerBtn = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/Timer/TimerSettings").GetComponent<Button>();
        m_highAndLowTimerText = m_mainViewGameObject.transform.Find("Panel/ViewPort/CenterPanel/Timer/TimerSettings/Count").GetComponent<Text>();
        m_highAndLowTimerText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();

        m_backButton = m_mainViewGameObject.transform.Find("Panel/BottomPanel/BackButton").GetComponent<Button>();
        m_backButton.onClick.AddListener(OnClickBackButton);

        EventSystem.current.SetSelectedGameObject(m_bgmSlider.gameObject);
        m_isInited = true;
    }

    void OnEnable()
    {
        if (m_isInited)
        {
            EventSystem.current.SetSelectedGameObject(m_bgmSlider.gameObject);
        }
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
    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            OnClickBackButton();
        }

        if (Input.GetButtonDown("Horizontal") && EventSystem.current.currentSelectedGameObject == m_highAndLowTimerBtn.gameObject)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                OnHighAndLowTimerValueChanged(-10);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                OnHighAndLowTimerValueChanged(+10);
            }
        }
    }

    private void OnBackgroundSliderChanged(float value)
    {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.ThemeSelectedIdx = intValue;
        m_backgroundToggles[AppConfig.Instance.ThemeSelectedIdx].isOn = true;
    }

    private void OnBgmSliderChanged(float value)
    {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.BgmSelectedIdx = intValue;
        m_bgmToggles[AppConfig.Instance.BgmSelectedIdx].isOn = true;
    }

    public void OnBgmVolumeSliderValueChanged(float value)
    {
        m_BGMVolumeText.text = string.Format("{0}%", value * 10);
        AppConfig.Instance.BGMVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetBgmVolume((int)value);
    }

    public void OnVolumeEffectValueChanged(float value)
    {
        m_EffectVolumeText.text = string.Format("{0}%", value * 10);
        AppConfig.Instance.EffectVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetEffectVolume((int)value);
        if (m_isPresettingEffectVolume) return;
        AudioManager.Instance.PlayNumberRotateEffectWithoutLoop();
        AudioManager.Instance.PlayNumberCheckEffect();
    }

    void OnHighAndLowTimerValueChanged(int value)
    {
        if (value < 0 && AppConfig.Instance.CurrentHighAndLowTimer == 10) return;
        if (value > 0 && AppConfig.Instance.CurrentHighAndLowTimer == 30) return;

        AppConfig.Instance.CurrentHighAndLowTimer += value;
        m_highAndLowTimerText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();
    }

    void OnClickBackButton()
    {
        AudioManager.Instance.PlayKeyBackEffect();
        if (m_homeView == null)
        {
            m_homeView = new HighAndLowHomeView();
            m_homeView.Build();
        }
        ViewManager.Instance.Push(m_homeView);
    }
}
