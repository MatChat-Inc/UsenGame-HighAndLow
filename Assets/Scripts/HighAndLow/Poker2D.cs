// Created by LunarEclipse on 2024-5-28 3:23.

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LeTai.TrueShadow;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[ExecuteAlways]
public class Poker2D : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite front;
    public Sprite back;
    [SerializeField] private bool perspective = true;
    
    private Image _image;
    private UIFlippable _flippable;
    private TrueShadow _shadow;
    
    private Camera _mainCamera;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _flippable = GetComponent<UIFlippable>();
        _shadow = GetComponent<TrueShadow>();
    }
    
    private void Start()
    {
        _image.sprite = front;
        _mainCamera = Camera.main;

        UpdateSprite();
    }
    
    private void Update()
    {
        UpdateSprite();

        if (Input.anyKey)
        {
            // transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y - 180f, 0), 2).OnUpdate(()=> {
            //     
            // }).OnComplete(()=> {
            //     
            // });
        }
    }
    
    public TweenerCore<float, float, FloatOptions> PlayShineAnimation(float tensity = 100f, float duration = 1f)
    {
        if (!_shadow.enabled)
            _shadow.enabled = true;
        
        return DOTween.To(() => _shadow.Size, x => _shadow.Size = x, tensity, duration)
            .SetEase(Ease.InSine)
            .OnComplete(()=> {
                _shadow.enabled = false;
                _shadow.Size = 0;
            });
    }
    
    private void UpdateSprite()
    {
        var isBack = perspective ? !IsFacingCamera() : transform.rotation.eulerAngles.y is > 90 and < 270;
        if (isBack)
        {
            _image.sprite = back;
            _flippable.horizontal = true;
        }
        else
        {
            _image.sprite = front;
            _flippable.horizontal = false;
        }
    }

    private bool IsFacingCamera()
    {
        Vector3 toCamera = (_mainCamera.transform.position - transform.position).normalized;
        var dotProduct = Vector3.Dot(transform.forward, toCamera);
        return dotProduct < 0;
    }
    
}
