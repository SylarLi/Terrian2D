using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Terrian2DEditorWindow : EditorWindow 
{
    [MenuItem("Window/Terrian2D")]
    public static void Init()
    {
        EditorWindow.GetWindow<Terrian2DEditorWindow>();
    }

    private Vector2 pivot = Vector2.zero;

    private Vector2 unit = Vector2.one;

    private List<Sprite> sprites = new List<Sprite>();

    private List<RenderTexture> textures = new List<RenderTexture>();

    private Sprite brush;

    private void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    private void OnGUI()
    {
        MainGUI();
        HandleEvent();
    }

    private void MainGUI()
    {
        pivot = EditorGUILayout.Vector2Field("原点", pivot);

        unit = EditorGUILayout.Vector2Field("单位块大小", unit);

        Sprite sprite = EditorGUILayout.ObjectField("添加sprite", null, typeof(Sprite), false) as Sprite;
        if (sprite != null && sprites.IndexOf(sprite) == -1)
        {
            Material material = new Material(Shader.Find("Terrian2D/Rtt2Tt2d"));
            material.mainTextureScale = new Vector2(sprite.textureRect.width / sprite.texture.width, sprite.textureRect.height / sprite.texture.height);
            material.SetFloat("_OffsetX", sprite.textureRect.xMin / sprite.texture.width);
            material.SetFloat("_OffsetY", sprite.textureRect.yMin / sprite.texture.height);
            RenderTexture rt = RenderTexture.GetTemporary((int)sprite.textureRect.width, (int)sprite.textureRect.height, 32, RenderTextureFormat.ARGB32);
            RenderTexture.active = rt;
            Graphics.Blit(sprite.texture, rt, material);
            RenderTexture.active = null;
            textures.Add(rt);
            sprites.Add(sprite);
        }

        for (int i = sprites.Count - 1; i >= 0; i--)
        {
            if (sprites[i] == null)
            {
                sprites.RemoveAt(i);
            }
        }

        GUILayout.BeginHorizontal();
        float totalw = 0;

        int index = sprites.IndexOf(brush);
        for (int i = 0; i < sprites.Count; i++)
        {
            Vector2 btnSize = new Vector2(40 * sprites[i].textureRect.width / sprites[i].textureRect.height, 40);
            totalw += btnSize.x;
            if (totalw > position.xMax - position.xMin)
            {
                GUILayout.EndHorizontal();
                GUILayout.Space(4);
                GUILayout.BeginHorizontal();
            }
            bool toggle = GUILayout.Toggle(i == index, textures[i], EditorStyles.miniButton, GUILayout.Width(btnSize.x), GUILayout.Height(btnSize.y));
            if (i == index)
            {
                index = toggle ? index : -1;
            }
            else
            {
                index = toggle ? i : index;
            }
            if (toggle)
            {
                Rect cr = EditorGUILayout.GetControlRect(GUILayout.Width(-4), GUILayout.Height(0));
                Rect scr = new Rect(cr.xMin - btnSize.x - 2, cr.yMin + btnSize.y, btnSize.x - 3, 2);
                EditorGUI.DrawRect(scr, Color.green);
            }
        }
        brush = index >= 0 && index < sprites.Count ? sprites[index] : null;

        GUILayout.EndHorizontal();
    }

    private void HandleEvent()
    {
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.keyUp:
                {
                    if (e.keyCode == KeyCode.Delete)
                    {
                        if (e.shift)
                        {
                            if (EditorUtility.DisplayDialog("操作确认", "删除所有sprite?", "删除", "取消"))
                            {
                                sprites.Clear();
                                textures.ForEach((RenderTexture each) => each.Release());
                                textures.Clear();
                                brush = null;
                                Repaint();
                            }
                        }
                        else
                        {
                            if (brush != null)
                            {
                                int index = sprites.IndexOf(brush);
                                sprites.RemoveAt(index);
                                textures[index].Release();
                                textures.RemoveAt(index);
                                brush = null;
                                Repaint();
                            }
                        }
                        e.Use();
                    }
                    break;
                }
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.MouseDown:
                {
                    if (e.button == 0)
                    {
                        if (brush != null)
                        {
                            BrushUnitGrid(HandleUtility.GUIPointToWorldRay(e.mousePosition).origin);
                            e.Use();
                        }
                    }
                    break;
                }
        }
    }

    private void BrushUnitGrid(Vector2 loc)
    {
        loc.x = Mathf.Floor(loc.x / unit.x) * unit.x;
        loc.y = Mathf.Floor(loc.y / unit.y) * unit.y;
        GameObject go = new GameObject(brush.name);
        go.transform.position = new Vector3(loc.x + unit.x / 2, loc.y + unit.y / 2);
        go.transform.localScale = new Vector3(unit.x / brush.bounds.size.x, unit.y / brush.bounds.size.y, 1);
        SpriteRenderer render = go.AddComponent<SpriteRenderer>();
        render.sprite = brush;
        render.sortingOrder = 0;
    }
}