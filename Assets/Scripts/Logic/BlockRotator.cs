using System.Collections.Generic;
using UnityEngine;

public class BlockRotator
{
	private GridChecker _gridChecker;

	public BlockRotator(GridChecker gridChecker)
	{
		_gridChecker = gridChecker;
	}

	public void RotateFigure(Figure figure)
	{
		if (figure.isSymmetric)
			return;
		List<Vector2> newPositions = new List<Vector2>();
		bool isBlocked = false;
		foreach (var block in figure.blocks)
		{
			if (block.X == figure.center.x && block.Y == figure.center.y)
			{
				newPositions.Add(new Vector2(block.X, block.Y));
				continue;
			}
			var newPos = RotateBlock(block.X, block.Y, figure.center);
			int newX = (int)newPos.x;
			if (_gridChecker.CheckPosition(newX, newPos.y))
			{
				newPositions.Add(newPos);
			}
			else
			{
				Debug.Log($"[Game] blocked rotation {block.X} {block.Y}");
				isBlocked = true;
				break;
			}
		}
		if (!isBlocked)
		{
			for (int i = 0; i < figure.blocks.Count; i++)
			{
				figure.blocks[i].X = (int)newPositions[i].x;
				figure.blocks[i].Y = newPositions[i].y;
			}
		}
	}

	public Vector2 RotateBlock(int x, float y, Vector2 center)
	{
		float newX = Mathf.Round(center.x + (y - center.y));
		float newY = center.y + (center.x - x);		
		return new Vector2(newX, newY);
	}
}