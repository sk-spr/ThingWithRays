using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ThingWithRays.Types;

public class Scene
{
    public List<Camera> Cameras = new();
    public List<ISceneObject> SceneObjects = new();

    public Color TraceRay(Ray ray)
    {
        var resultsL1 = SceneObjects.Select((obj) => obj.Intersect(ray)).ToArray();
        var rayCastResults = resultsL1.Length == 0 ? Array.Empty<RayCastResult>() :
                             resultsL1.Where(r => r.DidHit)
                                 .OrderBy(res => res.HitDistEntry)
                                 .ToArray();
        if (rayCastResults.Length == 0)
            return Color.Black;
        var col = (rayCastResults.First().HitColor ?? Color.Black);
        var pix = col.ToPixel<Bgr24>();
        //test
        float adjustment = 1f - (rayCastResults.First().HitDistEntry ?? 0.0f) / 12f;
        return new Color(new Bgr24((byte)(pix.R * adjustment), (byte)(pix.G * adjustment), (byte)(pix.B * adjustment)));
    }
}