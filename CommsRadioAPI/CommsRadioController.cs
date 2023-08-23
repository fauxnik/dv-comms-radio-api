using DV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

namespace CommsRadioAPI;

public static class CommsRadioController
{
	internal static CommsRadioMode AddMode(Predicate<ICommsRadioMode>? insertBeforeTest)
	{
		DV.CommsRadioController? controller = Accessor.CommsRadioController;
		if (controller == null) { throw new InvalidOperationException(); }

		FieldInfo allModesFieldInfo = typeof(DV.CommsRadioController).GetField("allModes", BindingFlags.NonPublic | BindingFlags.Instance);
		List<ICommsRadioMode>? allModes = allModesFieldInfo.GetValue(controller) as List<ICommsRadioMode>;
		if (allModes == null) { throw new NoNullAllowedException(); }

		CommsRadioMode mode = controller.gameObject.AddComponent<CommsRadioMode>();
		int spawnerIndex = allModes.FindIndex(insertBeforeTest ?? (mode => false));
		if (spawnerIndex != -1) { allModes.Insert(spawnerIndex, mode); }
		else { allModes.Add(mode); }
		controller.ReactivateModes();

		return mode;
	}

	public static void PlaySound(CommsRadioSound sound, Transform source)
	{
		AudioClip? audio = sound switch
		{
			CommsRadioSound.Confirm => Accessor.ConfirmSound,
			CommsRadioSound.Cancel => Accessor.CancelSound,
			CommsRadioSound.Warning => Accessor.WarningSound,
			CommsRadioSound.Switch => Accessor.SwitchSound,
			CommsRadioSound.ModeEnter => Accessor.ModeEnterSound,
			CommsRadioSound.HoverOver => Accessor.HoverOverSound,
			CommsRadioSound.MoneyRemoved => Accessor.MoneyRemovedSound,
			_ => null,
		};
		if (audio == null)
		{
			// TODO: log warning
			return;
		}

		DV.CommsRadioController.PlayAudioFromRadio(audio, source);
	}

	public static void PlayVehicleSound(VehicleSound sound, TrainCar source, bool parentToWorld = false)
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
			// TODO: log warning
			return;
		}

		DV.CommsRadioController.PlayAudioFromCar(audio, source, parentToWorld);
	}
}
