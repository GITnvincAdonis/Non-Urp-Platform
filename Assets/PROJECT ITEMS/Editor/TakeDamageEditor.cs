
using UnityEditor;
//[CustomEditor(typeof(TakeDamage))]
public class TakeDamageEditor : Editor
{
    SerializedProperty damagableType;
    SerializedProperty healthRef;
    SerializedProperty dronehealth;

    SerializedProperty attachedScript;
    SerializedProperty characterController;


    private void OnEndable()
    {
        damagableType = serializedObject.FindProperty("damagableType");
        healthRef = serializedObject.FindProperty("healthRef");
        dronehealth = serializedObject.FindProperty("dronehealth");
        attachedScript = serializedObject.FindProperty("attachedScript");
        characterController = serializedObject.FindProperty("characterController");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(damagableType);
        TakeDamage _damageHandler = (TakeDamage)target;

        if (_damageHandler.damagableType == TakeDamage.DamageableType.Player)
        {
            EditorGUILayout.PropertyField(healthRef);
            EditorGUILayout.PropertyField(attachedScript);
            EditorGUILayout.PropertyField(characterController);
        }
        else if (_damageHandler.damagableType == TakeDamage.DamageableType.Drone)
        {
            EditorGUILayout.PropertyField(dronehealth);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
