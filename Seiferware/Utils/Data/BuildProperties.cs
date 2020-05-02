using System;
using System.Linq;
using Seiferware.Utils.Management;
using UnityEngine;

namespace Seiferware.Utils.Data {
	public class BuildProperties : MonoSingleton {
		public SystemProperty[] properties;
		public static BuildProperties Instance => GetInstance<BuildProperties>();
		public static string GetValue(string id) {
			return (from prop in Instance.properties
				where id == prop.propertyName
				from ov in prop.overrides
				where ov.target == Application.platform
				select ov.value).FirstOrDefault();
		}
	}
	[Serializable]
	public struct SystemProperty {
		public string propertyName;
		public string defaultValue;
		public SystemPropertyOverride[] overrides;
	}
	[Serializable]
	public struct SystemPropertyOverride {
		public RuntimePlatform target;
		public string value;
	}
}
