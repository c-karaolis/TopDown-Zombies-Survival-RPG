using UnityEngine;
using System.Collections;

namespace Foxlair.Character
{
	/// <summary>
	/// The various states you can use to check if your character is doing something at the current frame
	/// </summary>    
	public class CharacterStates
	{
		/// The possible character conditions
		public enum CharacterConditions
		{
			Normal,
			ControlledMovement,
			Frozen,
			Paused,
			Dead
		}

		/// The possible Movement States the character can be in. These usually correspond to their own class, 
		/// but it's not mandatory
		public enum MovementStates
		{
			Null,
			Idle,
			Falling,
			Walking,
			Running,
			Crouching,
			Crawling,
			Dashing,
			Jetpacking,
			Jumping,
			Pushing,
			DoubleJumping,
			Attacking,
			FallingDownHole,
			Harvesting
		}
	}
}