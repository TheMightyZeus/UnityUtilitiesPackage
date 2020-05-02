using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seiferware.Utils.Management {
	public abstract class MonoSingleton : MonoBehaviour {
		private static readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();
		public static T GetInstance<T>() where T : class {
			return instances[typeof(T)] as T;
		}
		private void Awake() {
			if(instances.ContainsKey(GetType())) {
				Debug.LogError("Instance of " + GetType().Name + " already present!");
				enabled = false;
			} else {
				instances[GetType()] = this;
				// foreach(Type inter in GetType().GetInterfaces()) {
				// 	if(!instances.ContainsKey(inter)) {
				// 		instances[inter] = this;
				// 	}
				// }
			}
		}
	}
}
