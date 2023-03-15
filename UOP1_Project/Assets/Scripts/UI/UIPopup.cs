using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for closeable popup windows.
/// </summary>
public abstract class UIPopup : MonoBehaviour
{
	[SerializeField] internal InputReader _inputReader = default;
	public UnityEvent CloseEvent;

	internal void OnDisable()
	{
		_inputReader.MenuCloseEvent -= Close;
	}

	public void Close()
	{
		CloseEvent.Invoke();
	}

}
