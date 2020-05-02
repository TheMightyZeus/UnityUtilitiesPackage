using UnityEditor;
using UnityEngine;

namespace Seiferware.Utils.Noise {
	[CustomPropertyDrawer(typeof(Noise))]
	public class NoisePropertyDrawer : PropertyDrawer {
		/*
		public override VisualElement CreatePropertyGUI(SerializedProperty property) {
			SerializedProperty frequency = property.FindPropertyRelative("frequency");
			SerializedProperty lacunarity = property.FindPropertyRelative("lacunarity");
			SerializedProperty octaves = property.FindPropertyRelative("octaves");
			SerializedProperty persistence = property.FindPropertyRelative("persistence");
			
			PropertyField frequencyField = new PropertyField(frequency);
			PropertyField lacunarityField = new PropertyField(lacunarity);
			PropertyField octavesField = new PropertyField(octaves);
			PropertyField persistenceField = new PropertyField(persistence);
			
			VisualElement container = new VisualElement();
			container.Add(frequencyField);
			container.Add(octavesField);
			container.Add(lacunarityField);
			container.Add(persistenceField);
			return container;
		}
		*/
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			float lineHeight = EditorGUIUtility.singleLineHeight + 2;
			EditorGUI.BeginProperty(position, label, property);
			Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			Rect seedRect = new Rect(position.x, position.y + lineHeight, position.width, EditorGUIUtility.singleLineHeight);
			Rect frequencyRect = new Rect(position.x, position.y + lineHeight * 2, position.width, EditorGUIUtility.singleLineHeight);
			Rect octavesRect = new Rect(position.x, position.y + lineHeight * 3, position.width, EditorGUIUtility.singleLineHeight);
			Rect lacunarityRect = new Rect(position.x, position.y + lineHeight * 4, position.width, EditorGUIUtility.singleLineHeight);
			Rect persistenceRect = new Rect(position.x, position.y + lineHeight * 5, position.width, EditorGUIUtility.singleLineHeight);
			EditorGUI.LabelField(labelRect, label);
			EditorGUI.indentLevel++;
			EditorGUI.PropertyField(seedRect, property.FindPropertyRelative("seed"));
			EditorGUI.PropertyField(frequencyRect, property.FindPropertyRelative("frequencyFactor"));
			EditorGUI.PropertyField(octavesRect, property.FindPropertyRelative("octaves"));
			EditorGUI.PropertyField(lacunarityRect, property.FindPropertyRelative("lacunarity"));
			EditorGUI.PropertyField(persistenceRect, property.FindPropertyRelative("persistence"));
			EditorGUI.indentLevel--;
			EditorGUI.EndProperty();
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight * 6 + 10;
		}
	}
}
