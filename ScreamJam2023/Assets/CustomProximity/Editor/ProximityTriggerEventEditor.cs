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
        private SerializedProperty gizmosDrawType;
        private SerializedProperty onTriggerEnterEvent;
        private SerializedProperty triggerTags; // New serialized property for tags
        private SerializedProperty triggerLayer; // New serialized property for layer

        private void OnEnable()
        {
            proximityTrigger = (ProximityTriggerEvent)target;

            colliderType = serializedObject.FindProperty("colliderType");
            colliderSize = serializedObject.FindProperty("colliderSize");
            colliderPosition = serializedObject.FindProperty("colliderPosition");
            colliderRotation = serializedObject.FindProperty("colliderRotation");
            gizmosColor = serializedObject.FindProperty("gizmosColor");
            gizmosDrawType = serializedObject.FindProperty("gizmosDrawType");
            onTriggerEnterEvent = serializedObject.FindProperty("onTriggerEnterEvent");
            triggerTags = serializedObject.FindProperty("triggerTags"); // Assign serialized property for tags
            triggerLayer = serializedObject.FindProperty("triggerLayer"); // Assign serialized property for layer
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(colliderType);
            EditorGUILayout.PropertyField(colliderSize);
            EditorGUILayout.PropertyField(colliderPosition);
            EditorGUILayout.PropertyField(colliderRotation);
            EditorGUILayout.PropertyField(gizmosColor);
            EditorGUILayout.PropertyField(gizmosDrawType);
            EditorGUILayout.PropertyField(onTriggerEnterEvent);
            EditorGUILayout.PropertyField(triggerTags); // Show tags field
            EditorGUILayout.PropertyField(triggerLayer); // Show layer field

            serializedObject.ApplyModifiedProperties();
        }
    }
}
