using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawACard : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        int cards = specialAmount;
        while (cards > 0)
        {
            TurnManager.Instance.whoseTurn.DrawACard (fast: true); 
            cards-- ;
        }
    }
}