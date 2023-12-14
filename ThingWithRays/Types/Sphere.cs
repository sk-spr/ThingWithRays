using System.Diagnostics;
using SixLabors.ImageSharp;
using System.Numerics;

namespace ThingWithRays.Types;

public class Sphere : ISceneObject
{
    public Sphere(Vector3 position, float radius, Color color) //TODO materials
    {
        Radius = radius;
        Position = position;
        _objectColor = color;
    }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    private Color _objectColor;
    public float Radius { get; private set; }
    public RayCastResult Intersect(Ray ray)
    {
        RayCastResult r = new RayCastResult();
        Vector3 L = Position - ray.StartPoint;
        float tc = Vector3.Dot(L, ray.Direction);
        if (tc < 0.0)
        {
            //Console.WriteLine("Did not hit due to dot under 0.0");
            r.DidHit = false;
            return r;
        }

        float d2 = Vector3.Dot(L, L) - tc * tc;
        float radius2 = Radius * Radius;
        if (d2 >= radius2)
        {
            //Console.WriteLine("d2 < radius2");
            r.DidHit = false;
            return r;
        }

        float t1c = float.Sqrt(radius2 - d2);
        r.DidHit = true;
        float t1 = tc - t1c;
        float t2 = tc + t1c;
        r.HitDistEntry = float.Min(t1, t2);
        r.HitDistExit = float.Max(t1, t2);
        r.HitColor = _objectColor;
        return r;
    }
}