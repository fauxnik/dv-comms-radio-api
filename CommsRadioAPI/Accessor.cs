using UnityEngine;

namespace CommsRadioAPI;

internal static class Accessor
{
	internal static DV.CommsRadioController? CommsRadioController { get; private set; }

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
}
