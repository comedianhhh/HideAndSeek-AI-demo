using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YBot_CryState : YBot_BaseState
{

    private AnimationListener animationListener;

    private readonly int ANIM_Crying = Animator.StringToHash("Crying");


    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        animationListener = owner.GetComponent<AnimationListener>();

        Debug.Assert(animationListener != null, $"{owner.name}AnimationListener not found");
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animationListener.AddAnimationCompletedListener(ANIM_Crying, OnAnimationCompleted);
        agent.enabled = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.enabled = true;
    }

    private void OnAnimationCompleted(int shortHashCode)
    {
        if(shortHashCode== ANIM_Crying)
        {
            fsm.ChangeState(fsm.JogStateName);
        }
    }
}
