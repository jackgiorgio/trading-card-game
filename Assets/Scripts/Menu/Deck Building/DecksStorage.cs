using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckInfo
{
    public string DeckName;
    public CharacterAsset Character;
    public List<CardAsset> Cards;

    public DeckInfo(List<CardAsset> cards, string deckName, CharacterAsset charAsset)
    {
        // copy a list, not just use the cards list
        Cards = new List<CardAsset>(cards);
        Character = charAsset;
        DeckName = deckName;
    }

    public bool IsComplete()
    {
        return (DeckBuildingScreen.Instance.BuilderScript.AmountOfCardsInDeck == Cards.Count);
    }

    public int NumberOfThisCardInDeck (CardAsset asset)
    {
        int count = 0;
        foreach (CardAsset ca in Cards)
        {
            if (ca == asset)
                count++;
        }
        return count;
    }
}

public class DecksStorage : MonoBehaviour {

    public static DecksStorage Instance;
    public List<DeckInfo> AllDecks { get; set;}  

    private bool alreadyLoadedDecks = false;

    void Awake()
    {
        AllDecks = new List<DeckInfo>();
        Instance = this;
    }

    void Start()
    {
        if (!alreadyLoadedDecks)
        {
            LoadDecksFromPlayerPrefs();
            alreadyLoadedDecks = true;
        }
    }

    void LoadDecksFromPlayerPrefs()
    {
        List<DeckInfo> DecksFound = new List<DeckInfo>();
        // load the information about decks from PlayerPrefsX
        for(int i=0; i < 9; i++)
        {
            string deckListKey = "Deck" + i.ToString();
            string characterKey = "DeckHero" + i.ToString();
            string deckNameKey = "DeckName" + i.ToString();
            string[] DeckAsCardNames = PlayerPrefsX.GetStringArray(deckListKey);

            Debug.Log("Has character key: "+  PlayerPrefs.HasKey(characterKey));
            Debug.Log("Has Deckname key: "+  PlayerPrefs.HasKey(deckNameKey));
            Debug.Log("Length of DeckAsCardNames: " + DeckAsCardNames.Length);

            if (DeckAsCardNames.Length > 0 && PlayerPrefs.HasKey(characterKey) && PlayerPrefs.HasKey(deckNameKey))
            {
                string characterName = PlayerPrefs.GetString(characterKey);
                string deckName = PlayerPrefs.GetString(deckNameKey);

                // make a CardAsset list from an array of strings:
                List <CardAsset> deckList = new List<CardAsset>();
                foreach(string name in DeckAsCardNames)
                {
                    deckList.Add(CardCollection.Instance.GetCardAssetByName(name));
                }

                DecksFound.Add(new DeckInfo(deckList, deckName, CharacterAssetsByName.Instance.GetCharacterByName(characterName)));
            }
        }

        AllDecks = DecksFound;
    }

    public void SaveDecksIntoPlayerPrefs()
    {
        // clear all the keys of characters and deck names
        for(int i=0; i < 9; i++)
        {
            string characterKey = "DeckHero" + i.ToString();
            string deckNameKey = "DeckName" + i.ToString();
           
            if (PlayerPrefs.HasKey(characterKey))
            {
                PlayerPrefs.DeleteKey(characterKey);
            }

            if(PlayerPrefs.HasKey(deckNameKey))
            {
                PlayerPrefs.DeleteKey(deckNameKey);
            }
        }

        for(int i=0; i< AllDecks.Count; i++)
        {
            string deckListKey = "Deck" + i.ToString();
            string characterKey = "DeckHero" + i.ToString();
            string deckNameKey = "DeckName" + i.ToString();

            List<string> cardNamesList = new List<string>();
            foreach (CardAsset a in AllDecks[i].Cards)
                cardNamesList.Add(a.name);

            string[] cardNamesArray = cardNamesList.ToArray();

            PlayerPrefsX.SetStringArray(deckListKey, cardNamesArray);
            PlayerPrefs.SetString(deckNameKey, AllDecks[i].DeckName);
            PlayerPrefs.SetString(characterKey, AllDecks[i].Character.name);
        }
    }

    void OnApplicationQuit()
    {
        SaveDecksIntoPlayerPrefs();    
    }
}
