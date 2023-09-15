using DV;
using HarmonyLib;
using UnityEngine;

namespace CommsRadioAPI;

internal static class Accessor
{
	internal static CommsRadioController? CommsRadioController { get; private set; }

	internal static CommsRadioDisplay? CommsRadioDisplay { get; private set; }
	internal static ArrowLCD? CommsRadioArrow { get; private set; }
	internal static Light? CommsRadioLight { get; private set;}

	internal static Material? ValidMaterial { get; private set; }
	internal static Material? InvalidMaterial { get; private set; }

	internal static AudioClip? ConfirmSound { get; private set; }
	internal static AudioClip? CancelSound { get; private set; }
	internal static AudioClip? WarningSound { get; private set; }
	internal static AudioClip? SwitchSound { get; private set; }
	internal static AudioClip? ModeEnterSound { get; private set; }
	internal static AudioClip? HoverOverSound { get; private set; }
	internal static AudioClip? MoneyRemovedSound { get; private set; }

	internal static AudioClip? SpawnVehicleSound { get; private set; }
	internal static AudioClip? SelectVehicleSound { get; private set; }
	internal static AudioClip? RemoveVehicleSound { get; private set; }

	[HarmonyPatch(typeof(CommsRadioController), "Awake")]
	internal static class AcquireResourcesPatch
	{
		static void Postfix(CommsRadioController __instance)
		{
			CommsRadioController = __instance;

			CommsRadioCrewVehicle crewVehicleControl = CommsRadioController.crewVehicleControl;
			ConfirmSound = crewVehicleControl.confirmSound;
			CancelSound = crewVehicleControl.cancelSound;
			WarningSound = crewVehicleControl.warningSound;
			ModeEnterSound = crewVehicleControl.spawnModeEnterSound;
			HoverOverSound = crewVehicleControl.hoverOverCar;
			MoneyRemovedSound = crewVehicleControl.moneyRemovedSound;
			SpawnVehicleSound = crewVehicleControl.spawnVehicleSound;

			CommsRadioLight lightControl = CommsRadioController.commsRadioLight;
			SwitchSound = lightControl.switchSound;

			CommsRadioCarDeleter deleteControl = CommsRadioController.deleteControl;
			SelectVehicleSound = deleteControl.selectedCarSound;
			RemoveVehicleSound = deleteControl.removeCarSound;

			CommsRadioDisplay = crewVehicleControl.display;
			CommsRadioArrow = crewVehicleControl.lcdArrow;
			CommsRadioLight = lightControl.light;

			ValidMaterial = crewVehicleControl.validMaterial;
			InvalidMaterial = crewVehicleControl.invalidMaterial;

			ControllerAPI.Ready?.Invoke();
		}
	}
}
