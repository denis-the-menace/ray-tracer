using System.Text.Json;

public struct SceneObjects
{
  public Camera camera { get; set; }
  public Background background { get; set; }
  public Group group { get; set; }
  public Light[] lights { get; set; }
  public Material[] materials { get; set; }
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
    JsonElement lightsJSON = root.GetProperty("lights");
    JsonElement materialsJSON = root.GetProperty("materials");
    // bool hasOrthoCamera = root.TryGetProperty("orthocamera", out JsonElement orthocameraJSON);

    Camera camera;
    if (root.TryGetProperty("orthocamera", out JsonElement orthocameraJSON))
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
        new Vector4(ConvertJsonArrayToFloatArray(backgroundJSON.GetProperty("color"))),
    new Vector4(ConvertJsonArrayToFloatArray(backgroundJSON.GetProperty("ambient")))
    );

    Light[] lights = lightsJSON.EnumerateArray()
    .Select(lightElement =>
    {
      JsonElement directionalLightJSON = lightElement.GetProperty("directionalLight");
      Vector4 direction = new Vector4(ConvertJsonArrayToFloatArray(directionalLightJSON.GetProperty("direction")));
      Vector4 color = new Vector4(ConvertJsonArrayToFloatArray(directionalLightJSON.GetProperty("color")));
      return new DirectionalLight(color, direction);
    })
    .ToArray();

    Material[] materials = materialsJSON.EnumerateArray()
    .Select(materialElement =>
    {
      JsonElement phongMaterialJSON = materialElement.GetProperty("phongMaterial");
      Vector4 diffuseColor = new Vector4(ConvertJsonArrayToFloatArray(phongMaterialJSON.GetProperty("diffuseColor")));

      // Check if specularColor and exponent properties exist
      if (phongMaterialJSON.TryGetProperty("specularColor", out JsonElement specularColorElement) &&
          phongMaterialJSON.TryGetProperty("exponent", out JsonElement exponentElement))
      {
        Vector4 specularColor = new Vector4(ConvertJsonArrayToFloatArray(specularColorElement));
        float exponent = exponentElement.GetSingle();

        if (phongMaterialJSON.TryGetProperty("reflectiveColor", out JsonElement reflectiveColorElement) &&
            phongMaterialJSON.TryGetProperty("transparentColor", out JsonElement transparentColorElement) &&
            phongMaterialJSON.TryGetProperty("indexOfRefraction", out JsonElement indexOfRefractionElement))
        {
          Vector4 reflectiveColor = new Vector4(ConvertJsonArrayToFloatArray(reflectiveColorElement));
          Vector4 transparentColor = new Vector4(ConvertJsonArrayToFloatArray(transparentColorElement));
          float indexOfRefraction = indexOfRefractionElement.GetSingle();
          return new PhongMaterial(diffuseColor, reflectiveColor, transparentColor, indexOfRefraction, specularColor, exponent);
        }

        return new PhongMaterial(diffuseColor, specularColor, exponent);
      }
      else
      {
        return new PhongMaterial(diffuseColor);
      }
    })
    .ToArray();

    Group group = new Group();
    groupJSON.EnumerateArray().ToList().ForEach(element =>
    {
      if (element.TryGetProperty("transform", out JsonElement transformElement))
      {
        JsonElement transformationJson = transformElement.GetProperty("transformations");
        Matrix4 transformationMatrix = new Matrix4();

        foreach (JsonElement transformationElement in transformationJson.EnumerateArray())
        {
          if (transformationElement.TryGetProperty("translate", out var translateJson))
          {
            float[] translate = ConvertJsonArrayToFloatArray(translateJson);
            transformationMatrix = transformationMatrix.Multiply(Matrix4.Translate(translate[0], translate[1], translate[2]));
          }
          else if (transformationElement.TryGetProperty("scale", out var scaleJson))
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

        if (transformElement.TryGetProperty("object", out JsonElement objectJson))
        {
          if (objectJson.TryGetProperty("sphere", out JsonElement sphereElement))
          {
            Vector4 center = new Vector4(ConvertJsonArrayToFloatArray(sphereElement.GetProperty("center")));
            float radius = sphereElement.GetProperty("radius").GetSingle();
            int materialIndex = sphereElement.GetProperty("material").GetInt32();

            Sphere sphere = new Sphere(center, radius, materials[materialIndex]);
            Transformation transformation = new Transformation(transformationMatrix, sphere);
            group.AddObject(transformation);
            Console.WriteLine(transformation);
          }
        }
      }
      else if (element.TryGetProperty("sphere", out JsonElement sphereElement))
      {
        Vector4 center = new Vector4(ConvertJsonArrayToFloatArray(sphereElement.GetProperty("center")));
        float radius = sphereElement.GetProperty("radius").GetSingle();
        int materialIndex = sphereElement.GetProperty("material").GetInt32();

        Sphere sphere = new Sphere(center, radius, materials[materialIndex]);
        group.AddObject(sphere);
      }
      else if (element.TryGetProperty("plane", out JsonElement planeElement))
      {
        Vector4 normal = new Vector4(ConvertJsonArrayToFloatArray(planeElement.GetProperty("normal")));
        float offset = planeElement.GetProperty("offset").GetSingle();
        int materialIndex = planeElement.GetProperty("material").GetInt32();

        Plane plane = new Plane(normal, offset, materials[materialIndex]);
        group.AddObject(plane);
      }
      else if (element.TryGetProperty("triangle", out JsonElement triangleElement))
      {
        Vector4 v1 = new Vector4(ConvertJsonArrayToFloatArray(triangleElement.GetProperty("v1")));
        Vector4 v2 = new Vector4(ConvertJsonArrayToFloatArray(triangleElement.GetProperty("v2")));
        Vector4 v3 = new Vector4(ConvertJsonArrayToFloatArray(triangleElement.GetProperty("v3")));
        int materialIndex = triangleElement.GetProperty("material").GetInt32();

        Triangle triangle = new Triangle(v1, v2, v3, materials[materialIndex]);
        group.AddObject(triangle);
      }
      else
      {
        throw new Exception("Unknown object type or invalid format in group.");
      }
    });

    jsonDocument.Dispose();

    return new SceneObjects
    {
      camera = camera,
      background = background,
      group = group,
      lights = lights,
      materials = materials
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
