using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageAllOpponentCreatures : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToDamage = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
        List<DamageCommandInfo> Targets = new List<DamageCommandInfo>();
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            Targets.Add(new DamageCommandInfo(cl.ID, cl.Health-specialAmount, specialAmount));
        }
        new DealDamageCommand(Targets).AddToQueue();

        // deal damage to creatures only after making a command to prevent a situation: die first - deal damage after
        foreach(CreatureLogic cl in CreaturesToDamage)
            cl.Health -= specialAmount;
    }
}
