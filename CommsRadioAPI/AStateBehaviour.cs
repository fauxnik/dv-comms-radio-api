namespace CommsRadioAPI;

/// <summary>
/// Implement this abstract class to define the behaviour of a Comms Radio mode in a given state.
/// </summary>
public abstract class AStateBehaviour
{
	/// <summary>
	/// The state of the Comms Radio when it exhibits the behaviour described by the implementing class.
	/// </summary>
	public readonly CommsRadioState state;

	/// <summary>
	/// Instantiate a new action/update handler with the given Comms Radio state.
	/// </summary>
	/// <param name="state">The state of the Comms Radio when it exhibits the behaviour of the handler class.</param>
	public AStateBehaviour(CommsRadioState state)
	{
		this.state = state;
	}

	/// <summary>
	/// This method is called when the instance becomes the active state of the Comms Radio.<br/>
	/// This can happen when transitioning to this state or when enabling the Comms Radio.<br/>
	/// Use it to do any required setup.
	/// </summary>
	/// <param name="utility">Provides access to some useful functionality, eg. <c>StartCoroutine</c>.</param>
	public virtual void OnEnter(CommsRadioUtility utility) {}

	/// <summary>
	/// This method is called when the instance is no longer the active state of the Comms Radio.<br/>
	/// This can happen when transitioning away from this state or when disabling the Comms Radio.<br/>
	/// Use it to do any required cleanup.
	/// </summary>
	/// <param name="utility">Provides access to some useful functionality, eg. <c>StopCoroutine</c>.</param>
	public virtual void OnLeave(CommsRadioUtility utility) {}

	/// <summary>
	/// Use this method to respond to player input to the Comms Radio.<br/>
	/// <c>InputAction.Activate</c> must always result in a state transition.
	/// </summary>
	/// <param name="utility">Provides access to some useful functionality, eg. <c>StartCoroutine</c>.</param>
	/// <param name="action">The action that the player has taken.</param>
	/// <returns>The next state/action/update handler to use. Do NOT return <c>this</c>. Use <c>ButtonBehaviourType.Ignore</c> instead if you wish to ignore the Up & Down buttons.</returns>
	public abstract AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action);

	/// <summary>
	/// This method runs on each cycle of the game loop.<br/>
	/// It can be used, for example, to keep track of where the Comms Radio laser is pointing.
	/// </summary>
	/// <param name="utility">Provides access to some useful functionality, eg. <c>StartCoroutine</c>.</param>
	/// <returns>The next state/action/update handler to use. Return <c>this</c> if no state transition is necessary.</returns>
	public virtual AStateBehaviour OnUpdate(CommsRadioUtility utility)
	{
		return this;
	}
}
