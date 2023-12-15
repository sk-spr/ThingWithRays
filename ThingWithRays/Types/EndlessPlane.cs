using System.Numerics;
using SixLabors.ImageSharp;

namespace ThingWithRays.Types;

public class EndlessPlane : ISceneObject
{
    public EndlessPlane(Vector3 position, Vector3 normal, Color color)
    {
        Position = position;
        Rotation = Quaternion.Identity;
        Normal = normal;
        Color = color;
    }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Material Material { get; set; }
    public Vector3 Normal;
    public Color Color;
    public RayCastResult Intersect(Ray ray)
    {
        RayCastResult result = new()
        {
            CastAgainst = this
        };
        // assuming vectors are all normalized
        float denom = Vector3.Dot(-Normal, ray.Direction);
        if (denom > 0.001f) {
            var p0l0 = Position - ray.StartPoint;
            var t = Vector3.Dot(p0l0, -Normal) / denom;
            result.DidHit = (t >= 0);
            result.HitDistEntry = t;
            result.HitDistExit = t;
            result.HitColor = Color;
            result.Normal = -Normal;
            return result;
        }

        result.DidHit = false;
        return result;

    }
}