using UnityEngine;
using System.Collections;

public abstract class State : MonoBehaviour 
{
    /// <summary>
    /// Called by the state machine when this becomes the current state.
    /// </summary>
	public virtual void Enter ()
	{
		AddListeners();
	}

    /// <summary>
    /// Called by the state machine when this state stops being the current state.
    /// </summary>
    public virtual void Exit ()
	{
		RemoveListeners();
	}

	protected virtual void OnDestroy ()
	{
		RemoveListeners();
	}

	protected virtual void AddListeners ()
	{

	}
	
	protected virtual void RemoveListeners ()
	{

	}
}