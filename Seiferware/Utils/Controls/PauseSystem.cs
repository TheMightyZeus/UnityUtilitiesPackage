using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Seiferware.Utils.Controls {
	public class PauseSystem : MonoBehaviour {
		public bool paused;
		public bool controlCursor;
		public bool freezeTime;
		public PauseEvent onPauseStart;
		public PauseEvent onPauseEnd;
		public GameObject[] pausedGameObjects;
		public GameObject[] liveGameObjects;
		public MonoBehaviour[] pausedBehaviors;
		public MonoBehaviour[] liveBehaviors;
		private bool wasPaused;
		private void Start() {
			ChangePause(paused);
		}
		private void Update() {
			if(paused && !wasPaused) {
				ChangePause(true);
			} else if(!paused && wasPaused) {
				ChangePause(false);
			}
		}
		private void LateUpdate() {
			wasPaused = paused;
		}
		public void StartPause() {
			if(!paused) {
				ChangePause(true);
			}
		}
		public void EndPause() {
			if(paused) {
				ChangePause(false);
			}
		}
		private void ChangePause(bool p) {
			paused = p;
			if(pausedGameObjects != null) {
				for(int i = 0; i < pausedGameObjects.Length; i++) {
					pausedGameObjects[i].SetActive(p);
				}
			}
			if(liveGameObjects != null) {
				for(int i = 0; i < liveGameObjects.Length; i++) {
					liveGameObjects[i].SetActive(!p);
				}
			}
			if(pausedBehaviors != null) {
				for(int i = 0; i < pausedBehaviors.Length; i++) {
					pausedBehaviors[i].enabled = p;
				}
			}
			if(liveBehaviors != null) {
				for(int i = 0; i < liveBehaviors.Length; i++) {
					liveBehaviors[i].enabled = !p;
				}
			}
			if(controlCursor) {
				Cursor.visible = p;
				Cursor.lockState = p ? CursorLockMode.None : CursorLockMode.Locked;
			}
			if(freezeTime) {
				Time.timeScale = p ? 0f : 1f;
			}
			if(p) {
				onPauseStart?.Invoke();
			} else {
				onPauseEnd?.Invoke();
			}
		}
#if ENABLE_INPUT_SYSTEM
		public void ActionStartPause(InputAction.CallbackContext cc) {
			if(cc.phase == InputActionPhase.Started && !paused && !wasPaused) {
				ChangePause(true);
			}
		}
		public void ActionEndPause(InputAction.CallbackContext cc) {
			if(cc.phase == InputActionPhase.Started && paused && wasPaused) {
				ChangePause(false);
			}
		}
#endif
	}
	[Serializable]
	public class PauseEvent : UnityEvent {}
}
