using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System.Collections;

namespace UnityEditor.UI
{
	[CustomEditor(typeof(UIItemSlot), true)]
	public class UIItemSlotEditor : UISlotBaseEditor {

        private SerializedProperty onRightClickProperty;
        private SerializedProperty onDoubleClickProperty;
        private SerializedProperty onAssignProperty;
		private SerializedProperty onUnassignProperty;
		
		protected override void OnEnable()
		{
			base.OnEnable();
            this.onRightClickProperty = this.serializedObject.FindProperty("onRightClick");
            this.onDoubleClickProperty = this.serializedObject.FindProperty("onDoubleClick");
			this.onAssignProperty = this.serializedObject.FindProperty("onAssign");
			this.onUnassignProperty = this.serializedObject.FindProperty("onUnassign");
		}
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			EditorGUILayout.Separator();
			
			this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.onRightClickProperty, new GUIContent("On Right Click"), true);
            EditorGUILayout.PropertyField(this.onDoubleClickProperty, new GUIContent("On Double Click"), true);
            EditorGUILayout.PropertyField(this.onAssignProperty, new GUIContent("On Assign"), true);
			EditorGUILayout.PropertyField(this.onUnassignProperty, new GUIContent("On Unassign"), true);
			this.serializedObject.ApplyModifiedProperties();
		}
	}
}
