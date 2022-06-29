using System;

public class Block
{
	public int X
	{
		get => _x;
		set
		{
			if (_x != value)
			{
				_x = value;
				OnMoved?.Invoke();
			}
		}
	}

	private int _x = -1;

	public float Y
	{
		get => _y;
		set
		{
			if (_y != value)
			{
				_y = value;
				OnMoved?.Invoke();
			}
		}
	}

	private float _y = -1;

	public event Action OnMoved;
}