using System;
using UnityEngine;

public class BlockGOPresenter
{
	public GameObject go;
	private Block _block;

	public BlockGOPresenter(Block block, GameObject go)
	{
		_block = block;
		this.go = go;
		this.go.transform.position = new Vector3(block.X, block.Y, 0);
		_block.OnMoved += UpdatePosition;
	}

	public void RemoveModel()
	{
		_block.OnMoved -= UpdatePosition;
		_block = null;
	}

	private void UpdatePosition()
	{
		go.transform.position = new Vector3(_block.X, _block.Y, 0);
	}
}