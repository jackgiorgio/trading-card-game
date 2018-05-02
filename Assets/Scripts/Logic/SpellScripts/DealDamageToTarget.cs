using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DealDamageToTarget : SpellEffect 
{    
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {        
        new DealDamageCommand(new List<DamageCommandInfo>{ new DamageCommandInfo(target.ID, target.Health - specialAmount, specialAmount)}).AddToQueue();
        target.Health -= specialAmount;
    }
}
