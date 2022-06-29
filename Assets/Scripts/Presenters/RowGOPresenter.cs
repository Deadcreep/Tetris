using System;
using System.Collections;
using UnityEngine;

public class RowGOPresenter : MonoBehaviour
{
	private UnityObjectPool<GameObject> _pool;
	private int _row;
	private float _speed;
	private ParticleSystem _particle;

	private void OnEnable()
	{
		if (_particle == null)
			_particle = GetComponentInChildren<ParticleSystem>();
		_particle.transform.localPosition = new Vector3(GameGrid.Size.x / 2 - 0.5f, 0, -1);
	}

	public void Setup(UnityObjectPool<GameObject> pool, float speed, int row)
	{
		transform.position = new Vector3(0, row, 0);
		_pool = pool;
		_row = row;
		_speed = speed;
	}

	public void AddBlock(int x)
	{
		var block = _pool.Get();
		block.transform.parent = transform;
		block.transform.localPosition = new Vector3(x, 0, 0);
	}

	public void MoveTo(int newRow, Action callback = null)
	{
		StartCoroutine(MoveCoroutine(newRow, callback));
	}

	public void Destroy(Action callback = null)
	{
		StartCoroutine(DestroyCoroutine(callback));
	}

	private IEnumerator MoveCoroutine(int newRow, Action callback)
	{
		while (transform.position.y > newRow)
		{
			transform.position -= new Vector3(0, _speed * Time.deltaTime, 0);
			yield return null;
		}
		transform.position = new Vector3(transform.position.x, newRow, 0);
		gameObject.name = $"form {_row} to {newRow};";
		_row = newRow;
		callback?.Invoke();
	}

	private IEnumerator DestroyCoroutine(Action callback)
	{
		while (transform.childCount > 1)
		{
			_pool.Return(transform.GetChild(1).gameObject);
		}
		_particle.Play();
		yield return new WaitForSeconds(_particle.main.startLifetime.constant);
		callback?.Invoke();
	}
}