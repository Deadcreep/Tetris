using System;
using UnityEngine;
using UnityEngine.UI;

public class PausePresenter : PresenterBehaviour<PauseController>
{
	[SerializeField] private Button _pauseButton;

	protected override void OnInject()
	{
		_pauseButton.onClick.AddListener(TogglePause);
	}

	private void TogglePause()
	{
		Model.TogglePause();
	}

	protected override void OnRemove()
	{
		_pauseButton.onClick.RemoveListener(TogglePause);
	}
}

