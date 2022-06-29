using System;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
	public List<Block> blocks = new List<Block>();

	public Vector2 center;
	public Vector2Int size;
	public readonly bool isSymmetric;
	public event Action OnMoved;

	public Figure(bool isSymmetric)
	{
		this.isSymmetric = isSymmetric;
	}

	public Figure(List<Block> blocks, bool isSymmetric)
	{
		this.blocks = blocks;
		this.isSymmetric = isSymmetric;
	}
}