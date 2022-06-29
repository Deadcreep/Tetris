using System;
using UnityEngine;

public class GameGrid
{
	public static Vector2Int Size { get; private set; }

	private bool[,] _cells;
	private bool[] _cellStatuses;
	private static GameGrid _instance;

	private GameGrid(Vector2Int size)
	{
		Size = size;
		_cells = new bool[size.x, size.y];
		_cellStatuses = new bool[size.y];
		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				_cells[x, y] = true;
			}
			_cellStatuses[y] = true;
		}
	}

	public static void Init(Vector2Int size)
	{
		if (_instance != null)
			throw new Exception();
		_instance = new GameGrid(size);
	}

	public static bool GetCellStatus(int column, int row)
	{
		if (column >= Size.x || column < 0 || row < 0)
		{
			return false;
		}
		return _instance._cells[column, Mathf.Min(row, Size.y - 1)];
	}

	public static void FillCell(int row, int column)
	{
		_instance._cells[row, column] = false;
		if (_instance._cellStatuses[column])
		{
			_instance._cellStatuses[column] = false;
		}
	}

	public static bool GetRowStatus(int y)
	{
		return _instance._cellStatuses[y];
	}

	public static void SetRowStatus(int y, bool status)
	{
		for (int x = 0; x < Size.x; x++)
		{
			CellCellStatus(x, y, status);
		}
		_instance._cellStatuses[y] = status;
	}

	public static void MoveRow(int from, int to)
	{
		_instance._cellStatuses[to] = _instance._cellStatuses[from];
		for (int x = 0; x < Size.x; x++)
		{
			CellCellStatus(x, to, GetCellStatus(x, from));
			CellCellStatus(x, from, true);
		}
	}

	private static void CellCellStatus(int column, int row, bool status)
	{
		_instance._cells[column, row] = status;
	}
}