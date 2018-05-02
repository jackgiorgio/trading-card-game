using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DeckInScrollList : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Image AvatarImage;
    public Text NameText;
    public GameObject DeleteDeckButton;
    public DeckInfo savedDeckInfo;

    public void Awake()
    {
        DeleteDeckButton.SetActive(false);
    }

    public void EditThisDeck()
    {
        // switch collection to editing mode, display the deck list on the right
        // the easiest way is: 
        // 0) hide screen
        DeckBuildingScreen.Instance.HideScreen();
        // 1) make sure that it is for the same character and load the same deck name. 
        DeckBuildingScreen.Instance.BuilderScript.BuildADeckFor(savedDeckInfo.Character);
        DeckBuildingScreen.Instance.BuilderScript.DeckName.text = savedDeckInfo.DeckName;
        // 2) populate it with the same cards that were in this deck.
        foreach (CardAsset asset in savedDeckInfo.Cards)
            DeckBuildingScreen.Instance.BuilderScript.AddCard(asset);
        // 3) delete the deck that we are editing from DecksStorage
        DecksStorage.Instance.AllDecks.Remove(savedDeckInfo);
        // 4) when we press "Done", this deck with changes will be added as a new deck

        // apply character class and activate tab.
        DeckBuildingScreen.Instance.TabsScript.SetClassOnClassTab(savedDeckInfo.Character);
        DeckBuildingScreen.Instance.CollectionBrowserScript.ShowCollectionForDeckBuilding(savedDeckInfo.Character);
        // TODO: save the index of this deck not to make it shift to the end of the list of decks and add it to the same place.

        DeckBuildingScreen.Instance.ShowScreenForDeckBuilding();
    }

    public void DeleteThisDeck()
    {
        // TODO: Display the "Are you sure?" window
        DecksStorage.Instance.AllDecks.Remove(savedDeckInfo);
        Destroy(gameObject);
    }

    public void ApplyInfo (DeckInfo deckInfo)
    {
        AvatarImage.sprite = deckInfo.Character.AvatarImage;
        NameText.text = deckInfo.DeckName;
        savedDeckInfo = deckInfo;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // show delete deck button
        DeleteDeckButton.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        // hide delete deck button
        DeleteDeckButton.SetActive(false);
    }
}
