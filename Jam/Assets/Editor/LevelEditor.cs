using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow(){
        GetWindow<LevelEditor>("LevelEditor");
    }

    void OnGUI()
    {
        
    }
}
