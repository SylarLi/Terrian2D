using UnityEngine;
using UnityEditor;

public class PerlinNoiseMapInfo : ScriptableObject
{
    public int pixelWidth = 512;

    public int pixelHeight = 512;

    public Vector2 offset = new Vector2(0, 0);

    public Vector2 scale = new Vector2(20, 20);

    public Vector2 heightRange = new Vector2(0, 1);

    public float heightToNormalStrength = 0.5f;

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        Vector2 size = new Vector2(pixelWidth, pixelHeight);
        size = EditorGUILayout.Vector2Field("Texture Size", size);
        pixelWidth = Mathf.FloorToInt(size.x);
        pixelHeight = Mathf.FloorToInt(size.y);
        EditorGUILayout.EndHorizontal();

        offset = EditorGUILayout.Vector2Field("Perlin Offset", offset);
        scale = EditorGUILayout.Vector2Field("Perlin Scale", scale);
        heightRange = EditorGUILayout.Vector2Field("Height Range", heightRange);
        heightToNormalStrength = EditorGUILayout.Slider("Height To Normal Stength", heightToNormalStrength, 0f, 1f);
    }

    public float HeightFromMap(Color color)
    {
        return heightRange.x + (heightRange.y - heightRange.x) * color.r;
    }
}
