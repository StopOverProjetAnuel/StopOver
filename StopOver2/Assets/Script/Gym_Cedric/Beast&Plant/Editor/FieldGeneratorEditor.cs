using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldGenerator))]
public class FieldGeneratorEditor : Editor
{
    FieldGenerator fieldGenerator;
    SerializedObject soTarget;

    //Objects Variables
    private SerializedProperty ground;

    bool showSpawns = false;

    //Inspector Styles Variables
    GUIStyle subStyle1;
    GUIStyle subStyle2;
    GUIStyle buttonStyle;
    GUIStyle buttonStyle2;

    private void OnEnable()
    {
        //Recup les données du script "FieldGenerator"
        fieldGenerator = (FieldGenerator)target;
        soTarget = new SerializedObject(target);

        ////

        ground = soTarget.FindProperty("ground");
    }

    //Edite l'inspector Unity
    public override void OnInspectorGUI()
    {
        #region Styles

        subStyle1 = new GUIStyle("box");
        subStyle1.normal.background = MakeTex(1, 1, new Color(0.3f, 0.3f, 0.3f, 1f));
        subStyle1.normal.textColor = Color.black;

        subStyle2 = new GUIStyle("box");
        subStyle2.normal.background = MakeTex(1, 1, new Color(0.4f, 0.4f, 0.4f, 1f));
        subStyle2.normal.textColor = Color.black;

        buttonStyle = new GUIStyle("Button");
        buttonStyle.normal.background = MakeTex(1, 1, new Color(0.8f, 0.2f, 0.2f, 0.8f));
        buttonStyle.normal.textColor = Color.white;

        buttonStyle2 = new GUIStyle("Button");
        buttonStyle2.normal.background = MakeTex(1, 1, new Color(0.2f, 0.6f, 0.2f, 0.8f));
        buttonStyle2.normal.textColor = Color.white;

        #endregion

        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        #region Object Parameters
        {
            //Set le display dans l'inspector
            GUILayout.Label("Object Parameters", EditorStyles.boldLabel);

            #region Liste "Field"
            EditorGUILayout.BeginVertical("box"); //Set la liste de GameObject avec leurs poids
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                {
                    if (!showSpawns)
                    {
                        if (GUILayout.Button(" Show Prefabs (" + fieldGenerator.field.Count + ")", buttonStyle2, GUILayout.MaxHeight(20f)))
                        {
                            showSpawns = true;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(" Hide Prefabs (" + fieldGenerator.field.Count + ")", buttonStyle, GUILayout.MaxHeight(20f)))
                        {
                            showSpawns = false;
                        }
                    }
                    if (GUILayout.Button(" Add Prefab ", buttonStyle2, GUILayout.MaxHeight(20f)))
                    {
                        AddSpawn();
                        if (showSpawns == false)
                        {
                            showSpawns = true;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(5);
                EditorGUILayout.EndVertical();

                //EditorGUILayout.PropertyField(prefabToSpawn);
                if (showSpawns)
                {
                    EditorGUILayout.BeginVertical(subStyle1);
                    {
                        for (int i = 0; i < fieldGenerator.field.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal(subStyle2);
                            {
                                //myObject.prefabToSpawn[i].prefab = (GameObject)EditorGUILayout.ObjectField(myObject.prefabToSpawn[i].prefab, typeof(GameObject));
                                fieldGenerator.field[i].spawnObjects = (GameObject)EditorGUILayout.ObjectField(fieldGenerator.field[i].spawnObjects, typeof(GameObject), false, GUILayout.MaxWidth(200f));
                                EditorGUILayout.LabelField("Weight : ", GUILayout.MaxWidth(40f));
                                fieldGenerator.field[i].weight = EditorGUILayout.IntField(fieldGenerator.field[i].weight, GUILayout.MaxWidth(40f));



                                if (GUILayout.Button("X", buttonStyle))
                                {
                                    RemoveSpawn(i);
                                }

                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        //EditorGUILayout.PropertyField(patrolPoints);

                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
            #endregion

            GUILayout.Space(15);

            fieldGenerator.spawnAmmount = EditorGUILayout.IntField("Spawn Ammount : ", fieldGenerator.spawnAmmount);

            GUILayout.Space(15);

            EditorGUILayout.PropertyField(ground, new GUIContent("LayerMask"));

            GUILayout.Space(10);

            GUILayout.Label("Random Rotation : ", EditorStyles.label);

            #region Random rotation avec sliders
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    fieldGenerator.minRandomRotation.x = EditorGUILayout.FloatField("x : ", fieldGenerator.minRandomRotation.x, GUILayout.ExpandWidth(false));
                    fieldGenerator.minRandomRotation.y = EditorGUILayout.FloatField("y : ", fieldGenerator.minRandomRotation.y, GUILayout.ExpandWidth(false));
                    fieldGenerator.minRandomRotation.z = EditorGUILayout.FloatField("z : ", fieldGenerator.minRandomRotation.z, GUILayout.ExpandWidth(false));
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.MinMaxSlider(ref fieldGenerator.minRandomRotation.x, ref fieldGenerator.maxRandomRotation.x, -180, 180);
                    EditorGUILayout.MinMaxSlider(ref fieldGenerator.minRandomRotation.y, ref fieldGenerator.maxRandomRotation.y, 0, 360);
                    EditorGUILayout.MinMaxSlider(ref fieldGenerator.minRandomRotation.z, ref fieldGenerator.maxRandomRotation.z, -180, 180);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    fieldGenerator.maxRandomRotation.x = EditorGUILayout.FloatField(fieldGenerator.maxRandomRotation.x, GUILayout.MaxWidth(50));
                    fieldGenerator.maxRandomRotation.y = EditorGUILayout.FloatField(fieldGenerator.maxRandomRotation.y, GUILayout.MaxWidth(50));
                    fieldGenerator.maxRandomRotation.z = EditorGUILayout.FloatField(fieldGenerator.maxRandomRotation.z, GUILayout.MaxWidth(50));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                fieldGenerator.minRandomScale = EditorGUILayout.FloatField("Random Scale : ", fieldGenerator.minRandomScale, GUILayout.ExpandWidth(false));
                EditorGUILayout.MinMaxSlider(ref fieldGenerator.minRandomScale, ref fieldGenerator.maxRandomScale, 0, 2);
                fieldGenerator.maxRandomScale = EditorGUILayout.FloatField(fieldGenerator.maxRandomScale, GUILayout.MaxWidth(50));
            }
            EditorGUILayout.EndHorizontal();
        }
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

            fieldGenerator.spawnWidthNLength = EditorGUILayout.Vector2Field("Width & Length : ", fieldGenerator.spawnWidthNLength);
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

    void AddSpawn()
    {
        fieldGenerator.field.Add(new FieldGenerator.Field());
    }

    void RemoveSpawn(int index)
    {
        fieldGenerator.field.RemoveAt(index);
        //Debug.Log("Prefab length " + myObject.prefabToSpawn.Count);
        if (fieldGenerator.field.Count == 0)
        {
            showSpawns = false;
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}