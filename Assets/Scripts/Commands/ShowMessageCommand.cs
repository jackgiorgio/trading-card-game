using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

public class ShowMessageCommand : Command {

    string message;
    float duration;

    public ShowMessageCommand(string message, float duration)
    {
        this.message = message;
        this.duration = duration;
    }

    public override void StartCommandExecution()
    {
        MessageManager.Instance.ShowMessage(message, duration);
        Sequence s = DOTween.Sequence();
        s.AppendInterval(duration);
        s.OnComplete(() =>
            {
                Command.CommandExecutionComplete();
            });
    }
}
