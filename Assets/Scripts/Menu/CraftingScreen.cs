using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RarityTradingCost
{
    public RarityOptions Rarity;
    public int CraftCost;
    public int DisenchantOutcome;
}

public class CraftingScreen : MonoBehaviour {
  
    public static CraftingScreen Instance;

    public GameObject Content;

    public GameObject CreatureCard;
    public GameObject SpellCard;

    public Text CraftText;
    public Text DisenchantText;
    public Text QuantityText;

    public RarityTradingCost[] TradingCostsArray;

    public bool Visible{get{ return Content.activeInHierarchy;}}

    private CardAsset currentCard;
    private Dictionary <RarityOptions, RarityTradingCost> TradingCosts = new Dictionary<RarityOptions, RarityTradingCost>();

    void Awake()
    {
        Instance = this;
        foreach (RarityTradingCost cost in TradingCostsArray)
            TradingCosts.Add(cost.Rarity, cost);
    }

    public void ShowCraftingScreen(CardAsset cardToShow)
    {
        currentCard = cardToShow;

        // select type of card to show on this screen - creature or spell
        GameObject cardObject;
        if (currentCard.TypeOfCard == TypesOfCards.Creature)
        {
            cardObject = CreatureCard;
            CreatureCard.SetActive(true);
            SpellCard.SetActive(false);
        }
        else
        {
            cardObject = SpellCard;
            CreatureCard.SetActive(false);
            SpellCard.SetActive(true);
        }
        // change the look of the card to the card that we selected 
        OneCardManager manager = cardObject.GetComponent<OneCardManager>();
        manager.cardAsset = cardToShow;
        manager.ReadCardFromAsset();

        // change the text on buttons
        CraftText.text = "Craft this card for " + TradingCosts[cardToShow.Rarity].CraftCost.ToString() + " dust";
        DisenchantText.text = "Disenchant to get " + TradingCosts[cardToShow.Rarity].DisenchantOutcome.ToString() + " dust";

        ShopManager.Instance.DustHUD.SetActive(true);
        // make sure that correct amount of cards is shown
        UpdateQuantityOfCurrentCard();
        // show the content of this screen
        Content.SetActive(true);
    }

    public void UpdateQuantityOfCurrentCard()
    {
        // get amount from collection
        int AmountOfThisCardInYourCollection = CardCollection.Instance.QuantityOfEachCard[currentCard];
        QuantityText.text = "You have " + AmountOfThisCardInYourCollection.ToString() + " of these";
        // reload the page to keep the quantity updated in the background
        DeckBuildingScreen.Instance.CollectionBrowserScript.UpdatePage();
    }

    public void HideCraftingScreen()
    {
        ShopManager.Instance.DustHUD.SetActive(false);
        
        Content.SetActive(false);
    }

    public void CraftCurrentCard()
    {
        if (currentCard.Rarity != RarityOptions.Basic)
        {
            if (ShopManager.Instance.Dust >= TradingCosts[currentCard.Rarity].CraftCost)
            {
                ShopManager.Instance.Dust -= TradingCosts[currentCard.Rarity].CraftCost;
                CardCollection.Instance.QuantityOfEachCard[currentCard]++;
                UpdateQuantityOfCurrentCard();
            }
        }
        else
        {
            // TODO: show that basic cards can not be crafted or disable crafting buttons for them in advanvce
        }
    }

    public void DisenchantCurrentCard()
    {
        if (currentCard.Rarity != RarityOptions.Basic)
        {
            if (CardCollection.Instance.QuantityOfEachCard[currentCard] > 0)
            {
                CardCollection.Instance.QuantityOfEachCard[currentCard]--;
                ShopManager.Instance.Dust += TradingCosts[currentCard.Rarity].DisenchantOutcome;
                UpdateQuantityOfCurrentCard();

                // check if any of the decks in the collection are now lacking cards because we have disenchanted this card. 
                foreach(DeckInfo info in DecksStorage.Instance.AllDecks)
                {
                    while (info.NumberOfThisCardInDeck(currentCard) > CardCollection.Instance.QuantityOfEachCard[currentCard])
                    {
                        info.Cards.Remove(currentCard);
                    }
                }

                // if we are currently editing a deck and it contains a card that we just disenchanted
                while (DeckBuildingScreen.Instance.BuilderScript.InDeckBuildingMode &&
                       DeckBuildingScreen.Instance.BuilderScript.NumberOfThisCardInDeck(currentCard) > CardCollection.Instance.QuantityOfEachCard[currentCard])
                {
                    // remove the card from the deck.
                    DeckBuildingScreen.Instance.BuilderScript.RemoveCard(currentCard);
                }
            }
        }
        else
        {
            // TODO: show that basic cards can not be disenchanted or disable crafting buttons for them in advanvce
        }
    }
}
