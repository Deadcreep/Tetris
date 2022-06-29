using UnityEngine;

public class GOPool : UnityObjectPool<GameObject>
{
	public GOPool(GameObject origin) : base(origin)
	{
	}

	protected override void BeforeGet(GameObject item)
	{
		item.SetActive(true);
	}

	protected override void BeforeReturn(GameObject item)
	{
		item.transform.parent = null;
		item.SetActive(false);
	}

	protected override GameObject Create()
	{
		return GameObject.Instantiate(origin);
	}
}