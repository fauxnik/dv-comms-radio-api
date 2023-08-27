namespace CommsRadioAPI;

/// <summary>
/// Refers to a Comms Radio mode from the vanilla game.
/// </summary>
public enum VanillaModes
{
	/// <summary>
	/// Rerails locomotives and wagons.
	/// </summary>
	Rerail,

	/// <summary>
	/// Throws junctions/switches.
	/// </summary>
	Junction,

	/// <summary>
	/// Removes locomotives and wagons.
	/// </summary>
	Clear,

	/// <summary>
	/// Summons crew vehicles, like the handcar, caboose, and slug.
	/// </summary>
	SummonCrewVehicle,

	/// <summary>
	/// Spawns locomotives and wagons.<br/>Only available in sandbox.
	/// </summary>
	Spawn,

	/// <summary>
	/// Loads cargo into wagons.<br/>Only available in sandbox.
	/// </summary>
	LoadCargo,

	/// <summary>
	/// Increases locomotive or wagon damage by the chosen amount.<br/>Only available in sandbox.
	/// </summary>
	Damage,

	/// <summary>
	/// Activates or deactivates a dim light on the back of the Comms Radio.
	/// </summary>
	LED,
}
