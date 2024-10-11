using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantasma_Seguir_Behaviour : StateMachineBehaviour
{
    private Unit ghost;
    private float player_pos_x, player_pos_z;
    private float anguloRot;
    private float tiempo;


    public Transform target;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {  
        ghost  = animator.gameObject.GetComponent<Unit>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        tiempo = ghost.tiempoSeguir;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        player_pos_x = target.position.x - ghost.transform.position.x;
        player_pos_z = target.position.z - ghost.transform.position.z;
        anguloRot = Mathf.Atan2(player_pos_x, player_pos_z) * Mathf.Rad2Deg;
        ghost.transform.rotation = Quaternion.Euler(new Vector3(0, anguloRot, 0));
        PathRequestManager.RequestPath(ghost.transform.position, target.position, ghost.OnPathFound);
        
        tiempo -= Time.deltaTime;
        if (tiempo <= 0) { 
            animator.SetBool("Detectado", false);
            animator.SetTrigger("Disolver");
        }


    }
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}

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

