using System.Collections;
using System.Collections.Generic;
using Modules.Shader;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameSelector : MonoBehaviour, IDragHandler, IDropHandler
{
    public GameEntry[] gameEntries;
    public Button gameSettingsBtn;
    public GameObject gameSettingsGO;
    public ImageTransitionShader background;
    public float spacing = 300;
    public float dragSpeed = 0.5f;
    
    GameObject gameSettingsViewGO;

    private float dragOffset = 0;

    
    [FormerlySerializedAs("selectedIndex")] [SerializeField]
    private int _selectedIndex = 2;
    
    private GameObject _selectedGameObject;

    public int SelectedIndex
    {
        get { return _selectedIndex; }
        set
        {
            _selectedIndex = value;
            UpdateSelectedIndex(_selectedIndex);
        }
    }
    
    void Start()
    {
        for (int i = 0; i < gameEntries.Length; i++)
            gameEntries[i].m_index = i;
        _selectedIndex = Mathf.Clamp(AppConfig.Instance.SelectedGameIndex, 0, gameEntries.Length - 1);
        gameEntries[_selectedIndex].SetEventSystemSelected();
        JumpToIndex(_selectedIndex);
        gameEntries[_selectedIndex].m_selectedBg.alpha = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_selectedIndex > 0)
                AnimatedToIndex(_selectedIndex - 1, 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_selectedIndex < 4)
                AnimatedToIndex(_selectedIndex + 1, 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // selected game settings btn
            EventSystem.current.SetSelectedGameObject(gameSettingsBtn.gameObject);
            _selectedGameObject = gameSettingsBtn.gameObject;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && 
            EventSystem.current.currentSelectedGameObject == gameSettingsBtn.gameObject)
        {
            UpdateSelectedIndex(_selectedIndex);
        }

        if (Input.GetButtonDown("Cancel"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        UpdateEntryPosition();
    }

    public void SelectNextGame()
    {
        if (_selectedIndex < gameEntries.Length - 1)
            AnimatedToIndex(_selectedIndex + 1, 0.3f);
    }
    
    public void SelectPreviousGame()
    {
        if (_selectedIndex > 0)
            AnimatedToIndex(_selectedIndex - 1, 0.3f);
    }
    
    void UpdateSelectedIndex(int index)
    {
        _selectedIndex = index;
        
        if (background != null)
            background.Select(index);

        for (int i = 0; i < gameEntries.Length; i++)
        {
            if (i == _selectedIndex)
            {
                gameEntries[i].SetEventSystemSelected();
                _selectedGameObject = gameEntries[i].gameObject;
            }
            else
            {
                if (i < _selectedIndex)
                    gameEntries[i].SetSiblingIndex(i);
                else gameEntries[i].SetSiblingIndex(gameEntries.Length - i);
            }
        }
    }
    
    void JumpToIndex(int index)
    {
        UpdateSelectedIndex(index);
        dragOffset = -(index - Mathf.Floor(gameEntries.Length / 2f)) * spacing;
        UpdateEntryPosition();
    }
    
    void AnimatedToIndex(int index, float duration = 1f)
    {
        UpdateSelectedIndex(index);
        float targetOffset = -(index - Mathf.Floor(gameEntries.Length / 2f)) * spacing;
        StartCoroutine(AnimateOffset(targetOffset, duration));
    }

    IEnumerator AnimateOffset(float targetOffset, float duration)
    {
        float elapsedTime = 0;
        float startOffset = dragOffset;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            dragOffset = Mathf.Lerp(startOffset, targetOffset, elapsedTime / duration);
            yield return null;
        }

        dragOffset = targetOffset;
    }
    
    public void Show() 
    {
        gameObject.SetActive(true);
        // UpdateSelectedIndex(_selectedIndex);
        EventSystem.current.SetSelectedGameObject(_selectedGameObject);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnSettingsButtonClick()
    {
        Hide();
        if (gameSettingsViewGO == null)
        {
            gameSettingsViewGO = Instantiate(gameSettingsGO);
            gameSettingsViewGO.transform.SetParent(transform.parent, false);
        }
        gameSettingsViewGO.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragOffset += eventData.delta.x * dragSpeed;
    }

    public void OnDrop(PointerEventData eventData)
    {
        AnimatedToIndex(_selectedIndex, 0.3f);
    }
    
    private void UpdateEntryPosition()
    {
        var limit = spacing * Mathf.Floor(gameEntries.Length / 2f);

        dragOffset = Mathf.Clamp(dragOffset, -limit, limit);

        int closestIndex = _selectedIndex;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < gameEntries.Length; i++)
        {
            float newPositionX = (i - Mathf.Floor(gameEntries.Length / 2f)) * spacing + dragOffset;
            newPositionX = Mathf.Pow(Mathf.Abs(newPositionX), 0.6f) * Mathf.Sign(newPositionX) * 15;
            gameEntries[i].SetPositionX(newPositionX);

            float distanceFromCenter = Mathf.Abs(newPositionX);

            if (distanceFromCenter < closestDistance)
            {
                closestDistance = distanceFromCenter;
                closestIndex = i;
            }
        }

        if (closestIndex != _selectedIndex)
        {
            _selectedIndex = closestIndex;
            UpdateSelectedIndex(_selectedIndex);
        }
    }
}
