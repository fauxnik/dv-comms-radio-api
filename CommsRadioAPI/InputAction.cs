namespace CommsRadioAPI;

/// <summary>
/// Describes an action dispatched in response to the player's input to the Comms Radio.
/// </summary>
public enum InputAction
{
	/// <summary>
	/// Dispatched when the player:
	///   PC - presses the left mouse button
	///   VR - pulls the VR controller trigger
	///   DV - presses the button on the side of the Comms Radio
	/// </summary>
	Activate,

	/// <summary>
	/// Dispatched when the player:
	///   PC - uses the scroll wheel
	///   VR - holds the VR controller trigger and flicks the joystick left(?)
	///   DV - presses the button on the face of the Comms Radio that displays an upwards pointing arrow
	/// </summary>
	Up,

	/// <summary>
	/// Dispatched when the player:
	///   PC - uses the scroll wheel
	///   VR - holds the VR controlle trigger and flicks the joystick right(?)
	///   DV - presses the button on the face of the Comms Radio that displays a downwards pointing arrow
	/// </summary>
	Down,
}
