using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR

using UnityEditor;

#endif

[Serializable]
public class FigureVariant
{
	public List<bool> blocks = new List<bool>();
	public Vector2Int size;
	public bool isSymmetric;

	public bool this[int x, int y]
	{
		get => blocks[y + (x * size.y)];
		set => blocks[y + (x * size.y)] = value;
	}

	public override bool Equals(object obj)
	{
		if (obj is FigureVariant variant)
		{
			if (this.size != variant.size)
				return false;
			if (this.isSymmetric != variant.isSymmetric)
				return false;
			for (int y = 0; y < size.y; y++)
			{
				for (int x = 0; x < size.x; x++)
				{
					if (this[x, y] != variant[x, y])
						return false;
				}
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		int hashCode = -1458671009;

		hashCode = hashCode * -1521134295 + EqualityComparer<List<bool>>.Default.GetHashCode(blocks);
		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				hashCode = hashCode * 23 + (this[x, y] ? x * y : 0);
			}
		}
		hashCode = hashCode * -1521134295 + size.GetHashCode();
		hashCode = hashCode * -1521134295 + isSymmetric.GetHashCode();
		return hashCode;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(FigureVariant))]
public class FigureVariantDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		property.serializedObject.Update();
		var list = property.FindPropertyRelative("blocks");
		var sizeProp = property.FindPropertyRelative("size");
		var symProp = property.FindPropertyRelative("isSymmetric");
		EditorGUI.PropertyField(position, sizeProp);
		var rect = new Rect(position);
		rect.center += Vector2.up * EditorGUIUtility.singleLineHeight;
		rect.size = new Vector2(EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
		EditorGUI.PropertyField(rect, symProp);

		var size = sizeProp.vector2IntValue;
		if (list.arraySize != size.x * size.y)
		{
			list.arraySize = size.x * size.y;
		}
		var height = EditorGUIUtility.singleLineHeight;
		var startPos = rect.position;
		startPos.y += height;

		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				rect.size = new Vector2(height, height);
				rect.center = startPos + new Vector2(x * height, (y + 1) * height);
				var elem = list.GetArrayElementAtIndex(y + (x * size.y));
				elem.boolValue = EditorGUI.Toggle(rect, elem.boolValue);
			}
		}
		property.serializedObject.ApplyModifiedProperties();
	}


	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float totalH = (property.FindPropertyRelative("size").vector2IntValue.y + 3) * EditorGUIUtility.singleLineHeight;
		return totalH;
	}
}
#endif