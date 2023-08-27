using DV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

namespace CommsRadioAPI;

public static class ControllerAPI
{
	/// <summary>
	/// Get a vanilla mode from the CommsRadioController.
	/// </summary>
	/// <param name="mode">The mode to retrieve.</param>
	/// <returns></returns>
	public static ICommsRadioMode? GetVanillaMode(VanillaModes mode)
	{
		CommsRadioController? controller = Accessor.CommsRadioController;
		if (controller == null) { return default; }
		return mode switch
		{
			VanillaModes.Rerail => controller.rerailControl,
			VanillaModes.Junction => controller.switchControl,
			VanillaModes.Clear => controller.deleteControl,
			VanillaModes.SummonCrewVehicle => controller.crewVehicleControl,
			VanillaModes.Spawn => controller.carSpawnerControl,
			VanillaModes.LoadCargo => controller.cargoLoaderControl,
			VanillaModes.Damage => controller.carDamageControl,
			VanillaModes.LED => controller.commsRadioLight,
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

	internal static void PlaySound(CommsSound sound, Transform source)
	{
		AudioClip? audio = sound switch
		{
			CommsSound.Confirm => Accessor.ConfirmSound,
			CommsSound.Cancel => Accessor.CancelSound,
			CommsSound.Warning => Accessor.WarningSound,
			CommsSound.Switch => Accessor.SwitchSound,
			CommsSound.ModeEnter => Accessor.ModeEnterSound,
			CommsSound.HoverOver => Accessor.HoverOverSound,
			CommsSound.MoneyRemoved => Accessor.MoneyRemovedSound,
			_ => null,
		};
		if (audio == null)
		{
			Main.LogWarning($"Can't find audio for {sound}. No sound will be played.");
			return;
		}

		CommsRadioController.PlayAudioFromRadio(audio, source);
	}

	internal static void PlayVehicleSound(VehicleSound sound, TrainCar source, bool parentToWorld = false)
	{
		AudioClip? audio = sound switch
		{
			VehicleSound.SpawnVehicle => Accessor.SpawnVehicleSound,
			VehicleSound.SelectVehicle => Accessor.SelectVehicleSound,
			VehicleSound.RemoveVehicle => Accessor.RemoveVehicleSound,
			_ => null,
		};
		if (audio == null)
		{
			Main.LogWarning($"Can't find audio for {sound}. No sound will be played.");
			return;
		}

		CommsRadioController.PlayAudioFromCar(audio, source, parentToWorld);
	}
}