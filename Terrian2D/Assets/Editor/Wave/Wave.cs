using UnityEngine;

public class Wave
{
    public float length = 0.5f;

    public float amplitude = 0.2f;

    public float speed = 0.5f;

    public Vector2 direction = new Vector2(0.5f, 0.5f);

    public float k = 1;

    public float w
    {
        get
        {
            return 2 * Mathf.PI / length;
        }
    }

    public float GetHeight(float x, float z, float t)
    {
        float dot = Vector2.Dot(Vector3.Normalize(direction), new Vector2(x, z));
        return 2 * amplitude * Mathf.Pow((Mathf.Sin(dot * w + t * speed * w) + 1) / 2, k);
    }

    public float GetHeightDerivative_X(float x, float z, float t)
    {
        float dot = Vector2.Dot(Vector3.Normalize(direction), new Vector2(x, z));
        float rds = dot * w + t * speed * w;
        return k * Vector3.Normalize(direction).x * w * amplitude * Mathf.Pow((Mathf.Sin(rds) + 1) / 2, k - 1) * Mathf.Cos(rds);
    }

    public float GetHeightDerivative_Y(float x, float z, float t)
    {
        float dot = Vector2.Dot(Vector3.Normalize(direction), new Vector2(x, z));
        float rds = dot * w + t * speed * w;
        return k * Vector3.Normalize(direction).y * w * amplitude * Mathf.Pow((Mathf.Sin(rds) + 1) / 2, k - 1) * Mathf.Cos(rds);
    }
}
