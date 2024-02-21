using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class YBot_FSM : FSM
{
    public readonly int IdleStateName = Animator.StringToHash("Idle");
    public readonly int MoveToGoalStateName = Animator.StringToHash("Move To Goal");
    public readonly int ClimbStateName = Animator.StringToHash("Climb");
    public readonly int JumpWithStyleStateName = Animator.StringToHash("Jump With Style");
    public readonly int JumpAcrossStateName = Animator.StringToHash("Jump Across");
    public readonly int ClimbDownStateName = Animator.StringToHash("Climb Down");
    public readonly int JogStateName = Animator.StringToHash("Jog");
    public readonly int CoverStateName = Animator.StringToHash("Cover");
    public readonly int CryStateName=Animator.StringToHash("Cry");
}
