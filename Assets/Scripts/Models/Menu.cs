using System;
using UnityEngine;

public class Menu
{
	private PauseController _pauseController;
	private Game _game;
	private Score _score;
	private bool _isGameStarted;

	public event Action<bool> OnChangedState;
	public event Action<int> OnGameEnded;

	public Menu(PauseController pauseController, Game game, Score score)
	{
		_pauseController = pauseController;
		_game = game;
		_score = score;
		_game.OnGameEnded += HandleGameEnded;
	}

	public void Start()
	{
		if (!_isGameStarted)
		{
			_game.StartGame();
			_isGameStarted = true;
		}
		else
			_game.Restart();
		_pauseController.Unpause();
		OnChangedState?.Invoke(false);
	}

	public void ChangeState()
	{
		_pauseController.TogglePause();
		OnChangedState?.Invoke(_pauseController.IsPaused);
	}

	public void Quit()
	{
		Application.Quit();
	}

	private void HandleGameEnded()
	{
		_pauseController.Pause();
		OnGameEnded?.Invoke(_score.Points);
	}
}