using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
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

    public GameObject currentHidingSpot = null;

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
        AlignWithCover();
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
        Quaternion targetRotation = Quaternion.LookRotation(-Ybot.CurrentSpot.transform.forward);
        transform.rotation= targetRotation;
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
