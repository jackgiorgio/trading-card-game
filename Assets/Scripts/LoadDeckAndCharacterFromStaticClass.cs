using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDeckAndCharacterFromStaticClass : MonoBehaviour {

    void Awake()
    {
        Player p = GetComponent<Player>();
        if (BattleStartInfo.SelectedDeck != null)
        {
            if (BattleStartInfo.SelectedDeck.Character!=null)
                p.charAsset = BattleStartInfo.SelectedDeck.Character;
            if (BattleStartInfo.SelectedDeck.Cards!=null)
                p.deck.cards = new List<CardAsset>(BattleStartInfo.SelectedDeck.Cards);
        }       

    }
}
