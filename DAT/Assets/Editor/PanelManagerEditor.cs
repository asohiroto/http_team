using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LibraryManager))]
public class PanelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LibraryManager manager = (LibraryManager)target;

        if (GUILayout.Button("子オブジェクトを一括登録"))
        {
            Transform parent = manager.transform;

            SerializedObject so = new SerializedObject(manager);
            SerializedProperty prop = so.FindProperty("LibTrans");
            prop.arraySize = parent.childCount;

            for (int i = 0; i < parent.childCount; i++)
                prop.GetArrayElementAtIndex(i).objectReferenceValue = parent.GetChild(i);

            so.ApplyModifiedProperties();
        }
    }
}
