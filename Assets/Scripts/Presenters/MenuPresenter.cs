using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPresenter : PresenterBehaviour<Menu>
{
	[SerializeField] private GameObject _finalPanel;
	[SerializeField] private TextMeshProUGUI _finalScoreField;
	[Space]
	[SerializeField] private GameObject _menuPanel;
	[SerializeField] private Button _startButton;
	[SerializeField] private Button _openButton;
	[SerializeField] private Button _quitButton;
	[Space]
	[SerializeField] private List<CanvasGroup> _objectsToHide;

	protected override void OnInject()
	{
		_startButton.onClick.AddListener(FirstStart);
		_openButton.onClick.AddListener(Model.ChangeState);
		_quitButton.onClick.AddListener(Model.Quit);
		Model.OnChangedState += UpdateView;
		Model.OnGameEnded += ShowFinalMessage;
		UpdateView(true);
	}

	protected override void OnRemove()
	{
		_startButton.onClick.RemoveAllListeners();
		_openButton.onClick.RemoveListener(Model.ChangeState);
		_quitButton.onClick.RemoveListener(Model.Quit);
		Model.OnChangedState -= UpdateView;
		Model.OnGameEnded -= ShowFinalMessage;
	}

	private void UpdateView(bool state)
	{
		foreach (var item in _objectsToHide)
		{
			item.alpha = state ? 0 : 1;
		}
		_menuPanel.gameObject.SetActive(state);
	}

	private void FirstStart()
	{
		_startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
		_startButton.onClick.RemoveListener(FirstStart);
		_startButton.onClick.AddListener(Model.Start);
		Model.Start();
	}

	private void ShowFinalMessage(int points)
	{
		UpdateView(true);
		_finalScoreField.text = points.ToString("000000");
		_finalPanel.SetActive(true);
	}
}