using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ThingWithRays.Types;
using System;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using Color = SixLabors.ImageSharp.Color;

Scene scene = new();
Camera cam = new();
cam.FovX = 80;
cam.FovY = 80;
cam.Position = new(0, 0, -10);
cam.Rotation = Quaternion.Identity;
cam.ResX = 100;
cam.ResY = 100;
scene.Cameras.Add(cam);
Sphere testSphere = new(Vector3.UnitZ, 3f, Color.Green);
Sphere testSphere2 = new(-Vector3.UnitZ, 2f, Color.Red);
scene.SceneObjects.Add(testSphere);
scene.SceneObjects.Add(testSphere2);
Console.WriteLine(scene.TraceRay(new(){ Direction = new(0,0,1), StartPoint = cam.Position}));

var raysC1 = cam.GenRays();
var cols = new Color[cam.ResY, cam.ResX];
var img = new Image<SixLabors.ImageSharp.PixelFormats.Bgr24>(cam.ResX, cam.ResY);
for (int y = 0; y < cam.ResX; y++)
{
    for (int x = 0; x < cam.ResY; x++)
    {
        cols[y, x] = scene.TraceRay(raysC1[y * cam.ResX + x]);
        //Console.Write(raysC1[y * cam.ResX + x].Direction);
        Console.Write(cols[y,x].ToString() == "0" ? "EMPTY" : cols[y,x].ToString());
        
        img[x, y] = cols[y, x];
        Console.Write(" ");
    }
    Console.WriteLine();
}
img.SaveAsPng("./out.png");