using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Seiferware.Utils.Controls {
	enum GroundState {
		FALLING,
		GROUNDED,
		JUMPING
	}
	public class FirstPersonMovement : MonoBehaviour {
		public CharacterController controller;
		public float minPitch = -89f;
		public float maxPitch = 89f;
		// This gets the pitch (up and down) rotation.
		public Transform pitchTransform;
		// This gets the yaw (look left and right) rotation.
		public Transform yawTransform;
		// This is the frame of reference for left/right/forward/backward.
		public Transform perspectiveTransform;
		// If the character is moving backwards, we might want that to be slower than forwards movement.
		// Everything else (forward, lateral) can be handled by scaling the value in the InputAction.
		public float backwardsMovementScale = 1f;
		public float jumpForce = 3f;
		public float gravity = 9.81f;
		private float pitchValue;
		private bool currentCrouch = false;
		private bool currentSprint = false;
		private float currentVertical = 0f;
		private Vector2 currentMove = Vector2.zero;
		private Vector2 currentLook = Vector2.zero;
		private void Update() {
			Look(currentLook);
			Move(currentMove);
		}
		private void Look(Vector2 look) {
			yawTransform.Rotate(Vector3.up, look.x * Time.deltaTime);
			pitchValue -= look.y * Time.deltaTime;
			pitchValue = Mathf.Clamp(pitchValue, minPitch, maxPitch);
			// TODO: This works if the pitchTransform only ever gets pitched. But if it's also the yaw transform, it'll be an issue.
			pitchTransform.localRotation = Quaternion.Euler(pitchValue, 0, 0);
		}
		private void Move(Vector2 move) {
			Vector3 translate = StripY(perspectiveTransform.forward) * move.y + StripY(perspectiveTransform.right) * move.x;
			if(translate.z < 0) {
				translate.z *= backwardsMovementScale;
			}
			currentVertical -= gravity * Time.deltaTime;
			translate.y = currentVertical;
			controller.Move(translate * Time.deltaTime);
		}
		private static Vector3 StripY(Vector3 v3) {
			v3.y = 0;
			return v3.normalized;
		}
		public void DoSprint(bool sprint) {
			currentSprint = sprint;
		}
		public void DoCrouch(bool crouch) {
			currentCrouch = crouch;
		}
		public void DoJump() {
			if(controller.isGrounded) {
				currentVertical = jumpForce;
			}
		}
		public void DoMove(Vector2 move) {
			currentMove = move;
		}
		public void DoLook(Vector2 look) {
			currentLook = look;
		}
#if ENABLE_INPUT_SYSTEM
		// We can handle speed and look inversion by scaling the InputAction.
		public void ActionLook(InputAction.CallbackContext cc) {
			switch(cc.phase) {
				case InputActionPhase.Performed:
					DoLook(cc.ReadValue<Vector2>());
					break;
				case InputActionPhase.Canceled:
					DoLook(Vector2.zero);
					break;
			}
		}
		public void ActionMove(InputAction.CallbackContext cc) {
			switch(cc.phase) {
				case InputActionPhase.Performed:
					DoMove(cc.ReadValue<Vector2>());
					break;
				case InputActionPhase.Canceled:
					DoMove(Vector2.zero);
					break;
			}
		}
		// If we don't want jump, crouch, sprint, we can simply not assign any action to them.
		public void ActionJump(InputAction.CallbackContext cc) {
			if(cc.phase == InputActionPhase.Started) {
				DoJump();
			}
		}
		public void ActionCrouch(InputAction.CallbackContext cc) {
			switch(cc.phase) {
				case InputActionPhase.Started:
					DoCrouch(true);
					break;
				case InputActionPhase.Canceled:
					DoCrouch(false);
					break;
			}
		}
		public void ActionSprint(InputAction.CallbackContext cc) {
			switch(cc.phase) {
				case InputActionPhase.Started:
					DoSprint(true);
					break;
				case InputActionPhase.Canceled:
					DoSprint(false);
					break;
			}
		}
#endif
	}
}
