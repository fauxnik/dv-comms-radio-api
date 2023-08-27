using System.Collections;
using UnityEngine;

namespace CommsRadioAPI;

public class CommsRadioUtility
{
	private CommsRadioMode target;

	public CommsRadioUtility(CommsRadioMode mode)
	{
		target = mode;
	}

	public Coroutine StartCoroutine(IEnumerator routine)
	{
		return target.StartCoroutine(routine);
	}

	public void StopCoroutine(Coroutine routine)
	{
		target.StopCoroutine(routine);
	}

	/// <summary>
	/// Play a vanilla sound from the Comms Radio.
	/// </summary>
	/// <param name="sound">The vanilla sound to play.</param>
	/// <param name="source">Optional.<br/>Location of the sound source for spacial audio.</param>
	public void PlaySound(CommsSound sound, Transform? source = null)
	{
		ControllerAPI.PlaySound(sound, source ?? target.signalOrigin ?? target.gameObject.transform);
	}

	/// <summary>
	/// Play a vanilla sound from a locomotive or wagon.
	/// </summary>
	/// <param name="sound">The vanilla vehicle sound to play.</param>
	/// <param name="source">The locomotive or wagon that will be used as the sound source for spacial audio.</param>
	/// <param name="parentToWorld"></param>
	public static void PlayVehicleSound(VehicleSound sound, TrainCar source, bool parentToWorld = false)
	{
		ControllerAPI.PlayVehicleSound(sound, source, parentToWorld);
	}
}
