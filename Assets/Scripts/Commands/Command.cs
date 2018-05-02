using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Commands are used to collect everything that happens instantly in game Logic
// and show it gradually in certain order in the Visual part of the game
public abstract class Command
{
    // this will be true if we are showing some command in the Visual part of our game
    // and false - if our CommandQueue is empty
    public static bool playingQueue {get; set;}

    // a collection of Commands (first in - first out)
    static Queue<Command> CommandQueue = new Queue<Command>();

    // this method should always be called when a new Command is created.
    // For example: 
    // new DelayCommand(3f).AddToQueue(); - will add a 3 seconds delay into the CommandQueue
    public void AddToQueue()
    {
        CommandQueue.Enqueue(this);
        if (!playingQueue)
            PlayFirstCommandFromQueue();
    }

    // Include a list of everything that you want to do with this command into this method 
    // (draw a card, play a card, play spell effect, etc...)
    // In StartCommandExecution you should call only methods in the Visual part of the game
    // there are 2 options of timing : 
    // 1) use tween sequences and call CommandExecutionComplete in OnComplete()
    // 2) use coroutines (IEnumerator) and WaitFor... to introduce delays, call CommandExecutionComplete() in the end of coroutine
    public abstract void StartCommandExecution();


    public static bool CardDrawPending()
    {
        foreach (Command c in CommandQueue)
        {
            if (c is DrawACardCommand)
                return true;
        }
        return false;
    }

    // method to move to the next command in CommandQueue
    public static void CommandExecutionComplete()
    {
        if (CommandQueue.Count > 0)
            PlayFirstCommandFromQueue();
        else
            playingQueue = false;

        // after every Command we`ll highlight avaliable moves for current Player
        if (TurnManager.Instance.whoseTurn != null)
            TurnManager.Instance.whoseTurn.HighlightPlayableCards();
    }

    // plays command that has the index 0 in CommandQueue
    static void PlayFirstCommandFromQueue()
    {
        playingQueue = true;
        CommandQueue.Dequeue().StartCommandExecution();
    }
        
    public static void OnSceneReload()
    {
        CommandQueue.Clear();
        CommandExecutionComplete();
    }
}
