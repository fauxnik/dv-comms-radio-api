using DV;

namespace CommsRadioAPI;

/// <summary>
/// Describes a state of the Comms Radio.
/// </summary>
public sealed class CommsRadioState
{
	/// <summary>
	/// The text displayed in large letters at the top of the LCD screen.
	/// </summary>
	public readonly string lcdTitle;

	/// <summary>
	/// The text displayed in small letters below the title on the LCD screen.
	/// </summary>
	public readonly string lcdContent;

	/// <summary>
	/// The text displayed in large letters at the bottom of the LCD screen.
	/// </summary>
	public readonly string lcdAction;

	/// <summary>
	/// Whether the arrow on the LCD screen is off, pointing left, or pointing right.
	/// </summary>
	public readonly LCDArrowState lcdArrow;

	/// <summary>
	/// Whether the LED light on the back of the Comms Radio is off or on.
	/// </summary>
	public readonly LEDState led;

	/// <summary>
	/// Whether the up and down buttons switch between Comms Radio modes (Regular), trigger the state action handler (Override), or are completely ignored (Ignore).
	/// </summary>
	public readonly ButtonBehaviourType behaviour;

	/// <summary>
	/// Construct a new CommsRadioState object.
	/// </summary>
	/// <param name="titleText">Text to be displayed in large letters at the top of the LCD screen. (default: "")</param>
	/// <param name="contentText">Text to be displayed in small letters below the title on the LCD screen. (default: "")</param>
	/// <param name="actionText">Text to be displayed in large letters at the bottom of the LCD screen. (default: "")</param>
	/// <param name="arrowState">Whether the arrow on the LCD screen should be off, point left, or point right. (default: Off)</param>
	/// <param name="ledState">Whether the LED light on the back of the Comms Radio should be off or on. (default: Off)</param>
	/// <param name="buttonBehaviour">Whether the up and down buttons should switch between Comms Radio modes (Regular), trigger the state action handler (Override), or be completely ignored (Ignore). (default: Regular)</param>
	public CommsRadioState(string titleText = "", string contentText = "", string actionText = "", LCDArrowState arrowState = LCDArrowState.Off, LEDState ledState = LEDState.Off, ButtonBehaviourType buttonBehaviour = ButtonBehaviourType.Regular)
	{
		lcdTitle = titleText;
		lcdContent = contentText;
		lcdAction = actionText;
		lcdArrow = arrowState;
		led = ledState;
		behaviour = buttonBehaviour;
	}
}
