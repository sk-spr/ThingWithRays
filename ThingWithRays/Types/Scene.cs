using System.Diagnostics;
using System.Numerics;
using Gtk;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ThingWithRays.Types;

public class Scene
{
    public List<Camera> Cameras = new();
    public List<ISceneObject> SceneObjects = new();
    public List<SceneLight> Lights = new();

    public Color TraceRay(Ray ray, int maximumBounces)
    {
        var resultsL1 = SceneObjects.Select((obj) => obj.Intersect(ray)).ToArray();
        var rayCastResults = resultsL1.Length == 0 ? Array.Empty<RayCastResult>() :
                             resultsL1.Where(r => r.DidHit)
                                 .OrderBy(res => res.HitDistEntry)
                                 .ToArray();
        if (rayCastResults.Length == 0)
            return Color.Purple;
        if (maximumBounces <= 0)
            return Color.Turquoise;//rayCastResults[0].HitColor.GetValueOrDefault();
        //if(rayCastResults[0].CastAgainst.Material.DoesReflect) Console.WriteLine(rayCastResults[0].CastAgainst.Material.DoesReflect);
        var col = rayCastResults[0].CastAgainst.Material.DoesReflect
            ? TraceRay(new Ray
            {
                Direction = Vector3.Reflect(ray.Direction, rayCastResults[0].Normal ?? Vector3.UnitY),
                StartPoint = ray.StartPoint + ray.Direction * rayCastResults[0].HitDistEntry.GetValueOrDefault()
            }, maximumBounces - 1)
            : rayCastResults.First().HitColor.GetValueOrDefault();
        /*rayCastResults[0].CastAgainst.Material.DoesReflect
           ? TraceRay(new Ray
           {
               Direction = Utils.TracingUtils.GetReflection(ray.Direction, rayCastResults[0].Normal ?? Vector3.UnitY),
               StartPoint = ray.StartPoint + ray.Direction * rayCastResults[0].HitDistEntry.GetValueOrDefault()
           }, maximumBounces - 1)*/
        var pix = col.ToPixel<Bgr24>();
        Vector3 intersectionPosition = ray.StartPoint + ray.Direction * rayCastResults[0].HitDistEntry.GetValueOrDefault();
        float adjustment = 0f;
        //TODO make this work properly
        if(!rayCastResults[0].CastAgainst.Material.DoesReflect)
        {
            bool hitAny = false;
            foreach (var light in Lights)
            {
                var lightDir = Vector3.Normalize(light.Position - intersectionPosition);
                var directLightRays = SceneObjects.Select(obj => obj.Intersect(new()
                {
                    Direction = lightDir,
                    StartPoint = intersectionPosition
                }));
                if (!directLightRays.Any(result => result.DidHit))
                {
                    //we have a direct path to the light
                    adjustment += (light.Power /
                                  (Vector3.DistanceSquared(Vector3.Zero, light.Position - intersectionPosition) *
                                   light.Dropoff)) // distance attenuation
                                 *Single.Max(0.0f, Vector3.Dot(rayCastResults[0].Normal.GetValueOrDefault(), -lightDir));
                    hitAny = true;
                    //var lightPix = Lights[0].LightColor.ToPixel<Bgr24>();
                }
            }
            adjustment = float.Min(1f, adjustment);
            if(hitAny) // TODO add light color back in
                return new Color(new Bgr24((byte)(pix.R * adjustment),
                    (byte)(pix.G* adjustment), (byte)(pix.B * adjustment)));
            //return Color.Magenta;
            //return new Color(new Bgr24((byte)(adjustment * 255), (byte)(adjustment * 255), (byte)(adjustment * 255)));
            return Color.Black;
        }

        //lighting
        //return new Color(new Bgr24((byte)(pix.R * adjustment), (byte)(pix.G * adjustment), (byte)(pix.B * adjustment)));
        return col;
        return new Color(new Bgr24((byte)(pix.R * 0.2 + pix.R * adjustment), (byte)(pix.G * 0.2 + pix.G * adjustment), (byte)(pix.B * 0.2 + pix.B * adjustment)));
    }
}