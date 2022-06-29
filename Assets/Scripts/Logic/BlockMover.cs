using Inputs;
using System;
using UnityEngine;

public class BlockMover : IDisposable
{
	private float _defaultSpeed;
	private float _fastSpeed;
	private float _currentSpeed;

	private IInputProvider _inputProvider;
	private GridChecker _gridChecker;
	private Figure _figure;	

	public event Action<Figure> OnFigureStopped;

	public BlockMover(float defaultSpeed, float fastSpeed, IInputProvider inputProvider, GridChecker gridChecker)
	{
		_defaultSpeed = _currentSpeed = defaultSpeed;
		_fastSpeed = fastSpeed;
		_inputProvider = inputProvider;
		_gridChecker = gridChecker;

		_inputProvider.OnIncreaseSpeed += IncreaseSpeed;
		_inputProvider.OnDecreaseSpeed += DecreaseSpeed;
		_inputProvider.OnMoveLeft += MoveLeft;
		_inputProvider.OnMoveRight += MoveRight;
	}

	private void MoveRight()
	{
		if (_figure == null)
		{
			return;
		}
		var isFree = _gridChecker.CheckSidePositions(_figure.blocks, 1);
		if (isFree)
		{
			foreach (var block in _figure.blocks)
			{
				block.X++;
			}
			_figure.center += Vector2.right;
		}
	}

	private void MoveLeft()
	{
		if (_figure == null)
		{
			return;
		}
		var isFree = _gridChecker.CheckSidePositions(_figure.blocks, -1);
		if (isFree)
		{
			foreach (var block in _figure.blocks)
			{
				block.X--;
			}
			_figure.center += Vector2.left;
		}		
	}

	public void AddFigure(Figure figure)
	{
		_figure = figure;
	}

	public void MoveDown()
	{
		if (_figure == null)
		{
			return;
		}
		if (_gridChecker.CheckBottomPositions(_figure.blocks))
		{
			foreach (var block in _figure.blocks)
			{
				block.Y -= _currentSpeed;
			}
			_figure.center += Vector2.down * _currentSpeed;			
		}
		else
		{
			OnFigureStopped?.Invoke(_figure);
		}
	}

	public void RemoveFigure()
	{
		_figure = null;
	}

	public void IncreaseSpeed()
	{
		_currentSpeed = _fastSpeed;
	}

	public void DecreaseSpeed()
	{
		_currentSpeed = _defaultSpeed;
	}

	public void Dispose()
	{
		_inputProvider.OnIncreaseSpeed -= IncreaseSpeed;
		_inputProvider.OnDecreaseSpeed -= DecreaseSpeed;

		_inputProvider.OnMoveLeft -= MoveLeft;
		_inputProvider.OnMoveRight -= MoveRight;
	}
}