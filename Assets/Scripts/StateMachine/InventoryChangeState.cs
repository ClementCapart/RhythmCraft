using UnityEngine;
using System.Collections;

public class InventoryChangeState : StateMachineBehaviour 
{    
    public enum InventoryAction
    {
        Add,
        Remove,
    }

    public InventoryAction m_ActionType;

    public int m_Count;

    public string m_ItemUniqueID = "";

    private Animator m_Animator = null;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {      
        m_Animator = animator;        

        ItemData itemData = ItemDatabase.GetItemByUniqueID(m_ItemUniqueID);
        if(itemData != null)
        { 
            if(m_ActionType == InventoryAction.Add)
            {
                Inventory.Instance.AddItem(itemData, m_Count);
            }
            else if (m_ActionType == InventoryAction.Remove)
            {
                Inventory.Instance.RemoveItem(itemData, m_Count);
            }            
        }
        
        animator.SetTrigger("GotoNextState");        
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
