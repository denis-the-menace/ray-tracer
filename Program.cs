namespace assignment1;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

class Program
{
  static void Main(string[] args)
  {
    if (args.Length != 3)
    {
      Console.WriteLine("Usage: dotnet run <path_to_json_file> <output_file_name>");
      return;
    }

    SceneObjects scene = SceneParser.ParseScene(args[0]);

    float tMin = float.PositiveInfinity;
    float near = 8.0f;
    float far = 11.5f;

    int width = 1000;
    int height = 1000;

    //using keywordu yarattigimiz obje ile is bitince memory free ediyor
    using (Image<Rgba32> image = new Image<Rgba32>(width, height))
    {
      for (int x = 0; x < width; x++)
      {
        for (int y = 0; y < height; y++)
        {
          float normalizedX = (x / (width - 1.0f)) * 2.0f - 0.5f;
          float normalizedY = (y / (height - 1.0f)) * 2.0f - 0.5f;
          Ray ray = scene.camera.GenerateRay(normalizedX, normalizedY);
          Hit hit = new Hit(float.PositiveInfinity, scene.background.color);
          bool hitOccured = scene.group.Intersect(ray, hit, tMin);

          //eger hitOccured true ise depth value hesapla cunku shading etkisi, false ise hesaplama cunku bg gozuksun
          byte depthValue;
          float[] colors; if (hitOccured)
          {
            float depth = (far - hit.t) / (far - near);
            depth *= 255.0f; // Convert depth to [0, 255] interval
            depthValue = (byte)depth;
            colors = ComputeLighting(scene.background.ambient, hit.color, scene.light.color, scene.light.direction, hit.normal);
          }
          else
          {
            depthValue = (byte)255;
            colors = ComputeLighting(scene.background.ambient, hit.color, scene.light.color, scene.light.direction, hit.normal);
          }

          Rgba32 color = new Rgba32(colors[0], colors[1], colors[2], depthValue);
          image[x, y] = color;
        }
      }
      image.Save(args[1]);
    }
    Console.WriteLine("Image saved successfully.");
  }

  static float[] ComputeLighting(float[] ambientColor, float[] objectColor, float[] lightColor, Vector4 lightDirection, Vector4 normal)
  {
    // Calculate ambient part
    float[] ambientResult = new float[3];
    for (int i = 0; i < 3; i++)
    {
      ambientResult[i] = ambientColor[i] * objectColor[i];
    }

    // Calculate diffuse part
    float diffuseFactor = Math.Max(lightDirection.Dot(normal.Multiply(-1)), 0); // Calculate dot product and clamp it to 0 if negative
    float[] diffuseResult = new float[3];
    for (int i = 0; i < 3; i++)
    {
      diffuseResult[i] = objectColor[i] * lightColor[i] * diffuseFactor;
    }

    // Combine ambient and diffuse results
    float[] result = new float[3];
    for (int i = 0; i < 3; i++)
    {
      result[i] = ambientResult[i] + diffuseResult[i];
    }

    return result;
  }
}
