using UnityEngine;

namespace Seiferware.Utils.Data {
	public static class StructUtils {
		public static Bounds BoundsFromEdges(Vector3 start, Vector3 end) {
			return new Bounds((start + end) / 2f, end - start);
		}
	}
}
