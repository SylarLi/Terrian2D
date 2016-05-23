using System.Collections.Generic;
using UnityEngine;

public class WaterSurface
{
    public Vector2 size = new Vector2(1, 1);

    public float waveNums = 12;

    public float maxWaveAmplitude = 0.04f;

    public List<Wave> waves = new List<Wave>();

    public WaterSurface()
    {
        RandomWaves();
    }

    public void RandomWaves()
    {
        waves.Clear();
        for (int i = 0; i < waveNums; i++)
        {
            Wave wave = new Wave();
            wave.length = Random.Range(0.2f, 0.4f);
            wave.speed = Random.Range(0f, 1f);
            wave.amplitude = Random.Range(0f, maxWaveAmplitude);
            wave.direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            wave.k = 1.2f;
            waves.Add(wave);
        }
    }

    public float SampleHeight(float x, float z, float t)
    {
        float height = 0;
        x = Mathf.Clamp(x, 0, size.x);
        z = Mathf.Clamp(z, 0, size.y);
        waves.ForEach((Wave wave) =>
        {
            height += wave.GetHeight(x, z, t);
        });
        return height;
    }

    public Vector3 SampleNormal(float x, float z, float t)
    {
        Vector3 normal = new Vector3(0, 0, 1);
        x = Mathf.Clamp(x, 0, size.x);
        z = Mathf.Clamp(z, 0, size.y);
        waves.ForEach((Wave wave) =>
        {
            normal.x -= wave.GetHeightDerivative_X(x, z, t);
            normal.y -= wave.GetHeightDerivative_Y(x, z, t);
        });
        normal = normal.normalized;
        return normal;
    }
}
