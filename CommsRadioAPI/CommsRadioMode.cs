using DV;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CommsRadioAPI;

public class CommsRadioMode : MonoBehaviour, ICommsRadioMode
{
	private CommsRadioUtility proxy;
	private Color laserColor = Color.white;
	private StateActionUpdateHandler? startingState;
	private StateActionUpdateHandler? activeState;
	internal Transform? signalOrigin;

	/// <summary>
	/// Create a new mode entry in the Comms Radio.
	/// </summary>
	/// <param name="startingState">The starting state/action/update handler. Its button behavior must be <c>ButtonBehaviourType.Regular</c>.</param>
	/// <param name="laserColor">The color of the laser beam.</param>
	/// <param name="insertBeforeTest">Optional.<br/>Return true for the mode the new mode will be inserted before.<br/>If none return true or no predicate is given, the mode will be inserted at the end of the list.</param>
	/// <returns>The Comms Radio mode that was created.</returns>
	/// <exception cref="ArgumentException">Throws an exception if the provided starting state has a button behaviour other than <c>ButtonBehaviourType.Regular</c>.</exception>
	public static CommsRadioMode Create(StateActionUpdateHandler startingState, Color? laserColor, Predicate<ICommsRadioMode>? insertBeforeTest)
	{
		if (startingState.state.behaviour != ButtonBehaviourType.Regular) { throw new ArgumentException($"Starting state must have a button beviour type of Regular, but it has {startingState.state.behaviour}."); }
		CommsRadioMode mode = ControllerAPI.AddMode(insertBeforeTest);
		mode.laserColor = laserColor ?? Color.white;
		mode.startingState = startingState;
		mode.activeState = startingState;
		mode.display = Accessor.CommsRadioDisplay;
		if (mode.display == null) { Main.LogError("Can't find the Comms Radio LCD display."); }
		mode.lcdArrow = Accessor.CommsRadioArrow;
		if (mode.lcdArrow == null) { Main.LogError("Can't find the Comms Radio LCD arrow."); }
		mode.ledLight = Accessor.CommsRadioLight;
		if (mode.ledLight == null) { Main.LogError("Can't find the Comms Radio LED light."); }
		return mode;
	}

	public CommsRadioMode()
	{
		proxy = new CommsRadioUtility(this);
	}

	[DoesNotReturn]
	private void ThrowNullActiveState()
	{
		throw new InvalidOperationException("Active state should never be null. Was this CommsRadioMode component created without using CommsRadioMode.Create()?");
	}
	[DoesNotReturn]
	private void ThrowNullStartingState()
	{
		throw new InvalidOperationException("Starting state should never be null. Was this CommsRadioMode component created without using CommsRadioMode.Create()?");
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

	private void TransitionToState(StateActionUpdateHandler nextState)
	{
		if (activeState == nextState) { return; }
		activeState?.OnLeave(proxy);
		StopAllCoroutines();
		nextState.OnEnter(proxy);
		activeState = nextState;
		ApplyState();
	}

	private CommsRadioDisplay? display;
	private ArrowLCD? lcdArrow;
	private Light? ledLight;

	public void Awake()
	{
		signalOrigin = transform;
	}

	public void Enable()
	{
		activeState?.OnEnter(proxy);
	}

	public void Disable()
	{
		activeState?.OnLeave(proxy);
		StopAllCoroutines();
	}

	public void OverrideSignalOrigin(Transform transform)
	{
		signalOrigin = transform;
	}

	public void OnUse()
	{
		Main.Log("Comms Radio activated.");
		if (activeState == null) { ThrowNullActiveState(); }
		var nextState = activeState.OnAction(proxy, InputAction.Activate);
		if (nextState == activeState) { throw new InvalidOperationException("The Activate action must change the state of the Comms Radio."); }
		TransitionToState(nextState);
	}

	public void OnUpdate()
	{
		if (activeState == null) { ThrowNullActiveState(); }
		TransitionToState(activeState.OnUpdate(proxy));
	}

	public ButtonBehaviourType ButtonBehaviour { get; private set; }

	public bool ButtonACustomAction()
	{
		Main.Log("Button A pressed.");
		if (activeState == null) { ThrowNullActiveState(); }
		// TODO: is ButtonA actually the up button?
		TransitionToState(activeState.OnAction(proxy, InputAction.Up));
		return true;
	}

	public bool ButtonBCustomAction()
	{
		Main.Log("Button B pressed.");
		if (activeState == null) { ThrowNullActiveState(); }
		// TODO: is ButtonB actually the down button?
		TransitionToState(activeState.OnAction(proxy, InputAction.Down));
		return true;
	}

	public void SetStartingDisplay()
	{
		if (startingState == null) { ThrowNullStartingState(); }
		TransitionToState(startingState);
	}

	public Color GetLaserBeamColor() { return laserColor; }
}
