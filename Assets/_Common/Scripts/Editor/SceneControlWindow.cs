using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneControlWindow : EditorWindow
{
    [MenuItem("Tools/Scene Controller")]
    public static SceneControlWindow ShowWindow()
    {
        SceneControlWindow window = GetWindow<SceneControlWindow>("Scene Controller", true);
        window.minSize = new Vector2(175f, 300f);
        window.Show();

        return window;
    }

    private bool _showAssetSceneFolder = true;
    private bool _showCustomSceneFolder = true;
    Vector2 _scrollPos;

    private void SafeSceneSwap(Object scn)
    {
        if (EditorSceneManager.GetActiveScene().isDirty && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(scn), OpenSceneMode.Single);
    }

    private void OnGUI()
    {
        ScriptableSceneControl.RemoveInvalidScenes();

        bool deleting = Event.current.control;
        bool hasCustomScenes = ScriptableSceneControl.CustomScenes.Count > 0;
        GUIStyle style = EditorStyles.label;
        style.stretchHeight = true;

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, style);
        {
            if (ScriptableSceneControl.BaseScenes.Count > 0)
            {
                _showAssetSceneFolder = EditorGUILayout.Foldout(_showAssetSceneFolder, "Assets/Scenes");
                foreach (SceneAsset scenePair in ScriptableSceneControl.BaseScenes)
                {
                    if (!_showAssetSceneFolder) break;

                    if (GUILayout.Button(scenePair.name) && !deleting)
                        SafeSceneSwap(scenePair);
                }
            }

            if (hasCustomScenes)
            {
                _showCustomSceneFolder = EditorGUILayout.Foldout(_showCustomSceneFolder, "Custom Scenes");
                foreach (SceneAsset scenePair in ScriptableSceneControl.CustomScenes)
                {
                    if (!_showCustomSceneFolder) break;

                    if (!deleting)
                    {
                        if (GUILayout.Button(scenePair.name))
                            SafeSceneSwap(scenePair);
                    }
                    else
                    {
                        if (GUILayout.Button("Remove " + scenePair.name + " from list"))
                        {
                            ScriptableSceneControl.RemoveScene(scenePair);
                            EditorGUILayout.EndScrollView();
                            return;
                        }
                    }
                }
            }
        }
        EditorGUILayout.EndScrollView();


        GUILayout.FlexibleSpace();
        object[] dropped = DropZone("Drop new scene", (int)position.width, 100);

        if (hasCustomScenes)
        {
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.Label("Hold Ctrl to delete a custom scene", centeredStyle);
        }

        if (dropped == null || dropped.Length <= 0) return;

        foreach (object drop in dropped)
        {
            if (drop is not SceneAsset)
                continue;

            ScriptableSceneControl.AddScene((SceneAsset)drop);
        }
    }

    private static object[] DropZone(string title, int w, int h)
    {
        Color lastColor = GUI.color;
        GUI.color = Color.green;
        GUILayout.Box(title, GUILayout.Width(w), GUILayout.Height(h));
        GUI.color = lastColor;

        EventType eventType = Event.current.type;
        bool isAccepted = false;

        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (eventType == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                isAccepted = true;
            }
            Event.current.Use();
        }

        return isAccepted ? DragAndDrop.objectReferences : null;
    }

    #region UNITY_CALLBACKS
    private void OnEnable() => ScriptableSceneControl.RetrieveScenes();
    private void OnFocus() => ScriptableSceneControl.RetrieveScenes();
    #endregion
}

