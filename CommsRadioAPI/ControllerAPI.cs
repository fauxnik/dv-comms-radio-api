using DV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

namespace CommsRadioAPI;

/// <summary>
/// A conveience API for the <c>CommsRadioController</c> class.
/// </summary>
public static class ControllerAPI
{
	/// <summary>
	/// Subscribe to know when the Comms Radio is ready to receive new modes.
	/// </summary>
	public static Action? Ready;

	/// <summary>
	/// Get a vanilla Comms Radio mode from the <c>CommsRadioController</c>.
	/// </summary>
	/// <param name="mode">The mode to retrieve.</param>
	/// <returns>The requeted vanilla Comms Radio mode.</returns>
	public static ICommsRadioMode? GetVanillaMode(VanillaMode mode)
	{
		CommsRadioController? controller = Accessor.CommsRadioController;
		if (controller == null) { return default; }
		return mode switch
		{
			VanillaMode.Rerail => controller.rerailControl,
			VanillaMode.Junction => controller.switchControl,
			VanillaMode.Clear => controller.deleteControl,
			VanillaMode.SummonCrewVehicle => controller.crewVehicleControl,
			VanillaMode.Spawn => controller.carSpawnerControl,
			VanillaMode.LoadCargo => controller.cargoLoaderControl,
			VanillaMode.Damage => controller.carDamageControl,
			VanillaMode.LED => controller.commsRadioLight,
			_ => default,
		};
	}

	internal static CommsRadioMode AddMode(Predicate<ICommsRadioMode>? insertBeforeTest)
	{
		CommsRadioController? controller = Accessor.CommsRadioController;
		if (controller == null) { throw new InvalidOperationException("CommsRadioController should not be null."); }

		FieldInfo allModesFieldInfo = typeof(CommsRadioController).GetField("allModes", BindingFlags.NonPublic | BindingFlags.Instance);
		List<ICommsRadioMode>? allModes = allModesFieldInfo.GetValue(controller) as List<ICommsRadioMode>;
		if (allModes == null) { throw new NoNullAllowedException("Couldn't retrieve allModes from CommsRadioController."); }

		CommsRadioMode mode = controller.gameObject.AddComponent<CommsRadioMode>();
		int spawnerIndex = allModes.FindIndex(insertBeforeTest ?? (mode => false));
		if (spawnerIndex != -1) { allModes.Insert(spawnerIndex, mode); }
		else { allModes.Add(mode); }
		controller.ReactivateModes();

		return mode;
	}

	internal static void PlaySound(VanillaSoundCommsRadio sound, Transform source)
	{
		AudioClip? audio = sound switch
		{
			VanillaSoundCommsRadio.Confirm => Accessor.ConfirmSound,
			VanillaSoundCommsRadio.Cancel => Accessor.CancelSound,
			VanillaSoundCommsRadio.Warning => Accessor.WarningSound,
			VanillaSoundCommsRadio.Switch => Accessor.SwitchSound,
			VanillaSoundCommsRadio.ModeEnter => Accessor.ModeEnterSound,
			VanillaSoundCommsRadio.HoverOver => Accessor.HoverOverSound,
			VanillaSoundCommsRadio.MoneyRemoved => Accessor.MoneyRemovedSound,
			_ => null,
		};
		if (audio == null)
		{
			Main.LogWarning($"Can't find audio for {sound}. No sound will be played.");
			return;
		}

		if (audio == Accessor.MoneyRemovedSound)
		{
			audio.Play2D(1f, false);
			return;
		}

		CommsRadioController.PlayAudioFromRadio(audio, source);
	}

	internal static void PlayVehicleSound(VanillaSoundVehicle sound, TrainCar source, bool parentToWorld = false)
	{
		AudioClip? audio = sound switch
		{
			VanillaSoundVehicle.SpawnVehicle => Accessor.SpawnVehicleSound,
			VanillaSoundVehicle.SelectVehicle => Accessor.SelectVehicleSound,
			VanillaSoundVehicle.RemoveVehicle => Accessor.RemoveVehicleSound,
			_ => null,
		};
		if (audio == null)
		{
			Main.LogWarning($"Can't find audio for {sound}. No sound will be played.");
			return;
		}

		CommsRadioController.PlayAudioFromCar(audio, source, parentToWorld);
	}

	internal static Material? GetMaterial(VanillaMaterial material)
	{
		return material switch
		{
			VanillaMaterial.Valid => Accessor.ValidMaterial,
			VanillaMaterial.Invalid => Accessor.InvalidMaterial,
			_ => null,
		};
	}
}
