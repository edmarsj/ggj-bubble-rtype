using UnityEngine;

public static class BoundsExtensions
{
    public static Vector2 GetRandom2DPosition(this Bounds instance) =>
                new(Random.Range(instance.min.x, instance.max.x),
                    Random.Range(instance.min.y, instance.max.y));
}
