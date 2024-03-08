using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static YBot_CoverState;
using static YBot_JumpWithStyleState;

public class YBot_CoverState : YBot_BaseState
{
    public enum CoverStates
    {
        StandToCover,
        Cover,
        CoverToStand
    }
    public CoverStates coverStates = CoverStates.StandToCover;

    public bool HasBeenCaught=false;

    public float distanceToWall=0.2f;

    private AnimationListener animationListener;
    private readonly int ANIM_StandToCover = Animator.StringToHash("StandToCover");
    private readonly int ANIM_CoverToStand = Animator.StringToHash("CoverToStand");


    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        animationListener = owner.GetComponent<AnimationListener>();

        Debug.Assert(animationListener != null, $"{owner.name}AnimationListener not found");
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        AlignWithCover();

        animationListener.AddAnimationCompletedListener(ANIM_StandToCover, OnAnimationCompleted);
        animationListener.AddAnimationCompletedListener(ANIM_CoverToStand, OnAnimationCompleted);
        GameManager.Instance.OnFound += OnFoundHandler;

        coverStates = CoverStates.StandToCover;
        HasBeenCaught = false;
        
        YBotAnimator.SetTrigger("Cover");

    }



    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //switch (coverStates)
        //{
        //    case CoverStates.StandToCover:
        //        break;
        //    case CoverStates.Cover:
        //        break;
        //    case CoverStates.CoverToStand:
        //        break;

        //}
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.OnFound-= OnFoundHandler;
    }
    private void OnAnimationCompleted(int shortHashCode)
    {
        if (shortHashCode == ANIM_StandToCover)
        {
            coverStates = CoverStates.Cover;
            
        }
        else if (shortHashCode == ANIM_CoverToStand)
        {
            fsm.ChangeState(fsm.CryStateName);
        }
    }

    private void AlignWithCover()
    {
        RaycastHit hitInfo;
        if (FindNearestCoverObject(out hitInfo))
        {
            // Position the player a certain distance from the wall
            transform.position = hitInfo.point + hitInfo.normal * distanceToWall;

            // Calculate forward direction for the player
            Vector3 forwardDirection = hitInfo.normal;

            // Set the player's rotation to face away from the wall
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
            transform.rotation= targetRotation;
        }
    }

    private bool FindNearestCoverObject(out RaycastHit hitInfo)
    {
        GameObject[] coverObjects = GameObject.FindGameObjectsWithTag("Cover");
        bool hasHit = false;
        float nearestDistance = Mathf.Infinity;
        hitInfo = new RaycastHit();

        foreach (GameObject cover in coverObjects)
        {
            Vector3 directionToCover = cover.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToCover);
            if (angle > 90) continue; // Cover is not in front of the player

            if (Physics.Raycast(transform.position, directionToCover.normalized, out RaycastHit hit))
            {
                // Check if the first object hit is the cover object
                if (hit.collider.gameObject == cover && hit.distance < nearestDistance)
                {
                    nearestDistance = hit.distance;
                    hitInfo = hit;
                    hasHit = true;
                }
            }
        }

        return hasHit;
    }

    void OnFoundHandler()
    {
        HasBeenCaught = true;
        if (coverStates == CoverStates.Cover)
        {
            YBotAnimator.SetTrigger("FoundCover");
        }
        coverStates = CoverStates.CoverToStand;
        Debug.Log("I've been found!");
    }
    private void OnAnimationMove()
    {
        switch (coverStates)
        {
            case CoverStates.StandToCover:
                transform.position = YBotAnimator.rootPosition;
                break;

            case CoverStates.CoverToStand:
                transform.position = YBotAnimator.rootPosition;
                break;

        }
    }

    
}
