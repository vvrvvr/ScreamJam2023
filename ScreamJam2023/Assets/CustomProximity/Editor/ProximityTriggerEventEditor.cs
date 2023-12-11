using UnityEditor;
using UnityEngine;

namespace CustomProximity.Editors
{
    [CustomEditor(typeof(ProximityTriggerEvent))]
    public class ProximityTriggerEventEditor : Editor
    {
        private ProximityTriggerEvent proximityTrigger;

        private SerializedProperty colliderType;
        private SerializedProperty colliderSize;
        private SerializedProperty colliderPosition;
        private SerializedProperty colliderRotation;
        private SerializedProperty gizmosColor;
        private SerializedProperty gizmosDrawType; // New serialized property for Gizmos draw type
        private SerializedProperty onTriggerEnterEvent;

        private void OnEnable()
        {
            proximityTrigger = (ProximityTriggerEvent)target;

            colliderType = serializedObject.FindProperty("colliderType");
            colliderSize = serializedObject.FindProperty("colliderSize");
            colliderPosition = serializedObject.FindProperty("colliderPosition");
            colliderRotation = serializedObject.FindProperty("colliderRotation");
            gizmosColor = serializedObject.FindProperty("gizmosColor");
            gizmosDrawType = serializedObject.FindProperty("gizmosDrawType"); // Assign Gizmos draw type property
            onTriggerEnterEvent = serializedObject.FindProperty("onTriggerEnterEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(colliderType);
            EditorGUILayout.PropertyField(colliderSize);
            EditorGUILayout.PropertyField(colliderPosition);
            EditorGUILayout.PropertyField(colliderRotation);
            EditorGUILayout.PropertyField(gizmosColor);
            EditorGUILayout.PropertyField(gizmosDrawType); // Show Gizmos draw type field
            EditorGUILayout.PropertyField(onTriggerEnterEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
