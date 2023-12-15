using System.Numerics;
using SixLabors.ImageSharp;

namespace ThingWithRays.Types;

public class SceneLight
{
    public Vector3 Position;
    public float Power;
    public float Dropoff = 1;
    public Color LightColor;
}