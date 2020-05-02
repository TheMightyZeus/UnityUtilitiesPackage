using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Seiferware.Utils.World.Segments {
	public class SegmentItem3D : MonoBehaviour {
		[SerializeField]
		private Segmenter3D segmenter;
		private Dictionary<Type, MonoBehaviour> cache = new Dictionary<Type, MonoBehaviour>();
		public Segmenter3D Segmenter {
			get => segmenter;
			set {
				if(segmenter != null && enabled) {
					segmenter.RemoveItem(this);
				}
				segmenter = value;
				if(value != null && enabled) {
					segmenter.AddItem(this);
				}
			}
		}
		public Vector3Int Segment {
			get;
			set;
		}
		private void OnEnable() {
			if(segmenter) {
				segmenter.AddItem(this);
			}
		}
		private void OnDisable() {
			if(segmenter) {
				segmenter.RemoveItem(this);
			}
		}
		private void Update() {
			if(segmenter) {
				segmenter.UpdateItem(this);
			}
		}
		public T GetCachedComponent<T>() where T : MonoBehaviour {
			if(!cache.ContainsKey(typeof(T))) {
				cache[typeof(T)] = GetComponent<T>();
			}
			return cache[typeof(T)] as T;
		}
	}
}
