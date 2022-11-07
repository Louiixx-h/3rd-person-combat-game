using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayMenu : EditorWindow
{
    private VisualElement _root;

    [MenuItem("Window/UI Toolkit/GameplayMenu")]
    public static void ShowExample()
    {
        GameplayMenu wnd = GetWindow<GameplayMenu>();
        wnd.titleContent = new GUIContent("GameplayMenu");
    }

    public void CreateGUI()
    {
        _root = rootVisualElement;
    }
}