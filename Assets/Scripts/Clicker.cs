using UnityEngine;

public class Clicker : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	[SerializeField] private Vector3 _worldPos;
	[SerializeField] private Vector2Int cell;

	private void OnMouseDown()
	{
		_worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
		var x = Mathf.RoundToInt(_worldPos.x);
		var y = Mathf.RoundToInt(_worldPos.y);

		//GameGrid.FillCell(x, y, !GameGrid.GetCellStatus(x, y));
	}

	private void OnMouseDrag()
	{
		var _worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);

		var x = Mathf.RoundToInt(_worldPos.x);
		var y = Mathf.RoundToInt(_worldPos.y);
		if (x < 0 || y < 0 || x >= GameGrid.Size.x || y >= GameGrid.Size.y)
			return;
		if (cell.x == x && cell.y == y)
			return;
		cell.x = x;
		cell.y = y;

		//GameGrid.FillCell(x, y, !GameGrid.GetCellStatus(x, y));
	}
}