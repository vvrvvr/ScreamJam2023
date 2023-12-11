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
        private SerializedProperty triggerTag;
        private SerializedProperty triggerLayer;

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
            EditorGUILayout.PropertyField(gizmosDrawType);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Trigger Tag");
            triggerTag.stringValue = EditorGUILayout.TagField("", triggerTag.stringValue);
            EditorGUILayout.EndHorizontal();

            // Show LayerMask field using a LayerMaskField
            triggerLayer.intValue = LayerMaskField("Trigger Layer", triggerLayer.intValue);
            EditorGUILayout.PropertyField(onTriggerEnterEvent);

            // Show tags array field with selectable tags
           

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
