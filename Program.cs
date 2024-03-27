namespace assignment1;

using System.Text.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

class Program
{
  static void Main(string[] args)
  {
    string inputFile = "a3_scenes/scene3_perspective.json";
    string jsonContent = File.ReadAllText(inputFile);
    JsonDocument jsonDocument = JsonDocument.Parse(jsonContent);
    JsonElement root = jsonDocument.RootElement;
    // JsonElement orthocameraJSON = root.GetProperty("orthocamera");
    JsonElement perspectivecameraJSON = root.GetProperty("perspectivecamera");
    JsonElement backgroundJSON = root.GetProperty("background");
    JsonElement groupJSON = root.GetProperty("group");
    JsonElement lightJSON = root.GetProperty("light");

    // OrthographicCamera camera = new OrthographicCamera(new Vector4(ConvertJsonArrayToFloatArray(orthocameraJSON.GetProperty("center"))), new Vector4(ConvertJsonArrayToFloatArray(orthocameraJSON.GetProperty("direction"))), new Vector4(ConvertJsonArrayToFloatArray(orthocameraJSON.GetProperty("up"))), orthocameraJSON.GetProperty("size").GetSingle());

    PerspectiveCamera camera = new PerspectiveCamera(new Vector4(ConvertJsonArrayToFloatArray(perspectivecameraJSON.GetProperty("center"))), new Vector4(ConvertJsonArrayToFloatArray(perspectivecameraJSON.GetProperty("direction"))), new Vector4(ConvertJsonArrayToFloatArray(perspectivecameraJSON.GetProperty("up"))), perspectivecameraJSON.GetProperty("angle").GetSingle());

    // float[] background = ConvertJsonArrayToFloatArray(backgroundJSON.GetProperty("color"));
    Background background = new Background(ConvertJsonArrayToFloatArray(backgroundJSON.GetProperty("color")), ConvertJsonArrayToFloatArray(backgroundJSON.GetProperty("ambient")));

    Vector4 lightDirection = new Vector4(ConvertJsonArrayToFloatArray(lightJSON.GetProperty("direction")));
    float[] lightColor = ConvertJsonArrayToFloatArray(lightJSON.GetProperty("color"));

    Group group = new Group();
    foreach (JsonElement sphereElement in groupJSON.EnumerateArray())
    {
      JsonElement sphereProperties = sphereElement.GetProperty("sphere");
      Vector4 center = new Vector4(ConvertJsonArrayToFloatArray(sphereProperties.GetProperty("center")));
      float radius = sphereProperties.GetProperty("radius").GetSingle();
      float[] color = ConvertJsonArrayToFloatArray(sphereProperties.GetProperty("color"));

      Sphere sphere = new Sphere(center, radius, color);
      group.AddObject(sphere);
    }

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
          Ray ray = camera.GenerateRay(normalizedX, normalizedY);
          Hit hit = new Hit(float.PositiveInfinity, background.color);
          bool hitOccured = group.Intersect(ray, hit, tMin);

          //eger hitOccured true ise depth value hesapla cunku shading etkisi, false ise hesaplama cunku bg gozuksun
          byte depthValue;
          float[] colors;
          if (hitOccured)
          {
            float depth = (far - hit.t) / (far - near);
            depth *= 255.0f; // Convert depth to [0, 255] interval
            depthValue = (byte)depth;
            colors = ComputeLighting(background.ambient, hit.color, lightColor, lightDirection, hit.normal);
            // Console.WriteLine(colors[0] + " " + colors[1] + " " + colors[2]);
          }
          else
          {
            depthValue = (byte)255;
            colors = ComputeLighting(background.ambient, hit.color, lightColor, lightDirection, hit.normal);
          }


          Rgba32 color = new Rgba32(colors[0], colors[1], colors[2], depthValue);

          // Console.WriteLine(color.ToString());
          image[x, y] = color;
        }
      }
      image.Save("output3.png");
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

  static float[] ConvertJsonArrayToFloatArray(JsonElement jsonElement)
  {
    float[] floatArray = new float[jsonElement.GetArrayLength()];
    int i = 0;
    foreach (JsonElement element in jsonElement.EnumerateArray())
    {
      floatArray[i++] = element.GetSingle();
    }
    return floatArray;
  }
}

