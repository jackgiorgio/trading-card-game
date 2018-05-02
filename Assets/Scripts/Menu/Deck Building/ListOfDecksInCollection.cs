using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfDecksInCollection : MonoBehaviour {

    public Transform Content;
    // this is a prefab
    public GameObject DeckInListPrefab;
    // this is also a prefab
    public GameObject NewDeckButtonPrefab;

    public void UpdateList()
    {
        // delete all the deck icons first
        foreach (Transform t in Content)
        {
            if (t != Content)
            {
                Destroy(t.gameObject);
            }
        }
        // load the information about decks from DecksStorage
        foreach (DeckInfo info in DecksStorage.Instance.AllDecks)
        {
            // create a new DeckInListPrefab;
            GameObject g = Instantiate(DeckInListPrefab, Content);
            g.transform.localScale = Vector3.one;
            DeckInScrollList deckInScrollListComponent = g.GetComponent<DeckInScrollList>();
            deckInScrollListComponent.ApplyInfo(info);
        }

        // if there is room to create more decks, create a NewDeckButton
        if (DecksStorage.Instance.AllDecks.Count < 9)
        {
            GameObject g = Instantiate(NewDeckButtonPrefab, Content);
            g.transform.localScale = Vector3.one;
        }
    }
        
}
