using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PortraitMenu : MonoBehaviour {

    public CharacterAsset asset;
    private PlayerPortraitVisual portrait;
    private float InitialScale;
    private float TargetScale = 1.3f;
    private bool selected = false;

    void Awake()
    {
        portrait = GetComponent<PlayerPortraitVisual>();
        portrait.charAsset = asset;
        portrait.ApplyLookFromAsset();
        InitialScale = transform.localScale.x;
    }

    void OnMouseDown()
    {
        // show the animation
        if (!selected)
        {
            selected = true;
            transform.DOScale(TargetScale, 0.5f);
            CharacterSelectionScreen.Instance.HeroPanel.SelectCharacter(this);
            // deselect all the other Portrait Menu buttons 
            PortraitMenu[] allPortraitButtons = GameObject.FindObjectsOfType<PortraitMenu>();
            foreach (PortraitMenu m in allPortraitButtons)
                if (m != this)
                    m.Deselect();
        }
        else
        {
            Deselect();
            CharacterSelectionScreen.Instance.HeroPanel.SelectCharacter(null);
        }
    }

    public void Deselect()
    {
        transform.DOScale(InitialScale, 0.5f);
        selected = false;
    }
}
