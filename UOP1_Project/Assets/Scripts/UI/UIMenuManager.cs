using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
	[SerializeField] private UIConfirmationPopup _popupPanel = default;
	[SerializeField] private UISettingsController _settingsPanel = default;
	[SerializeField] private UICredits _creditsPanel = default;
	[SerializeField] private UIMainMenu _mainMenuPanel = default;
	[SerializeField] private UIMapSelectController _mapSelectPanel = default;

	[SerializeField] private SaveSystem _saveSystem = default;

	[SerializeField] private InputReader _inputReader = default;


	[Header("Broadcasting on")]
	[SerializeField]
	private VoidEventChannelSO _startNewGameEvent = default;
	[SerializeField]
	private VoidEventChannelSO _continueGameEvent = default;
	[SerializeField]
	private LoadEventChannelSO _loadLevelEvent = default;


	private bool _hasSaveData;

	private IEnumerator Start()
	{
		_inputReader.EnableMenuInput();
		yield return new WaitForSeconds(0.4f); //waiting time for all scenes to be loaded 
		SetMenuScreen();
		_mapSelectPanel.LoadLocationEvent.AddListener(LoadLevel);
	}

	void SetMenuScreen()
	{
		_hasSaveData = _saveSystem.LoadSaveDataFromDisk();
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
		_mainMenuPanel.ContinueButtonAction += _continueGameEvent.RaiseEvent;
		_mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
		_mainMenuPanel.MapSelectButtonAction += OpenMapSelectScreen;
		_mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
		_mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
		_mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;
	}

	void ButtonStartNewGameClicked()
	{
		if (!_hasSaveData)
		{
			ConfirmStartNewGame();

		}
		else
		{
			ShowStartNewGameConfirmationPopup();

		}

	}

	void ConfirmStartNewGame()
	{
		_startNewGameEvent.RaiseEvent();
	}

	void ShowStartNewGameConfirmationPopup()
	{
		_popupPanel.ConfirmationResponseAction += StartNewGamePopupResponse;
		_popupPanel.CloseEvent.AddListener(HidePopup);

		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.NewGame);

	}

	void StartNewGamePopupResponse(bool startNewGameConfirmed)
	{

		_popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
		_popupPanel.CloseEvent.RemoveListener(HidePopup);
		_popupPanel.gameObject.SetActive(false);

		if (startNewGameConfirmed)
		{
			ConfirmStartNewGame();
		}
		else
		{
			_continueGameEvent.RaiseEvent();
		}

		_mainMenuPanel.SetMenuScreen(_hasSaveData);

	}

	void HidePopup()
	{
		_popupPanel.CloseEvent.RemoveListener(HidePopup);
		_popupPanel.gameObject.SetActive(false);
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
	}
	void LoadLevel(LocationSO level)
	{
		_loadLevelEvent.OnLoadingRequested(level, true, true);
	}
	public void OpenSettingsScreen()
	{
		_settingsPanel.gameObject.SetActive(true);
		_settingsPanel.CloseEvent.AddListener(CloseSettingsScreen);
	}
	public void CloseSettingsScreen()
	{
		_settingsPanel.CloseEvent.RemoveListener(CloseSettingsScreen);
		_settingsPanel.gameObject.SetActive(false);
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
	}
	public void OpenCreditsScreen()
	{
		_creditsPanel.gameObject.SetActive(true);
		_creditsPanel.CloseEvent.AddListener(CloseCreditsScreen);
	}
	public void CloseCreditsScreen()
	{
		_creditsPanel.CloseEvent.RemoveListener(CloseCreditsScreen);
		_creditsPanel.gameObject.SetActive(false);
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
	}

	public void OpenMapSelectScreen()
	{
		_mapSelectPanel.gameObject.SetActive(true);
		_mapSelectPanel.CloseEvent.AddListener(CloseMapSelectScreen);
	}
	public void CloseMapSelectScreen()
	{
		_mapSelectPanel.CloseEvent.RemoveListener(CloseSettingsScreen);
		_mapSelectPanel.gameObject.SetActive(false);
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
	}

	public void ShowExitConfirmationPopup()
	{
		_popupPanel.ConfirmationResponseAction += HideExitConfirmationPopup;
		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.Quit);
	}
	void HideExitConfirmationPopup(bool quitConfirmed)
	{
		_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
		_popupPanel.gameObject.SetActive(false);
		if (quitConfirmed)
		{
			Application.Quit();
		}
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
	}
	private void OnDestroy()
	{
		_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
		_popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
		_mapSelectPanel.LoadLocationEvent.RemoveListener(LoadLevel);
	}


}
