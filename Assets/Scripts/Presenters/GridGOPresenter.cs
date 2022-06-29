using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridGOPresenter : PresenterBehaviour<Game>
{
	[SerializeField] private float _speed = 1.5f;
	private UnityObjectPool<GameObject> _blockPool;
	private ComponentPool<RowGOPresenter> _rowPool;
	[SerializeField] private List<RowGOPresenter> _rows;

	public void SetPool(UnityObjectPool<GameObject> pool, ComponentPool<RowGOPresenter> rowPool)
	{
		this._rowPool = rowPool;
		this._blockPool = pool;
		_rows = new List<RowGOPresenter>();
		_rows.AddRange(Enumerable.Repeat<RowGOPresenter>(null, GameGrid.Size.y));
	}

	protected override void OnInject()
	{
		Model.OnFigureStopped += UpdateGrid;
		Model.OnGridUpdated += UpdateGrid;
		Model.OnRestarted += Restart;
	}

	protected override void OnRemove()
	{
		Model.OnFigureStopped -= UpdateGrid;
		Model.OnGridUpdated -= UpdateGrid;
		Model.OnRestarted -= Restart;
	}

	private void UpdateGrid(UpdadeGridInfo info)
	{
		for (int i = 0; i < info.DestroyedRows.Count; i++)
		{
			var item = _rows[info.DestroyedRows[i]];

			if (!item)
			{
				continue;
			}
			item.Destroy(() =>
			{
				_rowPool.Return(item);
			});
			_rows[info.DestroyedRows[i]] = null;
		}
		if (info.RowsUpdatedPositions.Count > 0)
		{
			MoveRows(info);
		}
		else
		{
			Model.CreateFigure();
		}
	}

	private void MoveRows(UpdadeGridInfo info)
	{
		for (int i = 0; i < info.RowsUpdatedPositions.Count; i++)
		{
			(int oldY, int newY) item = info.RowsUpdatedPositions[i];
			//Debug.Log($"[GridGOPresenter] old {item.oldY}; new {item.newY}", this);
			if (!_rows[item.oldY])
			{
				Model.CreateFigure();
				return;
			}
			var row = _rows[item.oldY];
			if (i == info.RowsUpdatedPositions.Count - 1)
			{
				row.MoveTo(item.newY, Model.CreateFigure);
			}
			else
			{
				row.MoveTo(item.newY);
			}
			_rows[item.oldY] = null;
			_rows[item.newY] = row;
		}
	}

	private void UpdateGrid(Figure figure)
	{
		foreach (var block in figure.blocks)
		{
			int row = (int)block.Y;
			if (_rows[row] == null)
			{
				//Debug.Log($"<color=black>[GridGOPresenter]</color> before create {DateTime.Now.Ticks}", this);
				RowGOPresenter rowPresenter = _rowPool.Get();
				//Debug.Log($"<color=black>[GridGOPresenter]</color> after create {DateTime.Now.Ticks}", this);
				rowPresenter.name = $"{row};";
				rowPresenter.transform.parent = transform;
				rowPresenter.Setup(_blockPool, _speed, row);
				_rows[row] = rowPresenter;
				//Debug.Log($"[GridGOPresenter] row {row} pres not found", rowPresenter);
			}
			//Debug.Log($"[GridGOPresenter] add block to row {row}; raw y {block.Y}", _rows[row]);
			_rows[row].AddBlock(block.X);
		}
	}

	private void Restart()
	{
		for (int i = 0; i < _rows.Count; i++)
		{
			RowGOPresenter item = _rows[i];
			if (item)
			{
				item.Destroy();
				_rowPool.Return(item);
				_rows[i] = null;
			}
			else
				break;
		}
	}
}