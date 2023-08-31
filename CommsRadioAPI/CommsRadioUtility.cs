using System.Collections;
using UnityEngine;

namespace CommsRadioAPI;

/// <summary>
/// A convenience class that allows classes that inherit <c>StateActionUpdateHandler</c> to interact with their parent <c>CommsRadioMode</c>.
/// </summary>
public class CommsRadioUtility
{
	private CommsRadioMode target;

	internal CommsRadioUtility(CommsRadioMode mode)
	{
		target = mode;
	}

	/// <summary>
	/// Start a coroutine on the Comms Radio mode.
	/// </summary>
	/// <param name="routine">The return value from a coroutine method.</param>
	/// <returns>The coroutine instance. Can be passed to <c>StopCoroutine</c>.</returns>
	public Coroutine StartCoroutine(IEnumerator routine)
	{
		return target.StartCoroutine(routine);
	}

	/// <summary>
	/// Stop a running coroutine.
	/// </summary>
	/// <param name="routine">The coroutine to stop, as returned by <c>StartCoroutine</c>.</param>
	public void StopCoroutine(Coroutine routine)
	{
		target.StopCoroutine(routine);
	}

	/// <summary>
	/// Play a vanilla sound from the Comms Radio.
	/// </summary>
	/// <param name="sound">The vanilla sound to play.</param>
	/// <param name="source"><em>Optional.</em><br/>Location of the sound source for spacial audio.</param>
	public void PlaySound(VanillaSoundCommsRadio sound, Transform? source = null)
	{
		ControllerAPI.PlaySound(sound, source ?? target.signalOrigin ?? target.gameObject.transform);
	}

	/// <summary>
	/// Play a vanilla sound from a locomotive or wagon.
	/// </summary>
	/// <param name="sound">The vanilla vehicle sound to play.</param>
	/// <param name="source">The locomotive or wagon that will be used as the sound source for spacial audio.</param>
	/// <param name="parentToWorld"><em>Optional.</em><br/><em>(TODO: What does this do?)</em></param>
	public void PlayVehicleSound(VanillaSoundVehicle sound, TrainCar source, bool parentToWorld = false)
	{
		ControllerAPI.PlayVehicleSound(sound, source, parentToWorld);
	}

	/// <summary>
	/// Get a material used by the vanilla Comms Radio modes to highlight vehicles.
	/// </summary>
	/// <param name="material">The type of material to retrieve.</param>
	/// <returns>The requested material.</returns>
	public Material? GetMaterial(VanillaMaterial material)
	{
		return ControllerAPI.GetMaterial(material);
	}
}
