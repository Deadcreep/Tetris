using Inputs;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public FigureVariant NextFigure { get; private set; }
	[SerializeField] private Score _score;
	[SerializeField] private float _defaultSpeed;
	[SerializeField] private float _fastSpeed;

	private Vector2Int _startPos;
	private IRandomizer _randomizer;
	private IInputProvider _inputProvider;
	private BlockMover _blockMover;
	private BlockRotator _blockRotator;
	private GridChecker GridChecker;
	private Figure _currentFigure;

	public event Action<Figure> OnFigureCreated;

	public event Action<Figure> OnFigureStopped;

	public event Action<UpdadeGridInfo> OnGridUpdated;

	public event Action OnGameEnded;

	public event Action OnRestarted;

	public void Init(IRandomizer randomizer, IInputProvider inputProvider)
	{
		this._inputProvider = inputProvider;
		_randomizer = randomizer;
		GridChecker = new GridChecker();
		_blockMover = new BlockMover(_defaultSpeed * Time.fixedDeltaTime, _fastSpeed * Time.fixedDeltaTime, _inputProvider, GridChecker);
		_blockRotator = new BlockRotator(GridChecker);
		_startPos = new Vector2Int((GameGrid.Size.x / 2), GameGrid.Size.y);
		_blockMover.OnFigureStopped += HandleFigureStopped;
		_inputProvider.OnRotate += RotateFigure;		
	}

	private void OnDestroy()
	{
		_blockMover.Dispose();
		_blockMover.OnFigureStopped -= HandleFigureStopped;
		_inputProvider.OnRotate -= RotateFigure;		
	}

	private void FixedUpdate()
	{
		_blockMover.MoveDown();
	}

	public void StartGame()
	{
		NextFigure = _randomizer.Get();
		CreateFigure();
	}

	[ContextMenu("CreateFigure")]
	public void CreateFigure()
	{
		var variant = NextFigure;
		Vector3 startPos = new Vector3(_startPos.x, _startPos.y + variant.size.y);
		var figure = new Figure(variant.isSymmetric);
		figure.size = variant.size;
		figure.center = startPos - new Vector3(figure.size.x / 2, figure.size.y / 2);
		List<Block> blocks = new List<Block>();
		for (int y = 0; y < variant.size.y; y++)
		{
			for (int x = 0; x < variant.size.x; x++)
			{
				var val = variant[x, y];
				if (val)
				{
					Block block = new Block()
					{
						X = Mathf.CeilToInt(startPos.x - x),
						Y = startPos.y - y
					};
					blocks.Add(block);
				}
			}
		}
		NextFigure = _randomizer.Get();
		figure.blocks = blocks;
		_blockMover.AddFigure(figure);
		_currentFigure = figure;
		OnFigureCreated?.Invoke(figure);
	}

	public void Restart()
	{
		for (int y = 0; y < GameGrid.Size.y; y++)
		{
			if (GameGrid.GetRowStatus(y))
			{
				break;
			}
			GameGrid.SetRowStatus(y, true);
		}
		_score.Clear();
		OnRestarted?.Invoke();
		StartGame();
	}

	private void EndGame()
	{
		_score.CheckHighScores();
		_blockMover.RemoveFigure();		
		OnGameEnded?.Invoke();
	}

	private void HandleFigureStopped(Figure figure)
	{
		foreach (var block in figure.blocks)
		{
			var rounded = Mathf.RoundToInt(block.Y);
			if (rounded >= GameGrid.Size.y)
			{
				EndGame();
				return;
			}
			block.Y = rounded;
			GameGrid.FillCell(block.X, rounded);
		}
		_blockMover.RemoveFigure();
		OnFigureStopped?.Invoke(figure);
		_currentFigure = null;
		CheckGrid();
	}

	[ContextMenu("check grid")]
	private void CheckGrid()
	{
		UpdadeGridInfo info = new UpdadeGridInfo();
		info.DestroyedRows = new List<int>();
		info.RowsUpdatedPositions = new List<(int oldY, int newY)>();
		for (int y = 0; y < GameGrid.Size.y; y++)
		{
			if (GameGrid.GetRowStatus(y))
			{
				break;
			}
			bool isFilled = true;
			for (int x = 0; x < GameGrid.Size.x; x++)
			{
				isFilled &= !GameGrid.GetCellStatus(x, y);
			}
			if (isFilled)
			{
				GameGrid.SetRowStatus(y, true);
				info.DestroyedRows.Add(y);
			}
			else if (info.DestroyedRows.Count > 0)
			{
				int newY = y - info.DestroyedRows.Count;
				info.RowsUpdatedPositions.Add((y, newY));
				GameGrid.MoveRow(y, newY);
			}
		}
		if (info.DestroyedRows.Count == 0)
		{
			CreateFigure();
		}
		else
		{
			_score.AddPoints(info);
			OnGridUpdated?.Invoke(info);
		}
	}

	private void RotateFigure()
	{
		if (_currentFigure != null)
		{
			_blockRotator.RotateFigure(_currentFigure);
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			GUIStyle style = new GUIStyle();
			style.fontSize = 25;
			for (int y = 0; y < GameGrid.Size.y; y++)
			{
				Gizmos.color = GameGrid.GetRowStatus(y) ? Color.green : Color.red;
				Gizmos.DrawCube(new Vector3(-1, y, 2), Vector3.one * 0.9f);
				for (int x = 0; x < GameGrid.Size.x; x++)
				{
					Gizmos.color = GameGrid.GetCellStatus(x, y) ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
					Gizmos.DrawCube(new Vector3(x, y, 2), Vector3.one * 0.9f);
				}
			}
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(new Vector3(_startPos.x, _startPos.y, 2), Vector3.one);
			if (_currentFigure != null)
			{
				Gizmos.color = Color.black;
				foreach (var block in _currentFigure.blocks)
				{
					Vector3 pos = new Vector3(block.X, block.Y);
					Gizmos.DrawCube(pos, Vector3.one * 0.9f);
				}
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(_currentFigure.center, 0.1f);
			}
		}
	}
}