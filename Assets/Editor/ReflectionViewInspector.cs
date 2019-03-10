using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ReflectionView.Editor
{
    [CustomEditor(typeof(ReflectionView))]
    public class ReflectionViewInspector : UnityEditor.Editor
    {
        List<SerializedValue> serializedValues = new List<SerializedValue>();


        private void OnEnable()
        {
            serializedValues.Clear();

            if (target is ReflectionView reflectionView)
            {
                var components = reflectionView.gameObject.GetComponents<Component>();
                foreach (var component in components)
                {
                    var componentName = component.ToString();
                    SerializedContext context = new SerializedContextObject(component);
                    SerializedValue value = new SerializedValue(context, componentName);
                    serializedValues.Add(value);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.indentLevel++;

            foreach (var value in serializedValues)
            {
                value.Draw();
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();
        }
    }
}