using UnityEngine;
using System.Collections;

class ToastView : AbstractView, IViewOperater
{
    string m_prefabPath = "Toast";
    GameObject m_viewGameObject;

    float m_timeInterval = 0;

    void InstantiateGameObject() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetPopupTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;
    }

    public void Show() {
        if (m_viewGameObject == null) {
            InstantiateGameObject();
        }

        if (m_viewGameObject.activeSelf) return;
        m_viewGameObject.SetActive(true);
    }

    public void Hide() {
        m_viewGameObject.SetActive(false);
    }

    public bool IsToasting() {
        if (m_viewGameObject == null) return false;
        return m_viewGameObject.activeSelf;
    }

    public void Build()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        if (m_viewGameObject.activeSelf) {
            m_timeInterval += Time.deltaTime;
            if (m_timeInterval > 2f) {
                Hide();
            }
        }else {
            m_timeInterval = 0;
        }
    }

    public void OnThemeTypeChanged()
    {
        throw new System.NotImplementedException();
    }

    public void OnAndroidKeyDown(string keyName)
    {
        throw new System.NotImplementedException();
    }
}