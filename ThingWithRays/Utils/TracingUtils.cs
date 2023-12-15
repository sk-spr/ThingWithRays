using System.Numerics;

namespace ThingWithRays.Utils;

public static class TracingUtils
{
    public static Vector3 GetReflection(Vector3 incoming, Vector3 normal)
    {
        //angle of incidence is angle of reflection
        //https://math.stackexchange.com/questions/13261/how-to-get-a-reflection-vector
        return -2 * Vector3.Dot(incoming, normal) * normal - incoming;
    }
}