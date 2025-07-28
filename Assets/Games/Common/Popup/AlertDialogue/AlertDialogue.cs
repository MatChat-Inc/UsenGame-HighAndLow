
using System;
using Luna.UI;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlertDialogue: Widget
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI confirmText;
    public TextMeshProUGUI cancelText;
    public Button confirmButton;
    public Button cancelButton;
    
    public Action onConfirm;
    public Action onCancel;

    public string Title
    {
        get => titleText.text;
        set => titleText.text = value;
    }
    
    public string Content
    {
        get => contentText.text;
        set => contentText.text = value;
    }
    
    protected void Awake()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
        EventSystem.current.SetSelectedGameObject(confirmButton.gameObject);
    }

    protected void OnConfirm()
    {
        onConfirm?.Invoke();
    }
    
    protected void OnCancel()
    {
        onCancel?.Invoke();
        Navigator.Pop();
    }
}
