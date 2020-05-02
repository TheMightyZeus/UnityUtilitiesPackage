using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Seiferware.Utils.Data {
	public delegate void ValueChanged(string fieldName, object oldValue, object newValue);
	public class ValueChangeListener {
		private readonly object subject;
		private readonly List<IChangeListener> fields = new List<IChangeListener>();
		public ValueChangeListener(object subject) {
			this.subject = subject;
		}
		public ValueChangeListener(object subject, params string[] fields) {
			this.subject = subject;
			AddFields(fields);
		}
		public event ValueChanged valueChanged;
		public bool Check() {
			bool value = false;
			foreach(IChangeListener field in fields.Where(field => field.CheckForChange(valueChanged))) {
				value = true;
			}
			return value;
		}
		public void AddFields(params string[] names) {
			foreach(string s in names) {
				AddField(s);
			}
		}
		public void AddField(string name) {
			FieldInfo fi = subject.GetType().GetField(name);
			if(fi != null) {
				fields.Add(new FieldChangeListener(subject, fi));
			} else {
				PropertyInfo pi = subject.GetType().GetProperty(name);
				if(pi != null) {
					fields.Add(new PropertyChangeListener(subject, pi));
				} else {
					throw new MissingFieldException();
				}
			}
		}
	}
	internal interface IChangeListener {
		string Name {
			get;
		}
		bool CheckForChange(ValueChanged callback);
	}
	internal class PropertyChangeListener : IChangeListener {
		private readonly PropertyInfo info;
		private readonly object subject;
		private object value;
		public string Name => info.Name;
		public PropertyChangeListener(object subject, PropertyInfo info) {
			this.info = info;
			this.subject = subject;
			value = info.GetValue(subject);
		}
		public bool CheckForChange(ValueChanged callback) {
			object newValue = info.GetValue(subject);
			if(!Equals(newValue, value)) {
				callback?.Invoke(Name, value, newValue);
				value = newValue;
				return true;
			}
			return false;
		}
	}
	internal class FieldChangeListener : IChangeListener {
		private readonly FieldInfo info;
		private readonly object subject;
		private object value;
		public string Name => info.Name;
		public FieldChangeListener(object subject, FieldInfo info) {
			this.info = info;
			this.subject = subject;
			value = info.GetValue(subject);
		}
		public bool CheckForChange(ValueChanged callback) {
			object newValue = info.GetValue(subject);
			if(!Equals(newValue, value)) {
				callback?.Invoke(Name, value, newValue);
				value = newValue;
				return true;
			}
			return false;
		}
	}
}
