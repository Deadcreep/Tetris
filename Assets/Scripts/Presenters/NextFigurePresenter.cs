using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextFigurePresenter : PresenterBehaviour<Game>
{
	[SerializeField] private Vector2Int _size;
	private Vector3 startPos;
	private float _pixelsInUnit;
	private RectTransform _rectTransform;
	private UnityObjectPool<Image> _pool;
	private List<Image> _currentImages = new List<Image>();

	public void SetPool(UnityObjectPool<Image> pool)
	{
		_pool = pool;
	}

	protected override void OnInject()
	{
		Model.OnFigureCreated += UpdateView;
		float screenHeightInUnits = Camera.main.orthographicSize * 2;
		float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;
		_pixelsInUnit = Screen.width / screenWidthInUnits;
		_rectTransform = GetComponent<RectTransform>();
		_rectTransform.sizeDelta = new Vector2(_size.x, _size.y) * _pixelsInUnit;
		_rectTransform.anchoredPosition = new Vector2(0, -_rectTransform.sizeDelta.y / 2);
	}

	protected override void OnRemove()
	{
		Model.OnFigureCreated -= UpdateView;
		foreach (var item in _currentImages)
		{
			_pool.Return(item);
		}
		_currentImages.Clear();
	}

	private void UpdateView(Figure obj)
	{
		var figure = Model.NextFigure;
		startPos = Vector3.zero;
		startPos.x = _rectTransform.position.x + ((figure.size.x / 2 - (figure.size.x % 2 == 0 ? 0.5f : 0.0f)) * _pixelsInUnit);
		startPos.y = _rectTransform.position.y - (_rectTransform.sizeDelta.y / 2) + (figure.size.y * _pixelsInUnit);
		int addedImages = 0;
		for (int y = 0; y < figure.size.y; y++)
		{
			for (int x = figure.size.x - 1; x >= 0; x--)
			{
				var val = figure[x, y];
				if (val)
				{
					while (_currentImages.Count - 1 < addedImages)
					{
						_currentImages.Add(_pool.Get());
					}
					var block = _currentImages[addedImages];
					block.rectTransform.sizeDelta = new Vector2(_pixelsInUnit * 0.9f, _pixelsInUnit * 0.9f);
					block.transform.SetParent(transform, false);
					block.rectTransform.position = new Vector2(startPos.x - (x * _pixelsInUnit), startPos.y - (y * _pixelsInUnit));
					addedImages++;
				}
			}
		}
	}
}