using System.Collections.Generic;
using UnityEngine;

public class GridChecker
{
	private float _deltaForInsert = 0.15f;
	private List<(Vector2Int, bool)> _checkedPos = new List<(Vector2Int, bool)>();

	public bool CheckPosition(int x, float y)
	{
		int row = (int)y;

		var status = GameGrid.GetCellStatus(x, row);
		if (y % 1 > _deltaForInsert)
		{
			int nextRow = row + 1;
			var nextStatus = GameGrid.GetCellStatus(x, nextRow);
			if (!status || !nextStatus)
				return false;
		}		
		return status;
	}

	public bool CheckSidePositions(List<Block> blocks, int direction)
	{
		foreach (var block in blocks)
		{
			int row = (int)block.Y;
			var status = GameGrid.GetCellStatus(block.X + direction, row);
			if (block.Y % 1 > _deltaForInsert)
			{
				int nextRow = row + 1;
				var nextStatus = GameGrid.GetCellStatus(block.X + direction, nextRow);
				if (!status || !nextStatus)
					return false;
			}
			if (!status)
				return false;
		}
		return true;
	}

	public bool CheckBottomPositions(List<Block> blocks)
	{
		_checkedPos.Clear();

		foreach (var block in blocks)
		{
			if (block.Y <= 0)
			{
				return false;
			}
			int row = (int)block.Y;
			bool status = GameGrid.GetCellStatus(block.X, row);
			if (!status)
			{
				return false;
			}
			_checkedPos.Add((new Vector2Int(block.X, row), status));
		}
		return true;
	}

	public bool CheckPositions(List<Vector3> positions)
	{
		foreach (var pos in positions)
		{
			int row = (int)pos.y;
			int column = (int)pos.x;
			var status = GameGrid.GetCellStatus(column, row);			

			if (!status)
				return false;
		}
		return true;
	}

	public void DrawGizmos()
	{
		foreach (var item in _checkedPos)
		{
			Gizmos.color = item.Item2 ? Color.cyan : Color.yellow;
			Gizmos.DrawCube(new Vector3(item.Item1.x, item.Item1.y, 2), Vector3.one * 0.9f);
		}
	}
}