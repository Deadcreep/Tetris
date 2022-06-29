using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RotationTest
{
	// A Test behaves as an ordinary method

	BlockRotator _blockRotator;
	[SetUp]
	public void Setup()
	{
	//	_blockRotator = new BlockRotator();
	}

	[Test]
	public void RotationTestSimplePasses()
	{
		for (int step = 0; step < 100; step++)
		{

			var blocks = GetBlocks();
			var results = GetResult();
			var displacement = Random.Range(-0.99f, 0.99f);
			Vector2 center = Vector2.zero;
			center.y += displacement;
			for (int i = 0; i < blocks.Count; i++)
			{
				Vector2 item = blocks[i];
				var result = results[i];
				result.y += displacement;
				item.y += displacement;
				if (item.x == center.x && item.y == center.y)
				{
					Debug.Log($"[RotationTest] skip {item}");
					continue;
				}
				Debug.Log($"[RotationTest] {step}");
				var rotated = _blockRotator.RotateBlock((int)item.x, item.y, center);
				Assert.AreEqual(result, rotated);
			}
		}
	}

	// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
	// `yield return null;` to skip a frame.
	//[UnityTest]
	//public IEnumerator RotationTestWithEnumeratorPasses()
	//{
	//	// Use the Assert class to test conditions.
	//	// Use yield to skip a frame.
	//	yield return null;
	//}

	private List<Vector2> GetBlocks()
	{
		return new List<Vector2>()
		{
			new Vector2(0,1),
			new Vector2(1,1),
			new Vector2(0,0),
			new Vector2(0,-1)
		};
	}

	private List<Vector2> GetResult()
	{
		return new List<Vector2>()
		{
			new Vector2(1,0),
			new Vector2(1,-1),
			new Vector2(0,0),
			new Vector2(-1,0)
		};
	}


}


/*
 *	   0 1
 *	   
 *  1  1 1 
 *  0  1 0
 * -1  1 0
 *   
 *   -1 0 1
 *    
 * 1  1 1 1
 * 0  0 0 1
 * 
 * 
 *		float newX = Mathf.Floor(center.x + (y - center.y));
		float newY = center.y + (center.x - x);
		Debug.Log($"[BlockRotator] {x} => {newX}; {y} => {newY}");
 * */