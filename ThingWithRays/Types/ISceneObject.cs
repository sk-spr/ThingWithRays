using System.Numerics;

namespace ThingWithRays.Types;

public interface ISceneObject
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Material Material { get; set; }
    public abstract RayCastResult Intersect(Ray ray);
}