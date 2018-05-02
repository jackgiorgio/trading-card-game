using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragPackOpening : DraggingActions 
{   
    private bool canceling = false;
    private bool movingToOrReachedOpeningSpot = false;
    private Vector3 savedPosition;

    public override bool CanDrag
    {
        get
        { 
            return ShopManager.Instance.OpeningArea.AllowedToDragAPack && !canceling && !movingToOrReachedOpeningSpot;
        }
    }
        
    public override void OnStartDrag()
    {
        savedPosition = transform.localPosition;
        ShopManager.Instance.OpeningArea.AllowedToDragAPack = false;
    }

    public override void OnDraggingInUpdate()
    {
         
    }

    public override void OnEndDrag()
    {        
        // 1) Check if we are holding a card over the table
        if (DragSuccessful())
        {
            // snap the pack to the center of the pack opening area
            transform.DOMove(ShopManager.Instance.OpeningArea.transform.position, 0.5f).OnComplete(()=>
                { 
                    // enable opening on click
                    GetComponent<ScriptToOpenOnePack>().AllowToOpenThisPack();
                });
        }
        else
            OnCancelDrag();
    }

    public override void OnCancelDrag()
    {
        canceling = true;
        transform.DOLocalMove(savedPosition, 1f).OnComplete(() =>
            {
                canceling = false;
                ShopManager.Instance.OpeningArea.AllowedToDragAPack = true;
            });
    } 

    protected override bool DragSuccessful()
    {
        return ShopManager.Instance.OpeningArea.CursorOverArea();
    }

}
