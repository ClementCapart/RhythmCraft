using UnityEngine;
using System.Collections;

public class StartPatternBehaviourState : StateMachineBehaviour 
{    
    public bool m_WaitForResult = true;

    public string m_itemUniqueID = "";

    private Animator m_Animator = null;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {      
        m_Animator = animator;

        CraftPatternPlayer craftPatternPlayer = GameObject.FindObjectOfType<CraftPatternPlayer>();

        ItemData itemData = ItemDatabase.GetItemByUniqueID(m_itemUniqueID);
        if(itemData != null)
        { 
            craftPatternPlayer.StartPattern(itemData);
        }
        else
        {
            animator.SetTrigger("GotoNextState");
        }

        if (!m_WaitForResult)
        {
            animator.SetTrigger("GotoNextState");
        }
        else
        {
            CraftPatternPlayer.s_craftSequenceEnded += OnCraftEnded;
        }
	}

    void OnCraftEnded(ItemData item, CraftState state)
    {
        CraftPatternPlayer.s_craftSequenceEnded -= OnCraftEnded;

        if(state == CraftState.Success)
        {
            m_Animator.SetTrigger("GotoNextState");
        }
        else
        {
            m_Animator.SetTrigger("GotoFailState");
        }        
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
