using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageOpponentBattlecry : CreatureEffect
{
    public DamageOpponentBattlecry(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        new DealDamageCommand(new List<DamageCommandInfo>(){new DamageCommandInfo(owner.otherPlayer.ID, owner.otherPlayer.Health - specialAmount, specialAmount)}).AddToQueue();
        owner.otherPlayer.Health -= specialAmount;
    }
}
