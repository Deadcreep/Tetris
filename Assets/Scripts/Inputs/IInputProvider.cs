using System;

namespace Inputs
{
	public interface IInputProvider
	{
		void Pause();
		void Unpause();

		event Action OnMoveLeft;

		event Action OnMoveRight;

		event Action OnIncreaseSpeed;

		event Action OnDecreaseSpeed;

		event Action OnRotate;
	}
}