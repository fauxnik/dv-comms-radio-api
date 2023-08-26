using System;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace CommsRadioAPI;

internal static class Main
{
	public static Action<string> Log = (_) => {};
	public static readonly Action<string> LogWarning = (message) => { Log($"[Warning] {message}"); };
	public static readonly Action<string> LogError = (message) => { Log($"[Error] {message}"); };

	// Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
	private static bool Load(UnityModManager.ModEntry modEntry)
	{
		Log = modEntry.Logger.Log;

		Harmony? harmony = null;

		try
		{
			harmony = new Harmony(modEntry.Info.Id);
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			// Other plugin startup logic
		}
		catch (Exception ex)
		{
			modEntry.Logger.LogException($"Failed to load {modEntry.Info.DisplayName}:", ex);
			harmony?.UnpatchAll(modEntry.Info.Id);
			return false;
		}

		return true;
	}
}
