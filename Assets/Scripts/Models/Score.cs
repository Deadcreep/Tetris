using System;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
	public int Points
	{
		get => _points;
		private set
		{
			if (_points != value)
			{
				_points = value;
				OnScoreIncreased?.Invoke(_points);
			}
		}
	}

	public IReadOnlyList<int> HighScores { get => _highScores; }

	[SerializeField] private int _points;
	[SerializeField] private List<int> scorePerDestroyedRows = new List<int>();
	[SerializeField] private int[] _highScores = new int[10];

	public event Action<int> OnScoreIncreased;

	public event Action OnHighScoreUpdated;

	private void Awake()
	{
		LoadHighScores();
	}

	public void AddPoints(UpdadeGridInfo info)
	{
		var addedPoints = scorePerDestroyedRows[info.DestroyedRows.Count - 1];
		Points += addedPoints;
	}

	public void Clear()
	{
		Points = 0;
	}

	private void LoadHighScores()
	{
		for (int i = 0; i < _highScores.Length; i++)
		{
			_highScores[i] = PlayerPrefs.GetInt("HighScore" + i, 0);
		}
	}

	[ContextMenu("Check HighScores")]
	public void CheckHighScores()
	{
		for (int i = 0; i < _highScores.Length; i++)
		{
			if (Points > _highScores[i])
			{
				PlayerPrefs.SetInt("HighScore" + i, Points);
				if (i < _highScores.Length - 1)
				{
					for (int j = i + 1; j < _highScores.Length; j++)
					{
						PlayerPrefs.SetInt("HighScore" + j, _highScores[j - 1]);
					}
					_highScores[i] = Points;
					OnHighScoreUpdated?.Invoke();
					break;
				}
			}
		}
		LoadHighScores();
	}
}