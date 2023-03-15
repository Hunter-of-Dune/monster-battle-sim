using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;


/// <summary>
/// Responsible for populating the panel with location buttons.
/// Sending load requests to UIMenuManager.
/// </summary>
public class UIMapSelectController : UIPopup
{
	public UnityEvent<LocationSO> LoadLocationEvent;

	[SerializeField] private MapPoolSO _pool;
	[SerializeField] private GameObject _buttonPrefab;
	[SerializeField] private GameObject _itemLayoutGroup;
	[SerializeField] private LocationSO _currentLocation;

	private void OnEnable()
	{
		_inputReader.MenuCloseEvent += Close;
		FillPanel();
	}

	void FillPanel()
	{
		foreach (MapPoolItem item in _pool.Maps)
		{
			var newButton = Instantiate(_buttonPrefab, _itemLayoutGroup.transform).GetComponent<UIMapSelectItem>();
			newButton.FillItem(this, item);
		}
	}

	public void ChangeMap(LocationSO location)
	{
		_currentLocation = location;
	}

	public void LoadLocationButton()
	{
		LoadLocationEvent.Invoke(_currentLocation);
	}

}
