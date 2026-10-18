using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DoInFlashBack : StateMachineBehaviour
{
    PlayerMove playerMove;
    BossMove bossMove;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossMove = animator.GetComponentInParent<BossMove>();
        playerMove = bossMove.playerMove;
        bossMove.isFlashBack = true;
        bossMove.isNormalAttack = true;
        // 变换坐标,回头转向在BossMove里
        bossMove.faceDirection = playerMove.faceDirection;
        Vector3 newPozition = playerMove.transform.position;
        newPozition.x += (playerMove.faceDirection * -1.5f);
        newPozition.y = bossMove.transform.position.y;
        newPozition.z = -1;
        bossMove.transform.position = newPozition;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
