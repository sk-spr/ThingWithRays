using SixLabors.ImageSharp;
using System.Numerics;

namespace ThingWithRays.Types;

public struct RayCastResult
{
    public ISceneObject CastAgainst;
    public bool DidHit;
    public Vector3? Normal;
    public float? HitDistEntry;
    public float? HitDistExit;
    public Color? HitColor;
    public Ray? ReflectionRay;
}