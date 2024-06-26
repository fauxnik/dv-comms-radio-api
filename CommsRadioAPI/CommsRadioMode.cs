using DV;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CommsRadioAPI;

/// <summary>
/// Contains the method for creating new modes for the Comms Radio.
/// </summary>
public class CommsRadioMode : MonoBehaviour, ICommsRadioMode
{
	private CommsRadioUtility proxy;
	private Color laserColor = Color.white;
	private AStateBehaviour? startingState;
	private AStateBehaviour? activeState;
	internal Transform? signalOrigin;

	/// <summary>
	/// Create a new mode in the Comms Radio.
	/// </summary>
	/// <param name="startingState">The starting state/action/update handler. Its button behaviour must be <c>ButtonBehaviourType.Regular</c>.</param>
	/// <param name="laserColor"><em>Optional.</em><br/>The color of the laser beam.</param>
	/// <param name="insertBefore"><em>Optional.</em><br/>Return true for the mode the new mode will be inserted before.<br/>If none return true or no predicate is given, the mode will be inserted at the end of the list.</param>
	/// <returns>The Comms Radio mode that was created.</returns>
	/// <exception cref="ArgumentException">Throws an exception if the provided starting state has a button behaviour other than <c>ButtonBehaviourType.Regular</c>.</exception>
	public static CommsRadioMode Create(AStateBehaviour startingState, Color? laserColor = null, Predicate<ICommsRadioMode>? insertBefore = null)
	{
		if (startingState.state.behaviour != ButtonBehaviourType.Regular) { throw new ArgumentException($"Starting state must have a button beviour type of Regular, but it has {startingState.state.behaviour}."); }
		CommsRadioMode mode = ControllerAPI.AddMode(insertBefore);
		mode.proxy ??= new CommsRadioUtility(mode); // not sure if GameObject.AddComponent will call the class constructor
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

	private CommsRadioMode()
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

	private bool TransitionToState(AStateBehaviour nextState)
	{
		if (activeState == nextState) { return false; }
		activeState?.OnLeave(proxy, nextState);
		// We can't automatically stop all coroutines here, as some behaviours may need to wait longer than their lifespan to take certain actions.
		// TODO: Can we establish a timeout for coroutines?
		nextState.OnEnter(proxy, activeState);
		activeState = nextState;
		ApplyState();
		return true;
	}

	private CommsRadioDisplay? display;
	private ArrowLCD? lcdArrow;
	private Light? ledLight;

	// Unity lifecycle methods don't need to be public
	//   ↪ https://discussions.unity.com/t/does-the-access-modifier-of-start-awake-onenable-make-a-difference-to-unity/147910/3
	private void Awake()
	{
		signalOrigin ??= transform;
	}

	/// <summary>
	/// <em>Don't use.</em>
	/// Override <c>AStateBehaviour.OnEnter</c> instead.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public void Enable()
	{
		activeState = startingState;
		activeState?.OnEnter(proxy, null);
		ApplyState();
	}

	/// <summary>
	/// <em>Don't use.</em>
	/// Override <c>AStateBehaviour.OnLeave</c> instead.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public void Disable()
	{
		activeState?.OnLeave(proxy, null);
		// We can't automatically stop all coroutines here, as some behaviours may need to wait longer than their lifespan to take certain actions.
		// TODO: Can we establish a timeout for coroutines?
	}

	/// <summary>
	/// Override the signal origin.
	/// <em>(TODO: What does this mean?)</em><br/>
	/// By default, the signal origin will be the transform of the <c>CommsRadioController</c> game object.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	/// <param name="transform"></param>
	public void OverrideSignalOrigin(Transform transform)
	{
		signalOrigin = transform;
	}

	/// <summary>
	/// <em>Don't use.</em>
	/// Implement <c>AStateBehaviour.OnAction</c> instead.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public void OnUse()
	{
		Main.Log("Comms Radio activated.");
		if (activeState == null) { ThrowNullActiveState(); }
		var nextState = activeState.OnAction(proxy, InputAction.Activate);
		if (nextState == activeState) { throw new InvalidOperationException("The Activate action must change the state of the Comms Radio."); }
		TransitionToState(nextState);
	}

	/// <summary>
	/// <em>Don't use.</em>
	/// Override <c>AStateBehaviour.OnUpdate</c> instead.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public void OnUpdate()
	{
		if (activeState == null) { ThrowNullActiveState(); }
		TransitionToState(activeState.OnUpdate(proxy));
	}

	/// <summary>
	/// <em>Don't use.</em><br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public ButtonBehaviourType ButtonBehaviour { get; private set; }

	/// <summary>
	/// <em>Don't use.</em>
	/// Implement <c>AStateBehaviour.OnAction</c> instead.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public bool ButtonACustomAction()
	{
		Main.Log("Button A pressed.");
		if (activeState == null) { ThrowNullActiveState(); }
		// TODO: is ButtonA actually the up button?
		return TransitionToState(activeState.OnAction(proxy, InputAction.Up));
	}

	/// <summary>
	/// <em>Don't use.</em>
	/// Implement <c>AStateBehaviour.OnAction</c> instead.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public bool ButtonBCustomAction()
	{
		Main.Log("Button B pressed.");
		if (activeState == null) { ThrowNullActiveState(); }
		// TODO: is ButtonB actually the down button?
		return TransitionToState(activeState.OnAction(proxy, InputAction.Down));
	}

	/// <summary>
	/// <em>Don't use.</em>
	/// Define the <c>CommsRadioState</c> of the starting state passed to <c>CommsRadioMode.Create</c> instead.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public void SetStartingDisplay()
	{
		ApplyState();
	}

	/// <summary>
	/// Get the color of the laser beam used for this Comms Radio mode.<br/>
	/// Must be public to implement <c>ICommsRadioMode</c>.
	/// </summary>
	public Color GetLaserBeamColor() { return laserColor; }
}
