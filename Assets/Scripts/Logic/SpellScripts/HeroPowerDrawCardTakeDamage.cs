using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroPowerDrawCardTakeDamage : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        // Take 2 damage
        new DealDamageCommand(new List<DamageCommandInfo>{ new DamageCommandInfo(TurnManager.Instance.whoseTurn.PlayerID, TurnManager.Instance.whoseTurn.Health - 2, 2)}).AddToQueue();
        TurnManager.Instance.whoseTurn.Health -= 2;
        // Draw a card
        TurnManager.Instance.whoseTurn.DrawACard();

    }
}
