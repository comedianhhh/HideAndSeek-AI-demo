using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    


    public float distanceToWall=0.3f;

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

        animationListener.AddAnimationCompletedListener(ANIM_StandToCover, OnAnimationCompleted);
        animationListener.AddAnimationCompletedListener(ANIM_CoverToStand, OnAnimationCompleted);

        coverStates = CoverStates.CoverToStand;

        AlignWithCover();

        YBotAnimator.SetTrigger("Cover");

    }


    private void OnAnimationCompleted(int shortHashCode)
    {
        if (shortHashCode == ANIM_StandToCover)
        {
            coverStates = CoverStates.StandToCover;
        }
        else if (shortHashCode == ANIM_CoverToStand)
        {
            coverStates = CoverStates.CoverToStand;
        }
    }

    private void AlignWithCover()
    {
        RaycastHit hitInfo;
        if (FindNearestCoverObject(out hitInfo))
        {
            // Align the character to be a certain distance from the wall, facing away from it
            transform.position = hitInfo.point + hitInfo.normal * distanceToWall; // 'distanceToWall' is the desired distance from the wall

            // Make the character face away from the wall, which is the opposite direction of the wall's normal
            transform.rotation = Quaternion.LookRotation(-hitInfo.normal);
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
