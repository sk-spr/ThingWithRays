using SixLabors.ImageSharp;
using System.Numerics;

namespace ThingWithRays.Types;

public struct RayCastResult
{
    public bool DidHit;
    public Vector3? Normal;
    public float? HitDistEntry;
    public float? HitDistExit;
    public Color? HitColor;
}