namespace CommsRadioAPI;

/// <summary>
/// Describes the state of the arrow that appears in the lower right corner of the Comms Radio's LCD display.
/// </summary>
public enum LCDArrowState
{
	/// <summary>
	/// Indicates that the arrow is not visible.
	/// </summary>
	Off,

	/// <summary>
	/// Indicates that the arrow is visible and points to the left.
	/// </summary>
	Left,

	/// <summary>
	/// Indicates that the arrow is visible and points to the right.
	/// </summary>
	Right,
}
