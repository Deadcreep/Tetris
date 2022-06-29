using Inputs;
using UnityEngine;

public class PauseController
{
	public bool IsPaused { get; private set; } = true;
	private IInputProvider _inputProvider;
	private Game _game;

	public PauseController(IInputProvider inputProvider, Game game)
	{
		_inputProvider = inputProvider;
		_game = game;
	}

	public void TogglePause()
	{
		if (!IsPaused)
			Pause();
		else
			Unpause();
	}

	public void Pause()
	{
		_game.enabled = false;
		_inputProvider.Pause();
		IsPaused = true;
	}

	public void Unpause()
	{
		_game.enabled = true;
		_inputProvider.Unpause();
		IsPaused = false;
	}
}