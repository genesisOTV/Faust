using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DialogueEditor : EditorWindow
{
    public Dialogue dialogue; 

    private string streamingAssetsPath = "/StreamingAssets";

    Vector2 scrollPos; 

    [MenuItem ("Window/Dialogue Editor")]
    static void Init()
    {
        DialogueEditor window = (DialogueEditor)EditorWindow.GetWindow(typeof(DialogueEditor));
        window.Show();
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        if (dialogue != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("dialogue");

            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Save dialogue"))
            {
                SaveDialogue();
            }
        }

        if(GUILayout.Button("Load dialogue"))
        {
            LoadDialogue();
        }

        EditorGUILayout.EndScrollView();
    }

    private void LoadDialogue()
    {
        string path = EditorUtility.OpenFilePanel("Select JSON file", Application.dataPath + streamingAssetsPath, "json");

        if(File.Exists(path))
        {
            string jsongString = File.ReadAllText(path);
            dialogue = JsonUtility.FromJson<Dialogue>(jsongString);
        }
        else
        {
            dialogue = new Dialogue();
        }
    }

    private void SaveDialogue()
    {
        string jsonString = JsonUtility.ToJson(dialogue);
        string path = EditorUtility.SaveFilePanel("Save changes", Application.dataPath + streamingAssetsPath, dialogue.Name + "Dialogue.json", "json");
        File.WriteAllText(path, jsonString);
    }
}
