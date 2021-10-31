using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldGenerator))]
public class FieldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(25);

        //Recup les données du script "FieldGenerator"
        FieldGenerator fieldGenerator = (FieldGenerator)target;


        #region Object Parameters
        //Set le display dans l'inspector
        GUILayout.Label("Object Parameters", EditorStyles.boldLabel);

        GUILayout.Space(15);

        fieldGenerator.spawnAmmount = EditorGUILayout.IntField("Spawn Ammount : ", fieldGenerator.spawnAmmount);

        GUILayout.Space(10);

        GUILayout.Label("Random Rotation : ", EditorStyles.label);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        fieldGenerator.minRandomRotation.x = EditorGUILayout.FloatField("x : ", fieldGenerator.minRandomRotation.x, GUILayout.ExpandWidth(false));
        fieldGenerator.minRandomRotation.y = EditorGUILayout.FloatField("y : ", fieldGenerator.minRandomRotation.y, GUILayout.ExpandWidth(false));
        fieldGenerator.minRandomRotation.z = EditorGUILayout.FloatField("z : ", fieldGenerator.minRandomRotation.z, GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.MinMaxSlider(ref fieldGenerator.minRandomRotation.x, ref fieldGenerator.maxRandomRotation.x, -180, 180);
        EditorGUILayout.MinMaxSlider(ref fieldGenerator.minRandomRotation.y, ref fieldGenerator.maxRandomRotation.y, 0, 360);
        EditorGUILayout.MinMaxSlider(ref fieldGenerator.minRandomRotation.z, ref fieldGenerator.maxRandomRotation.z, -180, 180);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        fieldGenerator.maxRandomRotation.x = EditorGUILayout.FloatField(fieldGenerator.maxRandomRotation.x, GUILayout.MaxWidth(50));
        fieldGenerator.maxRandomRotation.y = EditorGUILayout.FloatField(fieldGenerator.maxRandomRotation.y, GUILayout.MaxWidth(50));
        fieldGenerator.maxRandomRotation.z = EditorGUILayout.FloatField(fieldGenerator.maxRandomRotation.z, GUILayout.MaxWidth(50));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        fieldGenerator.minRandomScale = EditorGUILayout.FloatField("Random Scale : ", fieldGenerator.minRandomScale, GUILayout.ExpandWidth(false));
        EditorGUILayout.MinMaxSlider(ref fieldGenerator.minRandomScale, ref fieldGenerator.maxRandomScale, 0, 2);
        fieldGenerator.maxRandomScale = EditorGUILayout.FloatField(fieldGenerator.maxRandomScale, GUILayout.MaxWidth(50));

        EditorGUILayout.EndHorizontal();
#endregion

        GUILayout.Space(25);

        #region Spawner Parameters
        GUILayout.Label("Spawner Parameters", EditorStyles.boldLabel);
        
        GUILayout.Space(15);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Circle Form"))
        {
            fieldGenerator.SelectCircle();
        }

        if (GUILayout.Button("Squarre Form"))
        {
            fieldGenerator.SelectSquarre();
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(15);

        if (fieldGenerator.whichSpawner == 0)
        {
            GUILayout.Label("Spawn in circle form : ", EditorStyles.label);

            fieldGenerator.spawnRadius = EditorGUILayout.FloatField("Radius : ", fieldGenerator.spawnRadius);
        }
        else if (fieldGenerator.whichSpawner == 1)
        {
            GUILayout.Label("Spawn in squarre form : ", EditorStyles.label);

            EditorGUILayout.BeginHorizontal();

            fieldGenerator.spawnWidth = EditorGUILayout.FloatField("Width (x) : ", fieldGenerator.spawnWidth);
            fieldGenerator.spawnLength = EditorGUILayout.FloatField("Lenght (z) : ", fieldGenerator.spawnLength);

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        GUILayout.Space(25);

        #region Editor Button
        GUILayout.Label("Editor Action", EditorStyles.boldLabel);

        GUILayout.Space(15);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Regenerate Field"))
        {
            fieldGenerator.RegenerateField();
        }

        if (GUILayout.Button("Delete Field"))
        {
            fieldGenerator.ResetField();
        }

        EditorGUILayout.EndHorizontal();
        #endregion
    }
}