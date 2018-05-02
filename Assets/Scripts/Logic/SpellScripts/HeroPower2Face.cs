using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroPower2Face : SpellEffect 
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(new List<DamageCommandInfo>{ new DamageCommandInfo(TurnManager.Instance.whoseTurn.otherPlayer.PlayerID, TurnManager.Instance.whoseTurn.otherPlayer.Health - 2, 2)}).AddToQueue();
        TurnManager.Instance.whoseTurn.otherPlayer.Health -= 2;
    }
}
