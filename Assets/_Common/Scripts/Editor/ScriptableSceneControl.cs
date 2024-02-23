using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;


[FilePath("ProjectSettings/SceneControl.asset", FilePathAttribute.Location.ProjectFolder)]
public class ScriptableSceneControl : ScriptableSingleton<ScriptableSceneControl>
{
    List<SceneAsset> _baseScenes = new List<SceneAsset>();
    [SerializeField] List<SceneAsset> _addedScenes = new List<SceneAsset>();

    public static List<SceneAsset> Scenes => instance._baseScenes.Concat(instance._addedScenes).ToList(); // All availables scenes from control
    public static List<SceneAsset> BaseScenes => instance._baseScenes;    // Scenes in Assets/Scenes
    public static List<SceneAsset> CustomScenes => instance._addedScenes; //Custom added scenes
    static string SceneFolderpath = Application.dataPath + "/_SheepFold/Scenes";

    public static void RetrieveScenes()
    {
        if (!Directory.Exists(SceneFolderpath))
            return;

        string[] fileEntries = Directory.GetFiles(SceneFolderpath);
        List<SceneAsset> scenes = instance._baseScenes;

        scenes.Clear();
        foreach (string path in fileEntries)
        {
            if (Path.GetExtension(path) == ".meta")
                continue;

            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            scenes.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(relativePath));
        }
    }

    public static void AddScene(SceneAsset scn)
    {
        if (Scenes.Contains(scn)) return;
        instance._addedScenes.Add(scn);
        instance.Save(true);
    }

    public static void RemoveScene(SceneAsset scn)
    {
        if (!instance._addedScenes.Contains(scn)) return;
        instance._addedScenes.Remove(scn);
        instance.Save(true);
    }

    public static void RemoveInvalidScenes()
    {
        for (int i = instance._baseScenes.Count - 1; i >= 0; i--)
            if (instance._baseScenes[i] == null)
                instance._baseScenes.RemoveAt(i);

        for (int i = instance._addedScenes.Count - 1; i >= 0; i--)
            if (instance._addedScenes[i] == null)
                instance._addedScenes.RemoveAt(i);
    }
}