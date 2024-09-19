using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TerminalView : AbstractView, IViewOperater
{
    string m_prefabPath = "TerminalPanel";
    GameObject m_viewGameObject;

    Button m_confirmBtn;
    Text m_confirmBtnText;
    Button m_cancelBtn;
    Text m_cancelBtnText;
    private static TerminalView m_instance;

    public static TerminalView Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new TerminalView();
            }
            return m_instance;
        }
    }

    public void Build()
    {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;

        m_confirmBtn = m_viewGameObject.transform.Find("ConfirmBtn").GetComponent<Button>();
        m_confirmBtnText = m_viewGameObject.transform.Find("ConfirmBtn/Text").GetComponent<Text>();
        m_confirmBtn.onClick.AddListener(OnClickedConfirmBtn);
        m_cancelBtn = m_viewGameObject.transform.Find("CancelBtn").GetComponent<Button>();
        m_cancelBtnText = m_viewGameObject.transform.Find("CancelBtn/Text").GetComponent<Text>();
        m_cancelBtn.onClick.AddListener(OnClickedCancelBtn);

        var eventTrigger = m_confirmBtn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(OnConfirmButtonSelected);
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.Deselect;
        entry1.callback.AddListener(OnConfirmButtonDeselected);
        eventTrigger.triggers.Add(entry1);

        var eventTrigger2 = m_cancelBtn.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.Select;
        entry2.callback.AddListener(OnCancelButtonSelected);
        eventTrigger2.triggers.Add(entry2);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.Deselect;
        entry3.callback.AddListener(OnCancelButtonDeselected);
        eventTrigger2.triggers.Add(entry3);
    }

    public void Hide()
    {
        m_viewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        
    }

    public void Show()
    {
        m_viewGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_confirmBtn.gameObject);
    }

    public void Update()
    {
        
    }

    void OnClickedConfirmBtn()
    {
        AppConfig.Instance.ClearGameData();
        Application.Quit();
    }

    void OnClickedCancelBtn()
    {
        Hide();
        ViewManager.Instance.Hided(this);
    }

    void OnConfirmButtonSelected(BaseEventData data)
    {
        m_confirmBtnText.color = Color.white;
        m_cancelBtnText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnConfirmButtonDeselected(BaseEventData data)
    {
        m_confirmBtnText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnCancelButtonSelected(BaseEventData data)
    {
        m_cancelBtnText.color = Color.white;
        m_confirmBtnText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    void OnCancelButtonDeselected(BaseEventData data)
    {
        m_cancelBtnText.color = new Color(0f, 147 / 255f, 1.0f, 1.0f);
    }

    public void OnThemeTypeChanged() {

    }
}
