using UnityEngine;

[System.Serializable]
public class SceneField {
    [SerializeField] private Object sceneAsset;
    [SerializeField] private string sceneName = "";
    public string SceneName => sceneName;
    
    // makes it work with the existing Unity methods (LoadLevel/LoadScene)
    public static implicit operator string(SceneField sceneField) {
        return sceneField.SceneName;
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : UnityEditor.PropertyDrawer {
    public override void OnGUI(Rect _position, UnityEditor.SerializedProperty _property, GUIContent _label) {
        UnityEditor.EditorGUI.BeginProperty(_position, GUIContent.none, _property);
        UnityEditor.SerializedProperty sceneAsset = _property.FindPropertyRelative("sceneAsset");
        UnityEditor.SerializedProperty sceneName = _property.FindPropertyRelative("sceneName");
        _position = UnityEditor.EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
        if (sceneAsset != null) {
            sceneAsset.objectReferenceValue = UnityEditor.EditorGUI.ObjectField(_position, sceneAsset.objectReferenceValue, typeof(UnityEditor.SceneAsset), false);
            if (sceneAsset.objectReferenceValue != null) {
                sceneName.stringValue = (sceneAsset.objectReferenceValue as UnityEditor.SceneAsset).name;
            }
        }
        UnityEditor.EditorGUI.EndProperty();
    }
}
#endif