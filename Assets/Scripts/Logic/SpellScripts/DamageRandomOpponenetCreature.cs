using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRandomOpponenetCreature : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        List<CreatureLogic> opponentCreatures = owner.otherPlayer.table.CreaturesOnTable;

        if (opponentCreatures.Count == 0)
            return;

        int randomIndex = Random.Range(0, opponentCreatures.Count);

        CreatureLogic creatureToDamage = opponentCreatures[randomIndex];

        new DealDamageCommand(new List<DamageCommandInfo>{ new DamageCommandInfo(creatureToDamage.ID, creatureToDamage.Health - specialAmount, specialAmount)}).AddToQueue();

        creatureToDamage.Health -= specialAmount;
    }
}
