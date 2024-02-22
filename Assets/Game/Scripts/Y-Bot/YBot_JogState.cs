using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class YBot_JogState : YBot_BaseState
{
    public float OffMeshAngleThreshold = 5.0f;
    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;

    private readonly int SpeedParameter = Animator.StringToHash("Speed");
    private AnimationListener animationListener;

    [Header("Hiding Spots")]
    public GameObject[] hidingSpots;
    public GameObject targetHidingSpot = null;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        animationListener = owner.GetComponent<AnimationListener>();

        hidingSpots = GameObject.FindGameObjectsWithTag("HidingSpot");
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetHidingSpot = ChooseHidingSpot();
        if(targetHidingSpot!=null)
        {
            agent.SetDestination(targetHidingSpot.transform.position);
        }
        animationListener.OnAnimatorMoveEvent += OnAnimatorMove;

    }
    private GameObject ChooseHidingSpot()
    {
        if (hidingSpots == null || hidingSpots.Length == 0) return null;
        int index = UnityEngine.Random.Range(0, hidingSpots.Length);
        return hidingSpots[index];
    }
    private void OnAnimatorMove()
    {
        agent.velocity = YBotAnimator.deltaPosition / Time.deltaTime;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (agent.desiredVelocity != Vector3.zero)
        {
            float speed = Vector3.Project(agent.desiredVelocity, transform.forward).magnitude * agent.speed;
            YBotAnimator.SetFloat(SpeedParameter, speed);

            float angle = Vector3.Angle(transform.forward, agent.desiredVelocity);
            if (Mathf.Abs(angle) <= deadZone)
            {
                transform.LookAt(transform.position + agent.desiredVelocity);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                     Quaternion.LookRotation(agent.desiredVelocity),
                                                     Time.deltaTime * angularDampeningTime);
            }
        }
        else
        {
            CheckIfArrivedAtHidingSpot();
        }
    }
    private void CheckIfArrivedAtHidingSpot()
    {
        // Check if we've reached the hiding spot
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Assume you have a method in your FSM to change to the Hide state
            fsm.ChangeState(fsm.CoverStateName);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
    }
}
