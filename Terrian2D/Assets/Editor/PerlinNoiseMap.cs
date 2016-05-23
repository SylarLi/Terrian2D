using UnityEngine;

public class PerlinNoiseMap
{
    private PerlinNoiseMapInfo mMapInfo;

    public PerlinNoiseMap(PerlinNoiseMapInfo mapInfo)
    {
        mMapInfo = mapInfo;
    }

    public void Sample(out Texture2D heightMap, out Texture2D normalMap)
    {
        Color[] colors = new Color[mMapInfo.pixelWidth * mMapInfo.pixelHeight];
        for (int y = 0; y < mMapInfo.pixelHeight; y++)
        {
            for (int x = 0; x < mMapInfo.pixelWidth; x++)
            {
                float px = mMapInfo.offset.x + (float)x / mMapInfo.pixelWidth * mMapInfo.scale.x;
                float py = mMapInfo.offset.y + (float)y / mMapInfo.pixelHeight * mMapInfo.scale.y;
                int index = y * mMapInfo.pixelWidth + x;
                float value = Mathf.PerlinNoise(px, py);
                colors[index] = new Color(value, value, value);
            }
        }
        heightMap = new Texture2D(mMapInfo.pixelWidth, mMapInfo.pixelHeight);
        heightMap.SetPixels(colors);
        heightMap.Apply();

        normalMap = new Texture2D(mMapInfo.pixelWidth, mMapInfo.pixelHeight);
        normalMap.SetPixels(HeightToNormalMap(colors));
        normalMap.Apply();
    }

    private Color[] HeightToNormalMap(Color[] heights)
    {
        Color[] normals = new Color[heights.Length];
        for (int y = 0; y < mMapInfo.pixelHeight; y++)
        {
            for (int x = 0; x < mMapInfo.pixelWidth; x++)
            {
                Vector3 normal = new Vector3();

                int index = y * mMapInfo.pixelWidth + x;
                float left = heights[ContinumIndexFrom(x - 1, y)].r;
                float right = heights[ContinumIndexFrom(x + 1, y)].r;
                float top = heights[ContinumIndexFrom(x, y - 1)].r;
                float bottom = heights[ContinumIndexFrom(x, y + 1)].r;
                float left_top = heights[ContinumIndexFrom(x - 1, y - 1)].r;
                float right_top = heights[ContinumIndexFrom(x + 1, y - 1)].r;
                float left_bottom = heights[ContinumIndexFrom(x - 1, y + 1)].r;
                float right_bottom = heights[ContinumIndexFrom(x + 1, y + 1)].r;

                // Sobel filter
                //  -1 0 1 
                //  -2 0 2
                //  -1 0 1
                normal.x = -left_top - 2 * left - left_bottom + right_top + 2 * right + right_bottom;

                //  -1 -2 -1 
                //  0  0  0
                //  1  2  1
                normal.y = -left_top - 2 * top - right_top + left_bottom + 2 * bottom + right_bottom;

                normal.z = 1;

                normal.Normalize();
                normal = normal * 0.5f + new Vector3(0.5f, 0.5f, 0.5f);
                normals[index] = new Color(normal.x, normal.y, normal.z);
            }
        }
        return normals;
    }

    private int ContinumIndexFrom(int x, int y)
    {
        if (x < 0)
        {
            x = 1;
        }
        else if (x >= mMapInfo.pixelWidth)
        {
            x = mMapInfo.pixelWidth - 2;
        } 
        if (y < 0)
        {
            y = 1;
        }
        else if (y >= mMapInfo.pixelHeight)
        {
            y = mMapInfo.pixelHeight - 2;
        }
        return y * mMapInfo.pixelWidth + x;
    }
}
