namespace ray_tracer;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

class Program
{
  static void Main(string[] args)
  {
    if (args.Length != 3)
    {
      Console.WriteLine("Usage: dotnet run <path_to_json_file> <output_file_name> Program.cs");
      return;
    }

    SceneObjects scene = SceneParser.ParseScene(args[0]);

    float tMin = 0.0001f;
    float near = 8.0f;
    float far = 11.0f;

    int width = 1000;
    int height = 1000;

    using (Image<Rgba32> image = new Image<Rgba32>(width, height))
    {
      for (int x = 0; x < width; x++)
      {
        for (int y = 0; y < height; y++)
        {
          float normalizedX = (x / (width - 1.0f)) * 2.0f - 0.5f;
          float normalizedY = (y / (height - 1.0f)) * 2.0f - 0.5f;
          Ray ray = scene.camera.GenerateRay(normalizedX, normalizedY);
          Hit hit = new Hit(float.PositiveInfinity, null);

          byte depthValue;
          Vector4 colors;
          if (scene.group.Intersect(ray, hit, tMin))
          {
            // DEPTH BAK
            // depthValue = (byte)(255 - (255 * (hit.t - near) / (far - near)));
            depthValue = (byte)255;

            colors = new Vector4(0, 0, 0, 0);
            colors = colors.Add(RayTracer.Trace(scene, ray, tMin, 2, 1.0f, hit.material.indexOfRefraction, hit));
          }
          else
          {
            depthValue = (byte)(255 - (255 * (hit.t - near) / (far - near)));
            colors = scene.background.color;
          }

          Rgba32 color = new Rgba32(colors.X, colors.Y, colors.Z, depthValue);
          // Rgba32 color = new Rgba32(depthValue, depthValue, depthValue, depthValue);
          image[x, y] = color;
        }
      }
      image.Save(args[1]);
    }
    Console.WriteLine("Image saved successfully.");
  }
}
