using System;
using UnityEngine;

namespace Inputs
{
	public class SimpleInputProvider : MonoBehaviour, IInputProvider
	{
		public event Action OnMoveLeft;

		public event Action OnMoveRight;

		public event Action OnIncreaseSpeed;

		public event Action OnDecreaseSpeed;

		public event Action OnRotate;

		public void Pause()
		{
			this.enabled = false;
		}

		public void Unpause()
		{
			this.enabled = true;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
			{
				OnRotate?.Invoke();
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
			{
				OnMoveLeft?.Invoke();
			}
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			{
				OnMoveRight?.Invoke();
			}

			if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				OnIncreaseSpeed?.Invoke();
			}
			if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
			{
				OnDecreaseSpeed?.Invoke();
			}
		}
	}
}