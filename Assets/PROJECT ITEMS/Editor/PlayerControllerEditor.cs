using UnityEditor;
[CustomEditor(typeof(PlatformScript))]
public class PlatformScriptEditor : Editor
{
    SerializedProperty BehaviourType;
    SerializedProperty rotationDuration;
    SerializedProperty Displacement;
    SerializedProperty DisplacementDuration; //[SerializeField] private float BounceForce;

    SerializedProperty BounceForce;



    SerializedProperty Trigger;
    SerializedProperty collider;


    SerializedProperty SwingPoint1;
    SerializedProperty SwingPoint2;
    SerializedProperty SwingPoint3;
    SerializedProperty SwingSpeed;

    SerializedProperty rb;
    SerializedProperty fallTimer;

    SerializedProperty SpeedIncrease;
    SerializedProperty upwardsForce;
    SerializedProperty DirectionSource;

    SerializedProperty material;



    private void OnEnable()
    {
        BehaviourType = serializedObject.FindProperty("BehaviourType");
        rotationDuration = serializedObject.FindProperty("RotationDuration");
        Displacement = serializedObject.FindProperty("Displacement");
        DisplacementDuration = serializedObject.FindProperty("DisplacementDuration");
        BounceForce = serializedObject.FindProperty("BounceForce");
        collider = serializedObject.FindProperty("collider");
        Trigger = serializedObject.FindProperty("Trigger");

        SwingPoint1 = serializedObject.FindProperty("SwingPoint1");
        SwingPoint2 = serializedObject.FindProperty("SwingPoint2");
        SwingPoint3 = serializedObject.FindProperty("SwingPoint3");

        SwingSpeed = serializedObject.FindProperty("SwingSpeed");

        rb = serializedObject.FindProperty("rb");
        fallTimer = serializedObject.FindProperty("fallTimer");
        SpeedIncrease = serializedObject.FindProperty("SpeedIncrease");

        upwardsForce = serializedObject.FindProperty("upwardsForce");
        DirectionSource = serializedObject.FindProperty("DirectionSource");
        material = serializedObject.FindProperty("material");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(BehaviourType);
        PlatformScript _scriptController = (PlatformScript)target;
        if (_scriptController.BehaviourType == PlatformScript.PlatformBehaviour.rotatingX ||
            _scriptController.BehaviourType == PlatformScript.PlatformBehaviour.rotatingY ||
            _scriptController.BehaviourType == PlatformScript.PlatformBehaviour.rotatingZ)
        {
            EditorGUILayout.PropertyField(rotationDuration);
        }

        else if (_scriptController.BehaviourType == PlatformScript.PlatformBehaviour.LineMove)
        {
            EditorGUILayout.PropertyField(Displacement);
            EditorGUILayout.PropertyField(DisplacementDuration);
            EditorGUILayout.PropertyField(material);
        }
        else if (_scriptController.BehaviourType == PlatformScript.PlatformBehaviour.Bounce)
        {
            EditorGUILayout.PropertyField(BounceForce);
           
        }
        else if (_scriptController.BehaviourType == PlatformScript.PlatformBehaviour.oneWay)
        {
            EditorGUILayout.PropertyField(collider);
            EditorGUILayout.PropertyField(Trigger);
        }
        else if (_scriptController.BehaviourType == PlatformScript.PlatformBehaviour.Swinging)
        {
            EditorGUILayout.PropertyField(SwingPoint1);
            EditorGUILayout.PropertyField(SwingPoint2);
            EditorGUILayout.PropertyField(SwingPoint3);
            EditorGUILayout.PropertyField(SwingSpeed);

        }
        else if (_scriptController.BehaviourType == PlatformScript.PlatformBehaviour.FallThrough)
        {
            EditorGUILayout.PropertyField(rb);
            EditorGUILayout.PropertyField(fallTimer);
            

        }
        else if (_scriptController.BehaviourType == PlatformScript.PlatformBehaviour.Speeding)
        {
            EditorGUILayout.PropertyField(SpeedIncrease);
            


        }
        else if (_scriptController.BehaviourType == PlatformScript.PlatformBehaviour.UpWardBlowers)
        {
            EditorGUILayout.PropertyField(DirectionSource);
            EditorGUILayout.PropertyField(upwardsForce);



        }
        serializedObject.ApplyModifiedProperties();
    }
}
