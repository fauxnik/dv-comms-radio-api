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
}
