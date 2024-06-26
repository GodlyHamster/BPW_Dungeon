using UnityEngine;

public static class Vector2IntExtension
{
    /// <summary>
    /// Rotates a Vector2Int around the zero point using the degrees as parameter
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static Vector2Int Rotate(this Vector2Int vec, float degrees)
    {
        degrees = Mathf.Deg2Rad * degrees;
        Vector2Int newPos = new Vector2Int(
            Mathf.RoundToInt(vec.x * Mathf.Cos(degrees) - vec.y * Mathf.Sin(degrees)),
            Mathf.RoundToInt(vec.x * Mathf.Sin(degrees) + vec.y * Mathf.Cos(degrees))
            );
        return newPos;
    }
}
