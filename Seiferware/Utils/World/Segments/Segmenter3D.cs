using System;
using System.Collections.Generic;
using Seiferware.Utils.Data;
using UnityEngine;

namespace Seiferware.Utils.World.Segments {
	public delegate void SegmentEvent(SegmentItem3D item3D, Vector3Int seg);
	public delegate void TwoSegmentEvent(SegmentItem3D item3D, Vector3Int seg1, Vector3Int seg2);
	public class Segmenter3D : MonoBehaviour {
		public SegmentEvent itemAdded;
		public TwoSegmentEvent itemMoved;
		public SegmentEvent itemRemoved;
		private ISet<SegmentItem3D> items = new HashSet<SegmentItem3D>();
		private int maxX;
		private int maxY;
		private int maxZ;
		private int minX;
		private int minY;
		private int minZ;
		private Dictionary<Vector3Int, ISet<SegmentItem3D>> segments = new Dictionary<Vector3Int, ISet<SegmentItem3D>>();
		[SerializeField]
		private float segmentSize = 10;
		public float SegmentSize {
			get => segmentSize;
			set {
				segmentSize = value;
				ForceUpdate();
			}
		}
		private void ForceUpdate() {
			foreach(SegmentItem3D item in items) {
				UpdateItem(item);
			}
		}
		public void RemoveItem(SegmentItem3D item) {
			if(items.Contains(item)) {
				items.Remove(item);
				RemoveFromSegment(item, item.Segment);
				itemRemoved?.Invoke(item, item.Segment);
			}
		}
		public void AddItem(SegmentItem3D item) {
			if(!items.Contains(item)) {
				SetPosition(item);
				items.Add(item);
				PutInSegment(item, item.Segment);
				itemAdded?.Invoke(item, item.Segment);
			} else {
				UpdateItem(item);
			}
		}
		public void UpdateItem(SegmentItem3D item) {
			Vector3Int v = item.Segment;
			SetPosition(item);
			if(v != item.Segment) {
				RemoveFromSegment(item, v);
				PutInSegment(item, item.Segment);
				itemMoved?.Invoke(item, v, item.Segment);
			}
		}
		private void SetPosition(SegmentItem3D item) {
			item.Segment = GetSegmentFromPosition(item.transform.position);
		}
		private void PutInSegment(SegmentItem3D item3D, Vector3Int seg) {
			if(!segments.ContainsKey(seg)) {
				segments[seg] = new HashSet<SegmentItem3D>();
			}
			segments[seg].Add(item3D);
			minX = Math.Min(seg.x, minX);
			minY = Math.Min(seg.y, minY);
			minZ = Math.Min(seg.z, minZ);
			maxX = Math.Max(seg.x, maxX);
			maxY = Math.Max(seg.y, maxY);
			maxZ = Math.Max(seg.z, maxZ);
		}
		private void RemoveFromSegment(SegmentItem3D item3D, Vector3Int seg) {
			segments[seg].Remove(item3D);
		}
		public ISet<SegmentItem3D> GetItemsFromSegment(Vector3Int seg) {
			return segments.ContainsKey(seg) ? segments[seg] : new HashSet<SegmentItem3D>();
		}
		public ISet<T> GetItemsFromSegment<T>(Vector3Int seg) where T : MonoBehaviour {
			ISet<T> set = new HashSet<T>();
			ISet<SegmentItem3D> it = GetItemsFromSegment(seg);
			foreach(SegmentItem3D item in it) {
				T cc = item.GetCachedComponent<T>();
				if(cc) {
					set.Add(cc);
				}
			}
			return set;
		}
		public Vector3Int GetMinSegment() {
			return new Vector3Int(minX, minY, minZ);
		}
		public Vector3Int GetMaxSegment() {
			return new Vector3Int(maxX, maxY, maxZ);
		}
		public Vector3Int GetSegmentFromPosition(Vector3 position) {
			return new Vector3Int(
				Mathf.FloorToInt(position.x / segmentSize), Mathf.FloorToInt(position.y / segmentSize), Mathf.FloorToInt(position.z / segmentSize)
			);
		}
		public Bounds GetSegmentBounds(Vector3Int seg) {
			return StructUtils.BoundsFromEdges((Vector3)seg * segmentSize, (Vector3)(seg + Vector3Int.one) * segmentSize);
		}
		public Bounds GetFullAreaBounds() {
			return StructUtils.BoundsFromEdges((Vector3)GetMinSegment() * segmentSize, (Vector3)(GetMaxSegment() + Vector3Int.one) * segmentSize);
		}
	}
}
