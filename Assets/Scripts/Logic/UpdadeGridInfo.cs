using System.Collections.Generic;
using UnityEngine;

public struct UpdadeGridInfo
{
	public List<int> DestroyedRows;
	public List<(int oldY, int newY)> RowsUpdatedPositions;
}