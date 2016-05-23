using System.IO;
using UnityEditor;
using UnityEngine;

public class WaterSurfaceWindow : EditorWindow
{
    [MenuItem("Window/WaterSurface")]
    public static void Open()
    {
        EditorWindow.GetWindow<WaterSurfaceWindow>().Show();
    }

    [SerializeField]
    private WaterSurface surface;

    [SerializeField]
    private float t = 0;

    private void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;

        if (surface == null)
        {
            surface = new WaterSurface();
        }
    }

    private void OnGUI()
    {
        t = EditorGUILayout.FloatField("Time: ", t);
        if (GUILayout.Button("G"))
        {
            surface.RandomWaves();

            int sampleX = 512;
            int sampleZ = 512;
            Color[] heights = new Color[sampleX * sampleZ];
            Color[] normals = new Color[sampleX * sampleZ];
            float pieceX = surface.size.x / sampleX;
            float pieceZ = surface.size.y / sampleZ;
            for (int z = 0; z < sampleZ; z++)
            {
                for (int x = 0; x < sampleX; x++)
                {
                    float height = surface.SampleHeight(x * pieceX, z * pieceZ, t);
                    heights[z * sampleX + x] = new Color(height, height, height);
                    Vector3 normal = surface.SampleNormal(x * pieceX, z * pieceZ, t);
                    normal = (normal + Vector3.one) / 2;
                    normals[z * sampleX + x] = new Color(normal.x, normal.y, normal.z);
                }
            }

            Texture2D tt = new Texture2D(sampleX, sampleZ, TextureFormat.ARGB32, false);
            tt.SetPixels(heights);
            tt.Apply();

            byte[] bytes1 = tt.EncodeToPNG();
            string heightMapPath = EditorUtility.SaveFilePanel("Save HeightMap", @"E:\UnityProject\Test1\Assets\Resources\", "HeightMap", "png");
            if (!string.IsNullOrEmpty(heightMapPath))
            {
                File.WriteAllBytes(heightMapPath, bytes1);
                Debug.Log("HeightMap: " + heightMapPath);
            }

            Texture2D nt = new Texture2D(sampleX, sampleZ, TextureFormat.ARGB32, false);
            nt.SetPixels(normals);
            nt.Apply();

            byte[] bytes2 = nt.EncodeToPNG();
            string normalMapPath = EditorUtility.SaveFilePanel("Save NormalMap", @"E:\UnityProject\Test1\Assets\Resources\", "NormalMap", "png");
            if (!string.IsNullOrEmpty(normalMapPath))
            {
                File.WriteAllBytes(normalMapPath, bytes2);
                Debug.Log("HeightMap: " + normalMapPath);
            }
        }
    }
}
