using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Randomizer : IRandomizer
{
	private FigureVariants _variants;
	private int _prevIndex = -1;
	private Queue<int> _history = new Queue<int>();

	public Randomizer(FigureVariants variants)
	{
		_variants = variants;
	}

	public FigureVariant Get()
	{
		int index = Random.Range(0, _variants.variants.Count);
		while (_history.Contains(index))
		{
			index = Random.Range(0, _variants.variants.Count);
		}
		_history.Enqueue(index);
		if (_history.Count > _variants.variants.Count / 2)
		{
			_history.Dequeue();
		}
		FigureVariant variant = _variants.variants[index];
		_prevIndex = index;
		return variant;
	}
}