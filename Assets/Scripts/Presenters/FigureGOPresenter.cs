using System.Collections.Generic;
using UnityEngine;

public class FigureGOPresenter : PresenterBehaviour<Game>
{
	private UnityObjectPool<GameObject> _blockPool;
	private List<BlockGOPresenter> _blocks = new List<BlockGOPresenter>();

	public void SetPool(UnityObjectPool<GameObject> pool)
	{
		this._blockPool = pool;
	}

	protected override void OnInject()
	{
		Model.OnFigureCreated += CreateFigureView;
		Model.OnFigureStopped += RemoveFigureView;
		Model.OnRestarted += Restart;
	}

	protected override void OnRemove()
	{
		Model.OnFigureCreated -= CreateFigureView;
		Model.OnFigureStopped -= RemoveFigureView;
		Model.OnRestarted -= Restart;
	}

	private void RemoveFigureView(Figure figure = null)
	{
		foreach (var item in _blocks)
		{
			item.RemoveModel();
			_blockPool.Return(item.go);
		}
		_blocks.Clear();
	}

	private void CreateFigureView(Figure figure)
	{
		foreach (var item in figure.blocks)
		{
			BlockGOPresenter presenter = new BlockGOPresenter(item, _blockPool.Get());
			_blocks.Add(presenter);
		}
	}

	private void Restart()
	{
		RemoveFigureView();
	}
}