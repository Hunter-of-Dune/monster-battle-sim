using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
	public InputReader inputReader = default;
	public CameraManager manager;

	private bool _isRMBPressed;
	private bool _rise;
	private Vector2 _moveVector;
	private float _inputRotateAxisX, _inputRotateAxisY;

	[SerializeField] [Range(.5f, 3f)] private float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings	
	[SerializeField] [Range(.5f, 3f)] private float _rotationMultiplier = 2f; //TODO: make this modifiable in the game settings	

	private void OnEnable()
	{
		inputReader.CameraMoveEvent += OnCameraMove;
		inputReader.MoveEvent += OnMove;
		inputReader.EnableMouseControlCameraEvent += OnEnableMouseControlCamera;
		inputReader.DisableMouseControlCameraEvent += OnDisableMouseControlCamera;
		inputReader.JumpEvent += OnRise;
		inputReader.JumpCanceledEvent += OnRiseEnd;

		inputReader.EnableGameplayInput();
	}

		private void OnDisable()
	{
		inputReader.CameraMoveEvent -= OnCameraMove;
		inputReader.MoveEvent -= OnMove;
		inputReader.EnableMouseControlCameraEvent -= OnEnableMouseControlCamera;
		inputReader.DisableMouseControlCameraEvent -= OnDisableMouseControlCamera;
		inputReader.JumpEvent -= OnRise;
		inputReader.JumpCanceledEvent -= OnRiseEnd;
	}

	private void Update()
	{
		bool moved = _inputRotateAxisX != 0.0f || _inputRotateAxisY != 0.0f || _moveVector != Vector2.zero;
		if (moved)
		{
			float rotationX = transform.localEulerAngles.x;
			float newRotationY = transform.localEulerAngles.y + _inputRotateAxisX;

			// Weird clamping code due to weird Euler angle mapping...
			float newRotationX = (rotationX - _inputRotateAxisY);
			if (rotationX <= 90.0f && newRotationX >= 0.0f)
				newRotationX = Mathf.Clamp(newRotationX, 0.0f, 90.0f);
			if (rotationX >= 270.0f)
				newRotationX = Mathf.Clamp(newRotationX, 270.0f, 360.0f);

			transform.localRotation = Quaternion.Euler(newRotationX, newRotationY, transform.localEulerAngles.z);

			transform.Translate(_moveVector.x, 0, _moveVector.y, Space.Self);
			
		}

		if(_rise)
			transform.Translate(Vector3.up * _speedMultiplier, Space.World);
	}

	private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
	{
		if (manager.CameraMovementLock)
			return;

		if (isDeviceMouse && !_isRMBPressed)
			return;

		//Using a "fixed delta time" if the device is mouse,
		//since for the mouse we don't have to account for frame duration
		float deviceMultiplier = isDeviceMouse ? 0.02f : Time.deltaTime;

		_inputRotateAxisX = cameraMovement.x * deviceMultiplier * _rotationMultiplier;
		_inputRotateAxisY = cameraMovement.y * deviceMultiplier * _rotationMultiplier;
	}

	private void OnMove(Vector2 movement)
	{
		_moveVector = movement * _speedMultiplier;
	}

	private void OnEnableMouseControlCamera()
	{
		_isRMBPressed = true;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		StartCoroutine(DisableMouseControlForFrame());
	}

	IEnumerator DisableMouseControlForFrame()
	{
		manager.CameraMovementLock = true;
		yield return new WaitForEndOfFrame();
		manager.CameraMovementLock = false;
	}

	private void OnDisableMouseControlCamera()
	{
		_isRMBPressed = false;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// when mouse control is disabled, the input needs to be cleared
		// or the last frame's input will 'stick' until the action is invoked again
		_inputRotateAxisX = 0.0f;
		_inputRotateAxisY = 0.0f;
	}

	private void OnRise()
	{
		_rise = true;
	}

	private void OnRiseEnd()
	{
		_rise = false;
	}

}
