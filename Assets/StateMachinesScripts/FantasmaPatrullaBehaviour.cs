using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantasmaPatrullaBehaviour : StateMachineBehaviour
{
    private Unit ghost;
    private float current_Waypoint_pos_x, current_Waypoint_pos_z;
    private float anguloRot;
    private float speed;
    private int wayPointIndex;
    Vector3[] patrolWaypoints;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ghost = animator.gameObject.GetComponent<Unit>();
        speed = ghost.speed;
        patrolWaypoints = ghost.waypointsPatrol;
        wayPointIndex = 1;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, patrolWaypoints[wayPointIndex], speed * Time.deltaTime);

        if (Vector3.Distance(ghost.transform.position, patrolWaypoints[wayPointIndex]) <= 0.4f)
            {
            Debug.Log(wayPointIndex);
            if (wayPointIndex == 0) wayPointIndex = 1;
            else wayPointIndex = 0;
                

                current_Waypoint_pos_x = patrolWaypoints[wayPointIndex].x - ghost.transform.position.x;
                current_Waypoint_pos_z = patrolWaypoints[wayPointIndex].z - ghost.transform.position.z;
                anguloRot = Mathf.Atan2(current_Waypoint_pos_x, current_Waypoint_pos_z) * Mathf.Rad2Deg;
                ghost.transform.rotation = Quaternion.Euler(new Vector3(0, anguloRot, 0));
               


            }
            
        

    }

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
