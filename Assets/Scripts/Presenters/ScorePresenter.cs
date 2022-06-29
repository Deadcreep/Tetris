using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePresenter : PresenterBehaviour<Score>
{
	[SerializeField] private TextMeshProUGUI _scoreField;
	[SerializeField] private Button _openButton;
	[SerializeField] private Button _closeButton;
	[SerializeField] private GameObject _highScoresParent;
	private List<TextMeshProUGUI> _highScores;

	protected override void OnInject()
	{
		_highScores = _highScoresParent.GetComponentsInChildren<TextMeshProUGUI>().ToList();
		_highScores.RemoveAt(0);
		Model.OnScoreIncreased += UpdateScoreView;
		Model.OnHighScoreUpdated += UpdateHighScoresView;
		_openButton.onClick.AddListener(ShowHighScores);
		_closeButton.onClick.AddListener(HideHighScores);
		UpdateHighScoresView();
	}

	protected override void OnRemove()
	{
		Model.OnScoreIncreased -= UpdateScoreView;
		Model.OnHighScoreUpdated -= UpdateHighScoresView;
		_openButton.onClick.RemoveListener(ShowHighScores);
		_closeButton.onClick.RemoveListener(HideHighScores);
	}

	private void ShowHighScores()
	{
		_highScoresParent.SetActive(true);
	}

	private void HideHighScores()
	{
		_highScoresParent.SetActive(false);
	}

	private void UpdateHighScoresView()
	{
		for (int i = 0; i < Model.HighScores.Count; i++)
		{
			_highScores[i].text = $"{i + 1:00}. {Model.HighScores[i]:000000}";
		}
	}

	private void Start()
	{
		_scoreField.ForceMeshUpdate();
		var textSize = _scoreField.GetRenderedValues();
		var rect = GetComponent<RectTransform>();
		rect.sizeDelta = textSize + new Vector2(10, 10);
		rect.anchoredPosition = new Vector2(rect.sizeDelta.x / 2, -rect.sizeDelta.y / 2);
	}

	private void UpdateScoreView(int score)
	{
		_scoreField.text = score.ToString("000000");
	}
}