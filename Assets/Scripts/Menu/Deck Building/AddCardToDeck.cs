using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AddCardToDeck : MonoBehaviour {

    public Text QuantityText;
    private float InitialScale;
    private float scaleFactor = 1.1f;
    private CardAsset cardAsset;

    void Awake()
    {
        InitialScale = transform.localScale.x;
    }

    public void SetCardAsset(CardAsset asset) { cardAsset = asset; } 

    void OnMouseDown()
    {
        CardAsset asset = GetComponent<OneCardManager>().cardAsset;
        if (asset == null)
            return;

        // check that these cards are available in collection (Quantity>0) or (TotalQuantity-AmountAlreadyInDeck)>0
        if (CardCollection.Instance.QuantityOfEachCard[cardAsset] - DeckBuildingScreen.Instance.BuilderScript.NumberOfThisCardInDeck(cardAsset) > 0)
        {
            DeckBuildingScreen.Instance.BuilderScript.AddCard(asset);
            UpdateQuantity();
        }
        else
        {
            // say that you do not have enough cards
        }
    }

    void OnMouseEnter()
    {        
        if (CraftingScreen.Instance.Visible)
            return;

        transform.DOScale(InitialScale*scaleFactor, 0.5f);
    }

    void OnMouseExit()
    {
        // if you remove / comment out this if statement, when the crefting screen is pened, when the cursor exits the card it will return to original scale.
        // if (CraftingScreen.Instance.Visible)
            //return;

        transform.DOScale(InitialScale, 0.5f);
    }

    void Update () 
    {
        if(Input.GetMouseButtonDown (1))
            OnRightClick();
    }

    // Check for Right-Click
    void OnRightClick()
    {
        if (CraftingScreen.Instance.Visible)
            return;

        // Cast a ray from the mouse
        // cursors position
        Ray clickPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;

        // See if the ray collided with an object
        if (Physics.Raycast(clickPoint, out hitPoint))
        {
            // Make sure this object was the
            // one that received the right-click
            if (hitPoint.collider == this.GetComponent<Collider>())
            {
                // Put code for the right click event
                Debug.Log("Right Clicked on " + this.name);

                // show craft/disenchant info
                CraftingScreen.Instance.ShowCraftingScreen(GetComponent<OneCardManager>().cardAsset);
            }
        }
    }

    public void UpdateQuantity()
    {
        int quantity = CardCollection.Instance.QuantityOfEachCard[cardAsset];

        if (DeckBuildingScreen.Instance.BuilderScript.InDeckBuildingMode && DeckBuildingScreen.Instance.ShowReducedQuantitiesInDeckBuilding)
            quantity -= DeckBuildingScreen.Instance.BuilderScript.NumberOfThisCardInDeck(cardAsset);
        
        QuantityText.text = "X" + quantity.ToString();

    }
}
