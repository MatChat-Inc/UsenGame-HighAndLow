using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GameSettingsView : MonoBehaviour
{
    Slider m_bgmSlider;
    ToggleGroup m_bgmToggleGroup;
    List<Toggle> m_bgmToggles = new List<Toggle>();
    Slider m_BGMVolumeSlider;
    Slider m_EffectVolumeSlider;
    Text m_BGMVolumeText;
    Text m_EffectVolumeText;
    bool m_isPresettingEffectVolume;
    GameSelector m_gameSelector;
    bool m_isInited;


    Slider m_backgroundToggleSlider;
    ToggleGroup m_backgroundToggleGroup;
    List<Toggle> m_backgroundToggles = new List<Toggle>();


    Button m_maxCellSettingButton;
    Text m_maxCellSettingText;
    float m_deltaTime = 0;

    Button m_confirmSettingCellCountBtn;
    Button m_cancelSettingCellCountBtn;
    int m_nextCellCountChange;
    GameObject m_confirmPanel;

    Button m_highAndLowTimerBtn;
    Text m_highAndLowTimerText;

    Button m_backButton;


    public void Start() {
        m_gameSelector = transform.parent.Find("Home").GetComponent<GameSelector>();

        m_bgmToggleGroup = transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings").GetComponent<ToggleGroup>();
        m_bgmSlider = transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings").GetComponent<Slider>();
        m_bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        m_bgmToggles.Clear();
        for (int i = 1; i <= 2; i++) {
            var toggle = transform.Find("Panel/ViewPort/CenterPanel/BgmSettings/Settings/BGM"+i).GetComponent<Toggle>();
            m_bgmToggles.Add(toggle);
        }
        m_bgmSlider.value = AppConfig.Instance.BgmSelectedIdx;


        m_BGMVolumeSlider = transform.Find("Panel/ViewPort/CenterPanel/BGMVolumeSettings/Slider").GetComponent<Slider>();
        m_BGMVolumeText = transform.Find("Panel/ViewPort/CenterPanel/BGMVolumeSettings/VolumeText").GetComponent<Text>();
        m_BGMVolumeSlider.onValueChanged.AddListener(OnBgmVolumeSliderValueChanged);
        m_BGMVolumeSlider.value = AppConfig.Instance.BGMVolume;

        m_EffectVolumeSlider = transform.Find("Panel/ViewPort/CenterPanel/VolumeEffectSettings/Slider").GetComponent<Slider>();
        m_EffectVolumeText = transform.Find("Panel/ViewPort/CenterPanel/VolumeEffectSettings/VolumeText").GetComponent<Text>();
        m_EffectVolumeSlider.onValueChanged.AddListener(OnVolumeEffectValueChanged);
        m_isPresettingEffectVolume = true;
        m_EffectVolumeSlider.value = AppConfig.Instance.EffectVolume;
        m_isPresettingEffectVolume = false;

        m_backgroundToggleGroup = transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings").GetComponent<ToggleGroup>();
        m_backgroundToggleSlider = transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings").GetComponent<Slider>();
        m_backgroundToggleSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);
        m_backgroundToggles.Clear();
        for (int i = 1; i <= 2; i++) {
            var toggle = transform.Find("Panel/ViewPort/CenterPanel/BackgroundSettings/Settings/Backgound"+i).GetComponent<Toggle>();
            m_backgroundToggles.Add(toggle);
        }
        m_backgroundToggleSlider.value = AppConfig.Instance.ThemeSelectedIdx;

        m_maxCellSettingButton = transform.Find("Panel/ViewPort/CenterPanel/MaxCellSetting/MaxCellCount").GetComponent<Button>();
        m_maxCellSettingText = transform.Find("Panel/ViewPort/CenterPanel/MaxCellSetting/MaxCellCount/Count").GetComponent<Text>();
        m_maxCellSettingText.text = AppConfig.Instance.MaxCellCount.ToString();

        m_confirmPanel = transform.Find("ConfirmPanel").gameObject;
        m_confirmSettingCellCountBtn = m_confirmPanel.transform.Find("ConfirmBtn").GetComponent<Button>();
        m_confirmSettingCellCountBtn.onClick.AddListener(OnClickConfirmSettingCellCountBtn);

        m_cancelSettingCellCountBtn = m_confirmPanel.transform.Find("CancelBtn").GetComponent<Button>();
        m_cancelSettingCellCountBtn.onClick.AddListener(OnClickCancelSettingCellCountBtn);

        m_highAndLowTimerBtn = transform.Find("Panel/ViewPort/CenterPanel/Timer/TimerSettings").GetComponent<Button>();
        m_highAndLowTimerText = transform.Find("Panel/ViewPort/CenterPanel/Timer/TimerSettings/Count").GetComponent<Text>();
        m_highAndLowTimerText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();

        m_backButton = transform.Find("Panel/BottomPanel/BackButton").GetComponent<Button>();
        m_backButton.onClick.AddListener(OnClickBackButton);

        EventSystem.current.SetSelectedGameObject(m_bgmSlider.gameObject);
        m_isInited = true;
    }

    void OnEnable() {
        if (m_isInited) {
            EventSystem.current.SetSelectedGameObject(m_bgmSlider.gameObject);
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
        m_gameSelector.Show();
    }

    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            OnClickBackButton();
        }

        if (Input.GetButtonUp("Horizontal")) {
            m_deltaTime = 0;
        }

        if (Input.GetButtonDown("Horizontal") && EventSystem.current.currentSelectedGameObject == m_maxCellSettingButton.gameObject) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                if (AppConfig.Instance.HasHistoryGameData()) {
                    m_confirmPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(m_confirmSettingCellCountBtn.gameObject);
                    m_nextCellCountChange = -1;
                } else{
                    OnMaxCellSettingSliderValueChanged(-1);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                if (AppConfig.Instance.HasHistoryGameData()) {
                    m_confirmPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(m_confirmSettingCellCountBtn.gameObject);
                    m_nextCellCountChange = +1;
                }else{
                    OnMaxCellSettingSliderValueChanged(+1);
                }
            }
        }

        if (Input.GetButtonDown("Horizontal") && EventSystem.current.currentSelectedGameObject == m_highAndLowTimerBtn.gameObject) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                OnHighAndLowTimerValueChanged(-10);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                OnHighAndLowTimerValueChanged(+10);
            }
        }

        if (Input.GetButton("Horizontal") && EventSystem.current.currentSelectedGameObject == m_maxCellSettingButton.gameObject) {
            m_deltaTime += Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow) && m_deltaTime > 1) {
                OnMaxCellSettingSliderValueChanged(-1);
            }
            if (Input.GetKey(KeyCode.RightArrow) && m_deltaTime > 1) {
                OnMaxCellSettingSliderValueChanged(+1);
            }
        }
    }

    private void OnBackgroundSliderChanged(float value) {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.ThemeSelectedIdx = intValue;
        m_backgroundToggles[AppConfig.Instance.ThemeSelectedIdx].isOn = true;
    }

    private void OnBgmSliderChanged(float value) {
        var intValue = Mathf.FloorToInt(value);
        AppConfig.Instance.BgmSelectedIdx = intValue;
        m_bgmToggles[AppConfig.Instance.BgmSelectedIdx].isOn = true;
    }

    public void OnBgmVolumeSliderValueChanged(float value) {
        m_BGMVolumeText.text = string.Format("{0}%", value * 10);
        AppConfig.Instance.BGMVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetBgmVolume((int)value);
    }

    public void OnVolumeEffectValueChanged(float value) {
        m_EffectVolumeText.text = string.Format("{0}%", value * 10);
        AppConfig.Instance.EffectVolume = Mathf.FloorToInt(value);
        AudioManager.Instance.SetEffectVolume((int)value);
        if (m_isPresettingEffectVolume) return;
        AudioManager.Instance.PlayNumberRotateEffectWithoutLoop();
        AudioManager.Instance.PlayNumberCheckEffect();
    }

    
    // 确认修改最大cell数
    void OnClickConfirmSettingCellCountBtn() {
        m_confirmPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_maxCellSettingButton.gameObject);
        OnMaxCellSettingSliderValueChanged(m_nextCellCountChange);
    }

    void OnClickCancelSettingCellCountBtn() {
        m_confirmPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_maxCellSettingButton.gameObject);
    }

    public void OnMaxCellSettingSliderValueChanged(int value) {
        if (value < 0 && AppConfig.Instance.MaxCellCount == 8) return;
        if (value > 0 && AppConfig.Instance.MaxCellCount == 75) return;

        AppConfig.Instance.MaxCellCount += value;
        m_maxCellSettingText.text = AppConfig.Instance.MaxCellCount.ToString();
        AppConfig.Instance.ClearGameData();
    }

    void OnHighAndLowTimerValueChanged(int value) {
        if (value < 0 && AppConfig.Instance.CurrentHighAndLowTimer == 10) return;
        if (value > 0 && AppConfig.Instance.CurrentHighAndLowTimer == 30) return;

        AppConfig.Instance.CurrentHighAndLowTimer += value;
        m_highAndLowTimerText.text = AppConfig.Instance.CurrentHighAndLowTimer.ToString();
    }

    void OnClickBackButton() {
        EventSystem.current.SetSelectedGameObject(null);
        AudioManager.Instance.PlayKeyBackEffect();
        Hide();
    }
}
