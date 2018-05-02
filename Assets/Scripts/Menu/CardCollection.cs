using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardCollection : MonoBehaviour 
{
    public int DefaultNumberOfBasicCards = 3; // how many cards of basic rarity should a character have by default;

    public static CardCollection Instance;
    private Dictionary<string, CardAsset > AllCardsDictionary = new Dictionary<string, CardAsset>();

    public Dictionary<CardAsset, int> QuantityOfEachCard = new Dictionary<CardAsset, int>();

    private CardAsset[] allCardsArray;

    void Awake()
    {
        Instance = this;

        allCardsArray = Resources.LoadAll<CardAsset>("");
        //Debug.Log(allCardsArray.Length);
        foreach (CardAsset ca in allCardsArray)
        {
            if (!AllCardsDictionary.ContainsKey(ca.name))
                AllCardsDictionary.Add(ca.name, ca);
        }      

        LoadQuantityOfCardsFromPlayerPrefs();
    }

    private void LoadQuantityOfCardsFromPlayerPrefs()
    {
        // TODO: load only cards from the non-basic set. Basic set should always have quantities set to some standard number, not disenchantable 

        foreach (CardAsset ca in allCardsArray)
        {
            // quantity of basic cards should not be affected:
            if(ca.Rarity == RarityOptions.Basic)
                QuantityOfEachCard.Add(ca, DefaultNumberOfBasicCards);            
            else if (PlayerPrefs.HasKey("NumberOf" + ca.name))
                QuantityOfEachCard.Add(ca, PlayerPrefs.GetInt("NumberOf" + ca.name));
            else
                QuantityOfEachCard.Add(ca, 0);
        }
    }

    private void SaveQuantityOfCardsIntoPlayerPrefs()
    {
        foreach (CardAsset ca in allCardsArray)
        {
            if (ca.Rarity == RarityOptions.Basic)
                PlayerPrefs.SetInt("NumberOf" + ca.name, DefaultNumberOfBasicCards);
            else
                PlayerPrefs.SetInt("NumberOf" + ca.name, QuantityOfEachCard[ca]);
        }
    }

    void OnApplicationQuit()
    {
        SaveQuantityOfCardsIntoPlayerPrefs();
    }

    public CardAsset GetCardAssetByName(string name)
    {        
        if (AllCardsDictionary.ContainsKey(name))  // if there is a card with this name, return its CardAsset
            return AllCardsDictionary[name];
        else        // if there is no card with name
            return null;
    }	

    public List<CardAsset> GetCardsOfCharacter(CharacterAsset asset)
    {   
        /*
        // get cards that blong to a particular character or neutral if asset == null
        var cards = from card in allCardsArray
                                    where card.CharacterAsset == asset
                                    select card; 
        
        var returnList = cards.ToList<CardAsset>();
        returnList.Sort();
        */
        return GetCards(true, true, false, RarityOptions.Basic, asset);
    }

    public List<CardAsset> GetCardsWithRarity(RarityOptions rarity)
    {
        /*
        // get neutral cards
        var cards = from card in allCardsArray
                where card.Rarity == rarity
            select card; 

        var returnList = cards.ToList<CardAsset>();
        returnList.Sort();

        return returnList;
        */
        return GetCards(true, false, true, rarity);

    }

    /// the most general method that will use multiple filters
    public List<CardAsset> GetCards(bool showingCardsPlayerDoesNotOwn = false, bool includeAllRarities = true, bool includeAllCharacters = true, RarityOptions rarity = RarityOptions.Basic,
                CharacterAsset asset = null, string keyword = "", int manaCost = -1, bool includeTokenCards = false)
    {
        // initially select all cards
        var cards = from card in allCardsArray select card;

        if (!showingCardsPlayerDoesNotOwn)
            cards = cards.Where(card => QuantityOfEachCard[card] > 0);

        if (!includeTokenCards)
            cards = cards.Where(card => card.TokenCard == false);

        if (!includeAllRarities)
            cards = cards.Where(card => card.Rarity == rarity);

        if (!includeAllCharacters)
            cards = cards.Where(card => card.CharacterAsset == asset);

        if (keyword != null && keyword != "")
            cards = cards.Where(card => (card.name.ToLower().Contains(keyword.ToLower()) || 
                (card.Tags.ToLower().Contains(keyword.ToLower()) && !keyword.ToLower().Contains(" "))));

        if (manaCost == 7)
            cards = cards.Where(card => card.ManaCost >= 7);
        else if (manaCost != -1)
            cards = cards.Where(card => card.ManaCost == manaCost);                
        
        var returnList = cards.ToList<CardAsset>();
        returnList.Sort();

        return returnList;
    }
}
