using System.Collections.Generic;
using UnityEngine;

public class UnityObjectPool<T> where T : Object
{
	protected Queue<T> queue = new Queue<T>();
	protected T origin;

	protected UnityObjectPool(T origin)
	{
		this.origin = origin;
	}

	public virtual void Preload(int count)
	{
		List<T> items = new List<T>();
		for (int i = 0; i < count; i++)
		{
			items.Add(Get());
		}
		foreach (var item in items)
		{
			Return(item);
		}
	}

	public T Get()
	{
		T item;
		//if (!origin.name.Contains("Cube"))
		//	Debug.Log($"[UnityObjectPool] queue count {queue.Count}");
		if (queue.Count == 0)
		{
			item = Create();
			//if (!item.name.Contains("Cube"))
			//	Debug.Log($"[UnityObjectPool] <color=green>●</color> create {item.name}; {queue.Count}", item);

		}
		else
		{
			item = queue.Dequeue();
			//if (!item.name.Contains("Cube"))
			//	Debug.Log($"[UnityObjectPool] <color=yellow>●</color> pop {item.name}; {queue.Count}", item);
		}
		BeforeGet(item);
		return item;
	}

	public void Return(T item)
	{
		BeforeReturn(item);
		queue.Enqueue(item);
	}

	protected virtual T Create()
	{
		return GameObject.Instantiate<T>(origin);
	}

	protected virtual void BeforeGet(T item)
	{
	}

	protected virtual void BeforeReturn(T item)
	{
	}
}