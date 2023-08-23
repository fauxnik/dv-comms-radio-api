namespace CommsRadioAPI;

public abstract class StateActionUpdateHandler
{
	public readonly CommsRadioState state;

	public StateActionUpdateHandler(CommsRadioState state)
	{
		this.state = state;
	}

	public abstract StateActionUpdateHandler OnAction(InputAction action);

	public abstract StateActionUpdateHandler OnUpdate();
}
