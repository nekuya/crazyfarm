using UnityEngine;

public static class BoundsExtensions
{
    public static Vector3 RandomPoint(this Bounds bounds)
    {
        // Generate a random point within the bounds
        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );

        return randomPoint;
    }
}