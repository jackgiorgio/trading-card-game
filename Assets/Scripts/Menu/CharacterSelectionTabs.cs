using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this class should work similarly to the mana filter selection (like a radio button)
// it will handle ALL the tabs, both in deckbuilding and in collectionbrowsing
public class CharacterSelectionTabs : MonoBehaviour 
{
    public List<CharacterFilterTab> Tabs = new List<CharacterFilterTab>();
    public CharacterFilterTab ClassTab;
    public CharacterFilterTab NeutralTabWhenCollectionBrowsing;
    private int currentIndex = 0;

    public void SelectTab(CharacterFilterTab tab, bool instant)
    {
        int newIndex = Tabs.IndexOf(tab);

        if (newIndex == currentIndex)
            return;

        currentIndex = newIndex;

        // we have selected a new tab
        // remove highlights from all the other tabs:
        foreach (CharacterFilterTab t in Tabs)
        {
            if (t != tab)
                t.Deselect(instant);
        }
        // select the tab that we have picked
        tab.Select(instant);
        // update the cards in the collection
        DeckBuildingScreen.Instance.CollectionBrowserScript.Asset = tab.Asset;
        DeckBuildingScreen.Instance.CollectionBrowserScript.IncludeAllCharacters = tab.showAllCharacters;
    }

    public void SetClassOnClassTab(CharacterAsset asset)
    {
        ClassTab.Asset = asset;
        ClassTab.GetComponentInChildren<Text>().text = asset.name;
    }
}
