using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LeTai;
using LeTai.TrueShadow;
using Luna.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(TrueShadow))]
public class OuterGlowButton : AudioButton
{
    public TrueShadow outerGlow;
    public float outglowDuration = 0.2f;
    public bool outglowOnNormal = false;
    public bool outglowOnHovered = true;
    public bool outglowOnPressed = true;
    public bool outglowOnSelected = true;
    public bool outglowOnDisabled = false;
    
    public bool focusOnEnable = false;

    private bool _isGlowing = false;
    
    protected override void Start()
    {
        base.Start();
        
        if (outerGlow == null) 
            outerGlow = GetComponent<TrueShadow>();

        outerGlow.Color = outerGlow.Color.WithA(0);
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        if (focusOnEnable) 
            EventSystem.current.SetSelectedGameObject(gameObject);
    }


    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        
        Debug.Log("DoStateTransition: " + state);

        switch (state)
        {
            case SelectionState.Normal:
                SetGlowState(outglowOnNormal);
                break;
            case SelectionState.Highlighted:
                SetGlowState(outglowOnHovered);
                break;
            case SelectionState.Pressed:
                SetGlowState(outglowOnPressed);
                break;
            case SelectionState.Selected:
                SetGlowState(outglowOnSelected);
                break;
            case SelectionState.Disabled:
                SetGlowState(outglowOnDisabled);
                break;
        }
            
    }
    
    private void SetGlowState(bool glow, bool animated = true)
    {
        if (glow == _isGlowing) return;
        
        if (glow) Glow();
        else StopGlow();
    }
    
    private void Glow(bool animated = true)
    {
        if (outerGlow == null) return;
        
        _isGlowing = true;
        
        outerGlow.enabled = true;

        // Fade in
        if (animated)
            DOTween.To(() => outerGlow.Color.WithA(0), 
                    x => outerGlow.Color = x, 
                    outerGlow.Color.WithA(1), 
                    outglowDuration)
                .SetEase(Ease.InOutSine);
        else outerGlow.Color = outerGlow.Color.WithA(1);
    }
    
    private void StopGlow(bool animated = true)
    {
        if (outerGlow == null) return;
        
        _isGlowing = false;
        
        // Fade out
        if (animated)
            DOTween.To(() => outerGlow.Color.WithA(1), 
                    x => outerGlow.Color = x, 
                    outerGlow.Color.WithA(0), 
                    outglowDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => outerGlow.enabled = false);
        else outerGlow.Color = outerGlow.Color.WithA(0);
    }
    
    
}
