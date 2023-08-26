namespace CommsRadioAPI;

/// <summary>
/// Implement this class to define the behavior of the Comms Radio when it's in a given state.
/// </summary>
public abstract class StateActionUpdateHandler
{
	public readonly CommsRadioState state;

	public StateActionUpdateHandler(CommsRadioState state)
	{
		this.state = state;
	}

	/// <summary>
	/// This method is called when the instance becomes the active state of the Comms Radio.
	/// This can happen when transitioning to this state or when enabling the Comms Radio.
	/// Use it to do any required setup.
	/// </summary>
	/// <param name="utility">Provides access to some useful functionality, eg. <c>StartCoroutine</c>.</param>
	public void OnEnter(CommsRadioUtility utility) {}

	/// <summary>
	/// This method is called when the instance is no longer the active state of the Comms Radio.
	/// This can happen when transitioning away from this state or when disabling the Comms Radio.
	/// Use it to do any required cleanup.
	/// </summary>
	/// <param name="utility">Provides access to some useful functionality, eg. <c>StopCoroutine</c>.</param>
	public void OnLeave(CommsRadioUtility utility) {}

	/// <summary>
	/// Use this method to respond to player input to the Comms Radio.
	/// <c>InputAction.Activate</c> must always result in a state transition.
	/// </summary>
	/// <param name="utility">Provides access to some useful functionality, eg. <c>StartCoroutine</c>.</param>
	/// <param name="action"></param>
	/// <returns>The next state/action/update handler to use. Do NOT return <c>this</c>. Use <c>ButtonBehaviourType.Ignore</c> instead if you wish to ignore the Up & Down buttons.</returns>
	public abstract StateActionUpdateHandler OnAction(CommsRadioUtility utility, InputAction action);

	/// <summary>
	/// This method runs on each cycle of the game loop.
	/// It can be used, for example, to keep track of where the Comms Radio laser is pointing.
	/// </summary>
	/// <param name="utility">Provides access to some useful functionality, eg. <c>StartCoroutine</c>.</param>
	/// <returns>The next state/action/update handler to use. Return <c>this</c> if no state transition is necessary.</returns>
	public StateActionUpdateHandler OnUpdate(CommsRadioUtility utility)
	{
		return this;
	}
}
