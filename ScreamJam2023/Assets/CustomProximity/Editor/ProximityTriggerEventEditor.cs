// ProximityTriggerEventEditor.cs

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
        private SerializedProperty onTriggerExitEvent; // New SerializedProperty for OnTriggerExit
        private SerializedProperty triggerTag;
        private SerializedProperty triggerLayer;
        private SerializedProperty maxTriggerCount; // New serialized property for max trigger count

        private void OnEnable()
        {
            proximityTrigger = (ProximityTriggerEvent)target;

            colliderType = serializedObject.FindProperty("colliderType");
            colliderSize = serializedObject.FindProperty("colliderSize");
            colliderPosition = serializedObject.FindProperty("colliderPosition");
            colliderRotation = serializedObject.FindProperty("colliderRotation");
            gizmosColor = serializedObject.FindProperty("gizmosColor");
            gizmosDrawType = serializedObject.FindProperty("gizmosDrawType");
            triggerTag = serializedObject.FindProperty("triggerTag");
            triggerLayer = serializedObject.FindProperty("triggerLayer");
            maxTriggerCount = serializedObject.FindProperty("maxTriggerCount"); // Assign max trigger count property
            onTriggerEnterEvent = serializedObject.FindProperty("onTriggerEnterEvent");
            onTriggerExitEvent = serializedObject.FindProperty("onTriggerExitEvent"); // Assign onTriggerExitEvent property
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
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Trigger Tag");
            triggerTag.stringValue = EditorGUILayout.TagField("", triggerTag.stringValue);
            EditorGUILayout.EndHorizontal();

            // Show LayerMask field using a LayerMaskField
            triggerLayer.intValue = LayerMaskField("Trigger Layer", triggerLayer.intValue);

            // Show the max trigger count field
            EditorGUILayout.PropertyField(maxTriggerCount, new GUIContent("Max Trigger Count"));

            EditorGUILayout.PropertyField(onTriggerEnterEvent);
            EditorGUILayout.PropertyField(onTriggerExitEvent);

            serializedObject.ApplyModifiedProperties();
        }

        // Custom LayerMask field to display layers in a dropdown
        private int LayerMaskField(string label, int layerMask)
        {
            var layers = UnityEditorInternal.InternalEditorUtility.layers;
            layerMask = EditorGUILayout.MaskField(label, layerMask, layers);
            return layerMask;
        }
    }
}
