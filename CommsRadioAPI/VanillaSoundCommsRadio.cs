namespace CommsRadioAPI;

/// <summary>
/// Refers to the vanilla sounds that eminate from the Comms Radio.
/// </summary>
public enum VanillaSoundCommsRadio
{
	/// <summary>
	/// CommsRadio_Confirm_01
	/// </summary>
	Confirm,
	/// <summary>
	/// CommsRadio_Cancel_01
	/// </summary>
	Cancel,
	/// <summary>
	/// CommsRadio_Message_01
	/// </summary>
	Warning,
	/// <summary>
	/// CommsRadio_Confirm_01<br/>
	/// This may play CommsRadio_Select_01 in the future. Use `Confirm` instead for CommsRadio_Confirm_01.
	/// </summary>
	Switch,
	/// <summary>
	/// CommsRadio_Apply_01
	/// </summary>
	ModeEnter,
	/// <summary>
	/// CommsRadio_Hover_01
	/// </summary>
	HoverOver,
	MoneyRemoved,
}
