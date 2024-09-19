using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public SpriteAtlas m_SpriteAtlas;
    public Image m_Image;
    private int m_CurrentIndex = 0;

    public void ShowLoading() {
        gameObject.SetActive(true);
    }

    public void HideLoading() {
        gameObject.SetActive(false);
    }

    void Start() {

    }

    void Update() {
        m_CurrentIndex++;
        if (m_CurrentIndex > 24) {
            m_CurrentIndex = 1;
        }
        if (m_CurrentIndex % 3 != 0) return;
        m_Image.sprite = m_SpriteAtlas.GetSprite(string.Format("loading_{0}", m_CurrentIndex/3));
    }
}