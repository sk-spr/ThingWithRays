using System.Numerics;

namespace ThingWithRays.Types;

public struct Camera
{
    public float FovX;
    public float FovY;
    public Vector3 Position;
    public Quaternion Rotation;
    public int ResX;
    public int ResY;

    public Ray[] GenRays()
    {
        List<Ray> rays = new();
        for (int y = 0; y < ResY; y++)
        {
            float angleY = (FovY) * ((float) (y - ResY * 0.5f) / (ResY));
            Console.WriteLine(angleY);
            for (int x = 0; x < ResX; x++)
            {
                float angleX = (FovX) * ((float) (x - ResX * 0.5f) / (ResX));
                Vector3 vec = Vector3.Transform(Vector3.UnitZ,
                    Quaternion.CreateFromYawPitchRoll(float.DegreesToRadians(angleY), float.DegreesToRadians(angleX),
                        0f));
                    // Vector3.Transform(
                    //     Vector3.UnitZ,
                    //     Quaternion.CreateFromAxisAngle(
                    //         Vector3.UnitY, 
                    //         float.DegreesToRadians(angleX)))
                    // + Vector3.Transform(Vector3.UnitZ, Quaternion.CreateFromAxisAngle(
                    //     Vector3.UnitY, 
                    //     float.DegreesToRadians(angleX)));
                
                rays.Add(new(){Direction = vec, StartPoint = Position});
            }
        }

        return rays.ToArray();
    }
    
}