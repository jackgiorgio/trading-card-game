using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CardAsset cardAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text NameText;
    public Text ManaCostText;
    public Text DescriptionText;
    public Text HealthText;
    public Text AttackText;
    [Header("Image References")]
    public Image CardTopRibbonImage;
    public Image CardLowRibbonImage;
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image CardFaceFrameImage;
    public Image CardFaceGlowImage;
    public Image CardBackGlowImage;
    public Image RarityStoneImage;

    void Awake()
    {
        if (cardAsset != null)
            ReadCardFromAsset();
    }

    private bool canBePlayedNow = false;
    public bool CanBePlayedNow
    {
        get
        {
            return canBePlayedNow;
        }

        set
        {
            canBePlayedNow = value;

            CardFaceGlowImage.enabled = value;
        }
    }

    public void ReadCardFromAsset()
    {
        // universal actions for any Card
        // 1) apply tint
        if (cardAsset.CharacterAsset != null)
        {
            CardBodyImage.color = cardAsset.CharacterAsset.ClassCardTint;
            CardFaceFrameImage.color = cardAsset.CharacterAsset.ClassCardTint;
            CardTopRibbonImage.color = cardAsset.CharacterAsset.ClassRibbonsTint;
            CardLowRibbonImage.color = cardAsset.CharacterAsset.ClassRibbonsTint;
        }
        else
        {
            //CardBodyImage.color = GlobalSettings.Instance.CardBodyStandardColor;
            CardFaceFrameImage.color = Color.white;
            //CardTopRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
            //CardLowRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
        }
        // 2) add card name
        NameText.text = cardAsset.name;
        // 3) add mana cost
        ManaCostText.text = cardAsset.ManaCost.ToString();
        // 4) add description
        DescriptionText.text = cardAsset.Description;
        // 5) Change the card graphic sprite
        CardGraphicImage.sprite = cardAsset.CardImage;

        if (cardAsset.TypeOfCard == TypesOfCards.Creature)
        {
            // this is a creature
            AttackText.text = cardAsset.Attack.ToString();
            HealthText.text = cardAsset.MaxHealth.ToString();
        }

        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }

        if (RarityStoneImage == null)
            Debug.Log("RarityStoneImage is null on object:" + gameObject.name);

        // NEW apply rarity color to a card 
        RarityStoneImage.color = RarityColors.Instance.ColorsDictionary[cardAsset.Rarity];
    }
}
