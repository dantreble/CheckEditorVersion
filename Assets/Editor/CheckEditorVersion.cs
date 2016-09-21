using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[InitializeOnLoad]
public class CheckEditorVersion : EditorWindow
{
    private static string m_productName = "Hand of Fate 2";
    private static string m_desiredVersion = @"5.4.1"; //5.4.1f1 (649f48bbbf0f)
   
    private static string m_desiredVersionWinURL =
        @"http://netstorage.unity3d.com/unity/649f48bbbf0f/Windows64EditorInstaller/UnitySetup64-5.4.1f1.exe";

    private static string m_desiredVersionOSXURL =
        @"http://netstorage.unity3d.com/unity/649f48bbbf0f/MacEditorInstaller/Unity-5.4.1f1.pkg";

    static CheckEditorVersion()
    {
        var skip = EditorPrefs.GetString(SkipKey()) == m_desiredVersion;  

        var fullUnityVersion = InternalEditorUtility.GetFullUnityVersion();

        if (!skip && !fullUnityVersion.StartsWith(m_desiredVersion))
        {
            ShowWindow();
        }
    }

    private static string SkipKey()
    {
        //get_productName is not allowed to be called from a ScriptableObject constructor
        //return Application.productName + "SkipVersionString";
        return m_productName + "SkipVersionString";
    }

    private static void ShowWindow()
    {
        GetWindowWithRect(typeof (CheckEditorVersion), new Rect(100f, 100f, 570f, 400f), true, "Update Unity");
    }

    public void OnGUI()
    {
        GUILayout.Label(
            "Project " + Application.productName + " requires unity version " + m_desiredVersion +
            ", you are currently on " + InternalEditorUtility.GetFullUnityVersion() + " please update",
            "WordWrappedLabel");

        GUILayout.Space(20f);

        if (GUILayout.Button("Download new version"))
        {
#if  UNITY_EDITOR_WIN
            Help.BrowseURL(m_desiredVersionWinURL);
#else
            Help.BrowseURL(m_desiredVersionOSXURL);
#endif
        }

        if (GUILayout.Button("Ignore"))
        {
            EditorPrefs.SetString(SkipKey(), m_desiredVersion);
        }
    }
}