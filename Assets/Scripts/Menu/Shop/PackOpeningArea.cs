using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class PackOpeningArea : MonoBehaviour {

    public bool AllowedToDragAPack{ get; set;}
    public GameObject SpellCardFromPackPrefab;
    public GameObject CreatureCardFromPackPrefab;
    public Button DoneButton;
    public Button BackButton;
    [Header("Probabilities")]
    [Range(0,1)]
    public float LegendaryProbability;
    [Range(0,1)]
    public float EpicProbability;
    [Range(0,1)]
    public float RareProbability;
    // these are the glow colors that will show while opening cards
    // or you can use colors from  RarityColors
    [Header ("Colors")]
    public Color32 LegendaryColor;
    public Color32 EpicColor;
    public Color32 RareColor;
    public Color32 CommonColor;

    public Dictionary<RarityOptions, Color32> GlowColorsByRarity = new Dictionary<RarityOptions, Color32>();

    public bool giveAtLeastOneRare = false;

    public Transform[] SlotsForCards;

    private BoxCollider col;
    private List<GameObject> CardsFromPackCreated = new List<GameObject>();
    private int numOfCardsOpened = 0;
    public int NumberOfCardsOpenedFromPack 
    { 
        get{ return numOfCardsOpened; } 
        set
        { 
            numOfCardsOpened = value;
            if (value == SlotsForCards.Length)
            {
                // activate the Done button
                DoneButton.gameObject.SetActive(true);
            }
        }
    }

    void Awake()
    {
        col = GetComponent<BoxCollider>();
        AllowedToDragAPack = true;

        GlowColorsByRarity.Add(RarityOptions.Common, CommonColor);
        GlowColorsByRarity.Add(RarityOptions.Rare, RareColor);
        GlowColorsByRarity.Add(RarityOptions.Epic, EpicColor);
        GlowColorsByRarity.Add(RarityOptions.Legendary, LegendaryColor);

    }

    public bool CursorOverArea()
    {
        RaycastHit[] hits;
        // raycst to mousePosition and store all the hits in the array
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            // check if the collider that we hit is the collider on this GameObject
            if (h.collider == col)
                passedThroughTableCollider = true;
        }
        return passedThroughTableCollider;
    }

    public void ShowPackOpening(Vector3 cardsInitialPosition)
    {      
        // ShopManager.Instance.PacksCreated--;
        // Allow To Drag Another Pack Only After DoneButton Is pressed
        // 1) Determine rarity of all cards
        RarityOptions[] rarities = new RarityOptions[SlotsForCards.Length];
        bool AtLeastOneRareGiven = false;
        for (int i = 0; i < rarities.Length; i++)
        {
            // determine rarity of this card
            float prob = Random.Range(0f,1f);
            if (prob < LegendaryProbability)
            {
                rarities[i] = RarityOptions.Legendary;
                AtLeastOneRareGiven = true;
            }
            else if (prob < EpicProbability)
            {
                rarities[i] = RarityOptions.Epic;
                AtLeastOneRareGiven = true;
            }
            else if (prob < RareProbability)
            {
                rarities[i] = RarityOptions.Rare;
                AtLeastOneRareGiven = true;
            }
            else
                rarities[i] = RarityOptions.Common;
        }

        if (AtLeastOneRareGiven == false && giveAtLeastOneRare)
        {
            rarities[Random.Range(0, rarities.Length)] = RarityOptions.Rare;
        }

        for (int i = 0; i < rarities.Length; i++)
        {
            GameObject card = CardFromPack(rarities[i]);
            CardsFromPackCreated.Add(card);
            card.transform.position = cardsInitialPosition;
            card.transform.DOMove(SlotsForCards[i].position, 0.5f);
        }
    }

    private GameObject CardFromPack(RarityOptions rarity)
    {   
        List<CardAsset> CardsOfThisRarity = CardCollection.Instance.GetCardsWithRarity(rarity);
        CardAsset a = CardsOfThisRarity[Random.Range(0, CardsOfThisRarity.Count)];

        // add this card to your collection. 
        CardCollection.Instance.QuantityOfEachCard[a]++;

        GameObject card;
        if (a.TypeOfCard == TypesOfCards.Creature)
            card = Instantiate(CreatureCardFromPackPrefab) as GameObject;
        else 
            card = Instantiate(SpellCardFromPackPrefab) as GameObject;

        card.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.cardAsset = a;
        manager.ReadCardFromAsset();
        return card;
    }

    public void Done()
    {
        AllowedToDragAPack = true;
        NumberOfCardsOpenedFromPack = 0;
        while (CardsFromPackCreated.Count > 0)
        {
            GameObject g = CardsFromPackCreated[0];
            CardsFromPackCreated.RemoveAt(0);
            Destroy(g);
        }
        BackButton.interactable = true;
    }
}
