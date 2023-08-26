using DV;

namespace CommsRadioAPI;

/// <summary>
/// Describes a state of the Comms Radio.
/// </summary>
public sealed class CommsRadioState
{
	public readonly string lcdTitle;
	public readonly string lcdContent;
	public readonly string lcdAction;
	public readonly LCDArrowState lcdArrow;
	public readonly LEDState led;
	public readonly ButtonBehaviourType behaviour;

	/// <summary>
	/// Construct a new CommsRadioState object.
	/// </summary>
	/// <param name="titleText">Text to be displayed in large letters at the top of the LCD screen. (default: "")</param>
	/// <param name="contentText">Text to be displayed in small letters below the title on the LCD screen. (default: "")</param>
	/// <param name="actionText">Text to be displayed in large letters at the bottom of the LCD screen. (default: "")</param>
	/// <param name="arrowState">Whether the arrow on the LCD screen is off, pointing left, or pointing right. (default: Off)</param>
	/// <param name="ledState">Whether the LED light on the back of the Comms Radio is off or on. (default: Off)</param>
	/// <param name="buttonBehaviour">Whether the up and down buttons switch between Comms Radio modes (Regular), trigger the state action handler (Override), or are completely ignored (Ignore). (default: Regular)</param>
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
