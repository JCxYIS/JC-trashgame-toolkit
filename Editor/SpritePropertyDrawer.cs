using UnityEngine;
using UnityEditor;

/// <summary>
/// Preview sprite in inspector
/// Code from https://pastebin.com/Y1R7eSA0
/// </summary>
[CustomPropertyDrawer(typeof(Sprite))]
public class SpritePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent labelN)
    {
        if (property.objectReferenceValue != null)
        {
            return _texSize;
        }
        else
        {
            return base.GetPropertyHeight(property, labelN);
        }
    }

    private const float _texSize = 70;

    

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        if (prop.objectReferenceValue != null)
        {
            position.width = EditorGUIUtility.labelWidth;
            GUI.Label(position, prop.displayName);

            position.x += position.width;
            position.width = _texSize;
            position.height = _texSize;

            prop.objectReferenceValue = EditorGUI.ObjectField(position, prop.objectReferenceValue, typeof(Sprite), false);
        }
        else
        {
            EditorGUI.PropertyField(position, prop, true);
        }

        EditorGUI.EndProperty();
    }
}