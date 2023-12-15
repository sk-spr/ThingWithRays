using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ThingWithRays.Types;
using System;
using Gdk;
using SixLabors.ImageSharp;
using Color = SixLabors.ImageSharp.Color;
using Gtk;
using SixLabors.ImageSharp.PixelFormats;
using Window = Gtk.Window;

int WIDTH = 900;
int HEIGHT = 600;

Scene scene = new();
Camera cam = new();
cam.FovX = 90;
cam.FovY = 60;
cam.Position = new(0, 5, -10);
cam.Rotation = Quaternion.CreateFromYawPitchRoll(0f, 0.5f, 0f);
cam.ResX = WIDTH;
cam.ResY = HEIGHT;
scene.Cameras.Add(cam);
Sphere testSphere = new(new(1,2,1), 3f, Color.Green);
testSphere.Material = new()
{
    Color = Color.Green,
    DoesReflect = false,
    IOR = 0f
};
Sphere testSphere2 = new(new(-2, 0, -4), 0.7f, Color.Red);
testSphere2.Material = new()
{
    Color = Color.Red,
    DoesReflect = false,
    IOR = 0f
};
EndlessPlane testPlane = new(new(0,-2.5f,0), new(0,1,0), Color.Beige);
testPlane.Material = new Material()
{
    Color = Color.Beige,
    DoesReflect = false,
    IOR = 0f
};
EndlessPlane testPlane2 = new(new(0,1.5f,0f), new(0,-1,0), Color.Beige);
testPlane2.Material = new Material()
{
    Color = Color.Beige,
    DoesReflect = false,
    IOR = 0f
};
SceneLight light1 = new();
light1.Position = new(0, 10, 0);
light1.Power = 50f;
light1.Dropoff = 1f;
light1.LightColor = Color.White;
scene.Lights.Add(light1);

SceneLight light2 = new();
light2.Position = new(7, 5, 0);
light2.Power = 50f;
light2.Dropoff = 1f;
light2.LightColor = Color.White;
scene.Lights.Add(light2);

scene.SceneObjects.Add(testSphere);
scene.SceneObjects.Add(testSphere2);
scene.SceneObjects.Add(testPlane);
//scene.SceneObjects.Add(testPlane2);
Random random = new();
for (int i = 0; i < 5; i++)
{
    float size = random.NextSingle() * 3f;
    Vector3 position = new(random.NextSingle() * 10-5, random.NextSingle() * 2, random.NextSingle() * 10-5);
    Sphere next = new(position, size,
        new Color(new Bgr24((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255))));
    next.Material = new()
    {
        Color = Color.Magenta,
        DoesReflect = false,
        IOR = 0f
    };
    //scene.SceneObjects.Add(next);
}
var raysC1 = cam.GenRays();
var cols = new Color[cam.ResY, cam.ResX];
var img = new Image<SixLabors.ImageSharp.PixelFormats.Bgr24>(cam.ResX, cam.ResY);
int timestep = 0;

void RunSingleRow(int y)
{
    for (int x = 0; x < WIDTH; x++)
    {
        cols[y, x] = scene.TraceRay(raysC1[y * cam.ResX + x], 2);
        //Console.Write(raysC1[y * cam.ResX + x].Direction);
        //Console.Write(cols[y,x].ToString() == "0" ? "EMPTY" : cols[y,x].ToString());
        img[x, y] = cols[y, x];
        //Console.Write(" ");
    }
}

void RenderImage()
{
    var tick = DateTimeOffset.Now;
    Parallel.For(0, HEIGHT, RunSingleRow);
    img.SaveAsPng("./out.png");
    Console.WriteLine($"Took {DateTimeOffset.Now - tick}");

}
RenderImage();

