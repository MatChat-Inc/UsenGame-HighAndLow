using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HighAndLowHistoryView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowHistoryPanel";
    Text m_restCountLabel;
    Button m_backButton;
    List<int> m_checkedPokers;
    HighAndLowGameView m_gameView;

    public HighAndLowHistoryView(List<int> checkedPokers) {
        m_checkedPokers = checkedPokers;
    }

    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

        foreach (var poker in m_checkedPokers)
        {
            var itemPath = string.Format("CheckList/{0}", EPokersHelper.GetTextureNameFromPoker((EPokers)poker));
            m_mainViewGameObject.transform.Find(itemPath).gameObject.GetComponent<Image>().color = Color.white;
        }

        m_restCountLabel = m_mainViewGameObject.transform.Find("RestCountLabel").GetComponent<Text>();
        m_restCountLabel.text = string.Format("{0}", 52 - m_checkedPokers.Count());
        m_backButton = m_mainViewGameObject.transform.Find("BottomPanel/BackBtn").GetComponent<Button>();
        m_backButton.onClick.AddListener(OnClickedBackButton);
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
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            OnClickedBackButton();
        }
    }

    void OnClickedBackButton() {
        if (m_gameView == null)
            m_gameView = new HighAndLowGameView();
        ViewManager.Instance.Push(m_gameView);
    }
}