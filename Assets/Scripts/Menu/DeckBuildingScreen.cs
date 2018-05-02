using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildingScreen : MonoBehaviour {

    public GameObject ScreenContent;
    public GameObject ReadyDecksList;
    public GameObject CardsInDeckList;
    public DeckBuilder BuilderScript;
    public ListOfDecksInCollection ListOfReadyMadeDecksScript;
    public CollectionBrowser CollectionBrowserScript;
    public CharacterSelectionTabs TabsScript;
    public bool ShowReducedQuantitiesInDeckBuilding = true;

    public static DeckBuildingScreen Instance;

    // Use this for initialization
    void Awake () 
    {
        Instance = this;    
        HideScreen();
    }   

    public void ShowScreenForCollectionBrowsing()
    {
        ScreenContent.SetActive(true);
        ReadyDecksList.SetActive(true);
        CardsInDeckList.SetActive(false);
        BuilderScript.InDeckBuildingMode = false;
        ListOfReadyMadeDecksScript.UpdateList();

        CollectionBrowserScript.AllCharactersTabs.gameObject.SetActive(true);
        CollectionBrowserScript.OneCharacterTabs.gameObject.SetActive(false);
        Canvas.ForceUpdateCanvases();

        CollectionBrowserScript.ShowCollectionForBrowsing();
    } 

    public void ShowScreenForDeckBuilding()
    {
        ScreenContent.SetActive(true);
        ReadyDecksList.SetActive(false);
        CardsInDeckList.SetActive(true);

        CollectionBrowserScript.AllCharactersTabs.gameObject.SetActive(false);
        CollectionBrowserScript.OneCharacterTabs.gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
        // TODO: update the tab to say the name of the character class that we are building a deck for, update the script on the tab.
    }

    public void BuildADeckFor(CharacterAsset asset)
    {
        ShowScreenForDeckBuilding();
        CollectionBrowserScript.ShowCollectionForDeckBuilding(asset);
        DeckBuildingScreen.Instance.TabsScript.SetClassOnClassTab(asset);
        BuilderScript.BuildADeckFor(asset);
    }

    public void HideScreen()
    {
        ScreenContent.SetActive(false);
        CollectionBrowserScript.ClearCreatedCards();
    }
}
