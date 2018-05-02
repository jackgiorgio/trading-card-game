using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum TargetingOptions
{
    NoTarget,
    AllCreatures, 
    EnemyCreatures,
    YourCreatures, 
    AllCharacters, 
    EnemyCharacters,
    YourCharacters
}

public enum RarityOptions
{
    Basic, Common, Rare, Epic, Legendary
}

public enum TypesOfCards
{
    Creature, Spell
}

public class CardAsset : ScriptableObject , IComparable<CardAsset>
{
    // this object will hold the info about the most general card
    [Header("General info")]
    public CharacterAsset CharacterAsset;  // if this is null, it`s a neutral card
    [TextArea(2,3)]
    public string Description;  // Description for spell or character
    [TextArea(2,3)]
    public string Tags;  // tags that can be searched as keywords
    public RarityOptions Rarity;
	[PreviewSprite]
    public Sprite CardImage;
    public int ManaCost;
    public bool TokenCard = false; // token cards can not be seen in collection
    public int OverrideLimitOfThisCardInDeck = -1;

    public TypesOfCards TypeOfCard;


    [Header("Creature Info")]
    [Range(1, 30)]
    public int MaxHealth =1;   
    [Range(1, 30)]
    public int Attack;
    [Range(1, 4)]
    public int AttacksForOneTurn = 1;
    public bool Charge;
    public bool Taunt;
    public string CreatureScriptName;
    public int SpecialCreatureAmount;

    [Header("SpellInfo")]
    public string SpellScriptName;
    public int SpecialSpellAmount;
    public TargetingOptions Targets;

    public int CompareTo (CardAsset other) 
    {
        if (other.ManaCost < this.ManaCost)
        {
            return 1;
        }
        else if (other.ManaCost > this.ManaCost)
        {
            return -1;
        }
        else
        {
            // if mana costs are equal sort in alphabetical order
            return name.CompareTo(other.name);
        }
    }

    // Define the is greater than operator.
    public static bool operator >  (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) == 1;
    }

    // Define the is less than operator.
    public static bool operator <  (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) == -1;
    }

    // Define the is greater than or equal to operator.
    public static bool operator >=  (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    // Define the is less than or equal to operator.
    public static bool operator <=  (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }

}
