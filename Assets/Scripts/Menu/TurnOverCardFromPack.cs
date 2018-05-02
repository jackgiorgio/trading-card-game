using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnOverCardFromPack : MonoBehaviour {

    public Image Glow;

    private float InitialScale;
    private float scaleFactor = 1.1f;
    private bool turnedOver = false;
    private OneCardManager manager;

    void Awake()
    {
        InitialScale = transform.localScale.x;
        manager = GetComponent<OneCardManager>();
    }

    void OnMouseDown()
    {
        if (turnedOver)
            return;

        turnedOver = true;        
        // turn the card over
        transform.DORotate(Vector3.zero, 0.5f);
        // add this card to collection as unlocked
        ShopManager.Instance.OpeningArea.NumberOfCardsOpenedFromPack++;
    }

    void OnMouseEnter()
    {
        transform.DOScale(InitialScale*scaleFactor, 0.5f);
        Glow.DOColor(ShopManager.Instance.OpeningArea.GlowColorsByRarity[manager.cardAsset.Rarity], 0.5f);
    }

    void OnMouseExit()
    {
        transform.DOScale(InitialScale, 0.5f);
        Glow.DOColor(Color.clear, 0.5f);
    }
}
