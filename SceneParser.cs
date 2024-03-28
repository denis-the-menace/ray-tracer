using System.Text.Json;

public struct SceneObjects
{
  public Camera camera { get; set; }
  public Background background { get; set; }
  public Group group { get; set; }
  public Light light { get; set; }
}

public class SceneParser
{

  public static SceneObjects ParseScene(string inputFile)
  {
    string jsonContent = File.ReadAllText(inputFile);
    using JsonDocument jsonDocument = JsonDocument.Parse(jsonContent);
    JsonElement root = jsonDocument.RootElement;
    JsonElement backgroundJSON = root.GetProperty("background");
    JsonElement groupJSON = root.GetProperty("group");
    JsonElement lightJSON = root.GetProperty("light");
    bool hasOrthoCamera = root.TryGetProperty("orthocamera", out JsonElement orthocameraJSON);

    Camera camera;
    if (hasOrthoCamera)
    {
      camera = new OrthographicCamera(
          new Vector4(ConvertJsonArrayToFloatArray(orthocameraJSON.GetProperty("center"))),
          new Vector4(ConvertJsonArrayToFloatArray(orthocameraJSON.GetProperty("direction"))),
          new Vector4(ConvertJsonArrayToFloatArray(orthocameraJSON.GetProperty("up"))),
          orthocameraJSON.GetProperty("size").GetSingle()
      );
    }
    else
    {
      JsonElement perspectivecameraJSON = root.GetProperty("perspectivecamera");
      camera = new PerspectiveCamera(
          new Vector4(ConvertJsonArrayToFloatArray(perspectivecameraJSON.GetProperty("center"))),
          new Vector4(ConvertJsonArrayToFloatArray(perspectivecameraJSON.GetProperty("direction"))),
          new Vector4(ConvertJsonArrayToFloatArray(perspectivecameraJSON.GetProperty("up"))),
          perspectivecameraJSON.GetProperty("angle").GetSingle()
      );
    }

    Background background = new Background(
        ConvertJsonArrayToFloatArray(backgroundJSON.GetProperty("color")),
        ConvertJsonArrayToFloatArray(backgroundJSON.GetProperty("ambient"))
    );

    Light light = new Light(
        new Vector4(ConvertJsonArrayToFloatArray(lightJSON.GetProperty("direction"))),
        ConvertJsonArrayToFloatArray(lightJSON.GetProperty("color"))
    );
    Vector4 lightDirection = new Vector4(ConvertJsonArrayToFloatArray(lightJSON.GetProperty("direction")));
    float[] lightColor = ConvertJsonArrayToFloatArray(lightJSON.GetProperty("color"));

    Group group = new Group();
    foreach (JsonElement element in groupJSON.EnumerateArray())
    {
      // Check if the element has a "transform" property
      if (element.TryGetProperty("transform", out JsonElement transformElement))
      {
        // If it does, retrieve the transformation JSON object
        JsonElement transformationJson = transformElement.GetProperty("transformations");
        // Initialize a new transformation matrix
        Matrix4 transformationMatrix = new Matrix4();

        // Iterate through each transformation in the array
        foreach (JsonElement transformationElement in transformationJson.EnumerateArray())
        {
          // Apply each transformation to the transformation matrix
          if (transformationElement.TryGetProperty("scale", out var scaleJson))
          {
            float[] scale = ConvertJsonArrayToFloatArray(scaleJson);
            transformationMatrix = transformationMatrix.Multiply(Matrix4.Scale(scale[0], scale[1], scale[2]));
          }
          else if (transformationElement.TryGetProperty("zrotate", out var zrotateJson))
          {
            float rotation = zrotateJson.GetSingle();
            transformationMatrix = transformationMatrix.Multiply(Matrix4.RotateZ(rotation));
          }
        }

        // Retrieve the object JSON within the transformation object
        JsonElement objectJson = transformElement.GetProperty("object");
        JsonElement sphereElement = objectJson.GetProperty("sphere");
        Vector4 center = new Vector4(ConvertJsonArrayToFloatArray(sphereElement.GetProperty("center")));
        float radius = sphereElement.GetProperty("radius").GetSingle();
        float[] color = ConvertJsonArrayToFloatArray(sphereElement.GetProperty("color"));

        Sphere sphere = new Sphere(center, radius, color);
        Transformation transformation = new Transformation(transformationMatrix, sphere);
        group.AddObject(transformation);
        Console.WriteLine(transformation.ToString());
      }
      else if (element.TryGetProperty("sphere", out JsonElement sphereElement))
      {
        Vector4 center = new Vector4(ConvertJsonArrayToFloatArray(sphereElement.GetProperty("center")));
        float radius = sphereElement.GetProperty("radius").GetSingle();
        float[] color = ConvertJsonArrayToFloatArray(sphereElement.GetProperty("color"));

        Sphere sphere = new Sphere(center, radius, color);
        group.AddObject(sphere);
        Console.WriteLine(sphere.ToString());
      }
      else if (element.TryGetProperty("plane", out JsonElement planeElement))
      {
        Vector4 normal = new Vector4(ConvertJsonArrayToFloatArray(planeElement.GetProperty("normal")));
        float offset = planeElement.GetProperty("offset").GetSingle();
        float[] color = ConvertJsonArrayToFloatArray(planeElement.GetProperty("color"));

        Plane plane = new Plane(normal, offset, color);
        group.AddObject(plane);
        Console.WriteLine(plane.ToString());
      }
      else if (element.TryGetProperty("triangle", out JsonElement triangleElement))
      {
        Vector4 v1 = new Vector4(ConvertJsonArrayToFloatArray(triangleElement.GetProperty("v1")));
        Vector4 v2 = new Vector4(ConvertJsonArrayToFloatArray(triangleElement.GetProperty("v2")));
        Vector4 v3 = new Vector4(ConvertJsonArrayToFloatArray(triangleElement.GetProperty("v3")));
        float[] color = ConvertJsonArrayToFloatArray(triangleElement.GetProperty("color"));

        Triangle triangle = new Triangle(v1, v2, v3, color);
        group.AddObject(triangle);
        Console.WriteLine(triangle.ToString());
      }
      else
      {
        throw new Exception("Unknown object type or invalid format in group.");
      }
    }

    jsonDocument.Dispose();

    return new SceneObjects
    {
      camera = camera,
      background = background,
      group = group,
      light = light,
    };
  }

  private static float[] ConvertJsonArrayToFloatArray(JsonElement jsonElement)
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
