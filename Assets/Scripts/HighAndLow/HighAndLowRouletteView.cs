using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class HighAndLowRouletteView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowRoulettePanel";
    Button m_startBtn;
    HighAndLowGameView m_gameView;
    List<GameObject> m_rouletteGOs = new List<GameObject>();
    GameObject m_rouletteRotateGO;
    List<GameObject> m_behaviourOptions = new List<GameObject>();
    Button m_backButton;
    Button m_startButton;
    Button m_editButton;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());


            m_rouletteRotateGO = m_mainViewGameObject.transform.Find("Roulette/Selector").gameObject;
        for (int i = 1; i < 5; i++) {
            var go = m_mainViewGameObject.transform.Find("Roulette/"+i).gameObject;
            m_rouletteGOs.Add(go);
            var option = m_mainViewGameObject.transform.Find("Behaviours/"+i+"/SelectedBg").gameObject;
            m_behaviourOptions.Add(option);
        }

        m_backButton = m_mainViewGameObject.transform.Find("BottomPanel/BackBtn").GetComponent<Button>();
        m_backButton.onClick.AddListener(OnClickedBackButton);
        m_startButton = m_mainViewGameObject.transform.Find("BottomPanel/StartBtn").GetComponent<Button>();
        m_startButton.onClick.AddListener(OnClickedStartButton);
        m_editButton = m_mainViewGameObject.transform.Find("BottomPanel/EditBtn").GetComponent<Button>();
        m_editButton.onClick.AddListener(OnClickedEditButton);
    }

    public override void OnDestroy() 
    {
        m_rouletteGOs.Clear();
        m_behaviourOptions.Clear();
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

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit")) {
            OnClickedStartButton();
        }
    }

    public void OnClickedStartButton()
    {
        AudioManager.Instance.PlayKeyStartEffect();
        foreach (var go in m_rouletteGOs)
        {
            go.SetActive(false);
        }
        foreach (var option in m_behaviourOptions)
        {
            option.SetActive(false);
        }
        var idx = Random.Range(1, 4);
        // 1:0 , 2:90 , 3:180 , 4:270
        var rotateAngle = new Vector3(0, 0, (idx - 1) * 90 + 2160);
        m_rouletteRotateGO.transform.DOLocalRotate(rotateAngle, 3, RotateMode.FastBeyond360).SetLink(m_rouletteRotateGO).OnComplete(() => {
            rotateAngle = new Vector3(0, 0, (idx - 1) * 90);
            if (m_rouletteRotateGO) {
                m_rouletteRotateGO.transform.localRotation = Quaternion.Euler(rotateAngle);
            }
            if (m_behaviourOptions.Count > 0) {
                m_behaviourOptions.ElementAt(idx - 1).SetActive(true);
            }
            if (m_rouletteGOs.Count > 0) {
                m_rouletteGOs.ElementAt(idx - 1).SetActive(true);
            }
        });
    }

    void OnClickedBackButton()
    {
        if (m_gameView == null)
            m_gameView = new HighAndLowGameView();
        ViewManager.Instance.Push(m_gameView);
    }

    void OnClickedEditButton() 
    {

    }
}