using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragCreatureAttack : DraggingActions {

    // private int savedCreatureSlot;
    // reference to the sprite with a round "Target" graphic
    private SpriteRenderer sr;
    // LineRenderer that is attached to a child game object to draw the arrow
    private LineRenderer lr;
    // reference to WhereIsTheCardOrCreature to track this object`s state in the game
    private WhereIsTheCardOrCreature whereIsThisCreature;
    // the pointy end of the arrow, should be called "Triangle" in the Hierarchy
    private Transform triangle;
    // SpriteRenderer of triangle. We need this to disable the pointy end if the target is too close.
    private SpriteRenderer triangleSR;
    // when we stop dragging, the gameObject that we were targeting will be stored in this variable.
    private GameObject Target;
    // Reference to creature manager, attached to the parent game object
    private OneCreatureManager manager;

    private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
    private Vector3[] linePositions = new Vector3[0];
    private Vector3[] linePositionsOld = new Vector3[0];

    //How high do we want the creature token to be lifted when starting to drag it.
    // public float dragHeight = -1f;
    //How fast the "move to" transition is
    // public float dragSpeed = 0.5f;

    void Awake()
    {
        // establish all the connections
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        manager = GetComponentInParent<OneCreatureManager>();
        whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override bool CanDrag
    {
        get
        {
            // TEST LINE: just for testing
            //return true;
			// we can drag this card if
            // a) we can control this our player (this is checked in base.canDrag)
            // b) creature "CanAttackNow" - this info comes from logic part of our code into each creature`s manager script
            return base.CanDrag && manager.CanAttackNow;
        }
    }

    public override void OnStartDrag()
    {
        // savedCreatureSlot = whereIsThisCreature.Slot;
        whereIsThisCreature.VisualState = VisualStates.Dragging;
        // enable target graphic
        sr.enabled = true;
        // enable line renderer to start drawing the line.
        lr.enabled = true;

        //transform.parent.DOMoveY(-5f,0.6f);
        //transform.parent.DOMoveZ(dragHeight,dragSpeed);
        //transform.parent.DORotate(Vector3.zero, 0.6f, 0);
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            //find curved points in children
            linePoints = lr.gameObject.GetComponentsInChildren<CurvedLinePoint>();

            //add positions
            Vector3 midPoint = Vector3.Lerp(transform.parent.position, transform.position - direction ,0.5f);
            linePositions = new Vector3[linePoints.Length];
            
            //Define midpoint height via numbers of line positions and set min max heights.       
            midPoint += new Vector3(0,0,-lr.positionCount * 0.1f);
            if(midPoint.z > 0)
                midPoint.z = 0;
            if(midPoint.z < -5f)
                midPoint.z = -5f;

            //If linerendere distance is short, set heights to zero, if we don't do this there will be a "height artifact" when line is short
            if(lr.positionCount < 2)
                midPoint.z = 0;

            //Set line points according to start mid and end of line
            linePoints[0].transform.position = transform.parent.position;
            linePoints[1].transform.position = midPoint;
            linePoints[2].transform.position = transform.position - direction*2f;
            
            //Set linerender positions defined by line points
            linePositions[0] = linePoints[0].transform.position;
            linePositions[1] = linePoints[1].transform.position;
            linePositions[2] = linePoints[2].transform.position;

            //create old positions if they dont match
            if( linePositionsOld.Length != linePositions.Length )
            {
                linePositionsOld = new Vector3[linePositions.Length];
            }

            //get smoothed values
            Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, 2 );

            //set line settings
            lr.positionCount = smoothedPoints.Length;
            lr.SetPositions( smoothedPoints );

            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f*direction;

            // proper rotarion of arrow end
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        } 
    }

    public override void OnEndDrag()
    {
        Target = null;
        RaycastHit[] hits;
        // TODO: raycast here anyway, store the results in
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f) ;

        foreach (RaycastHit h in hits)
        {
            if ((h.transform.tag == "TopPlayer" && this.tag == "LowCreature") ||
                (h.transform.tag == "LowPlayer" && this.tag == "TopCreature"))
            {
                // go face
                Target = h.transform.gameObject;
            }
            else if ((h.transform.tag == "TopCreature" && this.tag == "LowCreature") ||
                    (h.transform.tag == "LowCreature" && this.tag == "TopCreature"))
            {
                // hit a creature, save parent transform
                Target = h.transform.parent.gameObject;
            }

        }

        bool targetValid = false;

        if (Target != null)
        {
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            Debug.Log("Target ID: " + targetID);
            if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
            {
                // attack character
                Debug.Log("Attacking "+Target);
                Debug.Log("TargetID: " + targetID);
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].GoFace();
                targetValid = true;
            }
            else if (CreatureLogic.CreaturesCreatedThisGame[targetID] != null)
            {
                // if targeted creature is still alive, attack creature
                targetValid = true;
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].AttackCreatureWithID(targetID);
                Debug.Log("Attacking "+Target);
            }

        }

        if (!targetValid)
        {
            // not a valid target, return
            if(tag.Contains("Low"))
                whereIsThisCreature.VisualState = VisualStates.LowTable;
            else
                whereIsThisCreature.VisualState = VisualStates.TopTable;
            whereIsThisCreature.SetTableSortingOrder();

            //Return Creature down to board after dragging
            //transform.parent.DOMoveZ(0,dragSpeed);
        }

        // return target and arrow to original position
        transform.localPosition = Vector3.zero;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }

    public override void OnCancelDrag()
    {
      
    }
}
