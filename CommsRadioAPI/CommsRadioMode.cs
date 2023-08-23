using DV;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CommsRadioAPI;

public class CommsRadioMode : MonoBehaviour, ICommsRadioMode
{
	private Color laserColor = Color.white;
	private StateActionUpdateHandler? startingState;
	private StateActionUpdateHandler? activeState;
	private Transform? signalOrigin;

	public static CommsRadioMode Create(StateActionUpdateHandler startingState, Color? laserColor, Predicate<ICommsRadioMode>? insertBeforeTest)
	{
		CommsRadioMode mode = CommsRadioController.AddMode(insertBeforeTest);
		mode.laserColor = laserColor ?? Color.white;
		mode.startingState = startingState;
		mode.activeState = startingState;
		return mode;
	}

	[DoesNotReturn]
	private void ThrowNullActiveState()
	{
		throw new InvalidOperationException("Active state should never be null. Did you create a CommsRadioMode component without using CommsRadioMode.Create()?");
	}

	private void ApplyState()
	{
		if (activeState == null) { ThrowNullActiveState(); }
		CommsRadioState state = activeState.state;
		display?.SetDisplay(state.lcdTitle, state.lcdContent, state.lcdAction);
		if (state.lcdArrow == LCDArrowState.Off) { lcdArrow?.TurnOff(); }
		else { lcdArrow?.TurnOn(state.lcdArrow == LCDArrowState.Left); }
		ledLight?.gameObject.SetActive(state.led == LEDState.On);
		ButtonBehaviour = state.behaviour;
	}

	private CommsRadioDisplay? display;
	private ArrowLCD? lcdArrow;
	private Light? ledLight;

	private void Awake()
	{
		signalOrigin = transform;
	}

	public void Enable()
	{}

	public void Disable()
	{}

	public void OverrideSignalOrigin(Transform transform)
	{
		signalOrigin = transform;
	}

	public void OnUse()
	{
		if (activeState == null) { ThrowNullActiveState(); }
		activeState = activeState.OnAction(InputAction.Activate);
		ApplyState();
	}

	public void OnUpdate()
	{
		if (activeState == null) { ThrowNullActiveState(); }
		activeState = activeState.OnUpdate();
		ApplyState();
	}

	public ButtonBehaviourType ButtonBehaviour { get; private set; }

	public bool ButtonACustomAction()
	{
		if (activeState == null) { ThrowNullActiveState(); }
		// TODO: is ButtonA actually the up button?
		activeState = activeState.OnAction(InputAction.Up);
		ApplyState();
		return true;
	}

	public bool ButtonBCustomAction()
	{
		if (activeState == null) { ThrowNullActiveState(); }
		// TODO: is ButtonB actually the down button?
		activeState = activeState.OnAction(InputAction.Down);
		ApplyState();
		return true;
	}

	public void SetStartingDisplay()
	{
		activeState = startingState;
		ApplyState();
	}

	public Color GetLaserBeamColor() { return laserColor; }
}
