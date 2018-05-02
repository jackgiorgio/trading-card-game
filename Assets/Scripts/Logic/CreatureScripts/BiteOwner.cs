using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiteOwner : CreatureEffect
{  
    public BiteOwner(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    public override void RegisterEventEffect()
    {
        owner.EndTurnEvent += CauseEventEffect;
        //owner.otherPlayer.EndTurnEvent += CauseEventEffect;
        Debug.Log("Registered bite effect!!!!");
    }

    public override void UnRegisterEventEffect()
    {
        owner.EndTurnEvent -= CauseEventEffect;
    }

    public override void CauseEventEffect()
    {
        Debug.Log("InCauseEffect: owner: "+ owner + " specialAmount: "+ specialAmount);
        new DealDamageCommand(new List<DamageCommandInfo>(){new DamageCommandInfo(owner.ID, owner.Health - specialAmount, specialAmount)}).AddToQueue();
        owner.Health -= specialAmount;
    }


}
