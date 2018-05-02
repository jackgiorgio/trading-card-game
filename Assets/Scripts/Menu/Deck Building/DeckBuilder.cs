using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour 
{
    public GameObject CardNamePrefab;
    public Transform Content;
    public InputField DeckName;

    public int SameCardLimit = 2;
    public int AmountOfCardsInDeck = 10;

    public GameObject DeckCompleteFrame;

    private List<CardAsset> deckList = new List<CardAsset>();
    private Dictionary<CardAsset, CardNameRibbon> ribbons = new Dictionary<CardAsset, CardNameRibbon>();

    public bool InDeckBuildingMode{ get; set;}
    private CharacterAsset buildingForCharacter;

    void Awake()
    {
        DeckCompleteFrame.GetComponent<Image>().raycastTarget = false;
    }

    public void AddCard(CardAsset asset)
    {
        // if we are browsing the collection
        if (!InDeckBuildingMode)
            return;

        // if the deck is already full 
        if (deckList.Count == AmountOfCardsInDeck)
            return;

        int count = NumberOfThisCardInDeck(asset);

        int limitOfThisCardInDeck = SameCardLimit;

        // if something else is specified in the CardAsset, we use that.
        if (asset.OverrideLimitOfThisCardInDeck > 0)
            limitOfThisCardInDeck = asset.OverrideLimitOfThisCardInDeck;

        if (count < limitOfThisCardInDeck)
        {
            deckList.Add(asset);

            CheckDeckCompleteFrame();

            // added one to count if we are adding this card
            count++;

            // do all the graphical stuff
            if (ribbons.ContainsKey(asset))
            {
                // update quantity 
                ribbons[asset].SetQuantity(count);
            }
            else
            {
                // 1) Add card`s name to the list
                GameObject cardName = Instantiate(CardNamePrefab, Content) as GameObject;
                cardName.transform.SetAsLastSibling();
                cardName.transform.localScale = Vector3.one;
                CardNameRibbon ribbon = cardName.GetComponent<CardNameRibbon>();
                ribbon.ApplyAsset(asset, count);
                ribbons.Add(asset, ribbon);
            }
        }
    }

    void CheckDeckCompleteFrame()
    {
        DeckCompleteFrame.SetActive(deckList.Count == AmountOfCardsInDeck);
    }

    public int NumberOfThisCardInDeck (CardAsset asset)
    {
        int count = 0;
        foreach (CardAsset ca in deckList)
        {
            if (ca == asset)
                count++;
        }
        return count;
    }

    public void RemoveCard(CardAsset asset)
    {
        Debug.Log("InRemoveCard");
        CardNameRibbon ribbonToRemove = ribbons[asset];
        ribbonToRemove.SetQuantity(ribbonToRemove.Quantity-1);

        if (NumberOfThisCardInDeck(asset) == 1)
        {            
            ribbons.Remove(asset);
            Destroy(ribbonToRemove.gameObject);
        }

        deckList.Remove(asset);

        CheckDeckCompleteFrame();

        // update quantities of all cards that we currently show in the collection
        // this should be after deckList.Remove(asset); line to show correct quantities
        DeckBuildingScreen.Instance.CollectionBrowserScript.UpdateQuantitiesOnPage();
    }

    public void BuildADeckFor(CharacterAsset asset)
    {
        InDeckBuildingMode = true;
        buildingForCharacter = asset;
        // TODO: remove all the cards from the deck list if there are any, 
        // both the actual list and visually delete all the card list items
        while (deckList.Count>0)
        {
            RemoveCard(deckList[0]);
        }

        // apply character class and activate tab.
        DeckBuildingScreen.Instance.TabsScript.SetClassOnClassTab(asset);
        DeckBuildingScreen.Instance.CollectionBrowserScript.ShowCollectionForDeckBuilding(asset);

        CheckDeckCompleteFrame();

        // reset the InputField text to be empty
        DeckName.text = "";
    }

    public void DoneButtonHandler()
    {
        // save current deck list, character and deck name into DecksStorage
        DeckInfo deckToSave = new DeckInfo(deckList, DeckName.text, buildingForCharacter);
        DecksStorage.Instance.AllDecks.Add(deckToSave);
        DecksStorage.Instance.SaveDecksIntoPlayerPrefs();
        // the screen with collection and pre-made decks is loaded by calling other methods on this button
        DeckBuildingScreen.Instance.ShowScreenForCollectionBrowsing();
    }

    void OnApplicationQuit()
    {
        // if we exit the app while editing a deck, we want to save it anyway
        DoneButtonHandler();
    }
}
