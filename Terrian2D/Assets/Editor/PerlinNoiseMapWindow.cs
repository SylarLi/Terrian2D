using UnityEditor;
using UnityEngine;
using System.IO;

public class PerlinNoiseMapWindow : EditorWindow
{
    [MenuItem("Window/PerlinNoiseMap")]
    public static void OpenWindow()
    {
        EditorWindow.GetWindow<PerlinNoiseMapWindow>();
    }

    [SerializeField]
    private PerlinNoiseMapInfo info;

    private void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;
        if (info == null)
        {
            info = new PerlinNoiseMapInfo();
        }
    }

    private void OnGUI()
    {
        info.OnGUI();
        if (GUILayout.Button("Sample HeightMap & NormalMap"))
        {
            PerlinNoiseMap map = new PerlinNoiseMap(info);
            Texture2D heightMap = null;
            Texture2D normalMap = null;
            map.Sample(out heightMap, out normalMap);

            byte[] bytes1 = heightMap.EncodeToPNG();
            string heightMapPath = EditorUtility.SaveFilePanel("Save HeightMap", @"E:\UnityProject\Test1\Assets\Resources\", "HeightMap", "png");
            if (!string.IsNullOrEmpty(heightMapPath))
            {
                File.WriteAllBytes(heightMapPath, bytes1);
                Debug.Log("HeightMap: " + heightMapPath);
            }

            byte[] bytes2 = normalMap.EncodeToPNG();
            string normalMapPath = EditorUtility.SaveFilePanel("Save NormalMap", @"E:\UnityProject\Test1\Assets\Resources\", "NormalMap", "png");
            if (!string.IsNullOrEmpty(normalMapPath))
            {
                File.WriteAllBytes(normalMapPath, bytes2);
                Debug.Log("NormalMap: " + normalMapPath);
            }
        }
    }
}
