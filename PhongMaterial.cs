public class PhongMaterial : Material
{
  private Vector4 specularColor;
  private float exponent;

  public PhongMaterial(Vector4 diffuseColor, Vector4 reflectiveColor, Vector4 transparentColor, float indexOfRefraction,
                       Vector4 specularColor, float exponent)
      : base(diffuseColor, reflectiveColor, transparentColor, indexOfRefraction)
  {
    this.specularColor = specularColor;
    this.exponent = exponent;
  }

  public PhongMaterial(Vector4 diffuseColor, Vector4 specularColor, float exponent)
      : base(diffuseColor)
  {
    this.specularColor = specularColor;
    this.exponent = exponent;
  }

  public PhongMaterial(Vector4 diffuseColor)
      : base(diffuseColor)
  {
    this.specularColor = new Vector4(0, 0, 0, 0);
    this.exponent = 0;
  }

  //https://www.youtube.com/watch?v=LKXAIuCaKAQ
  public override Vector4 Shade(Ray ray, Hit hit, Light light)
  {
    if (!(light is DirectionalLight directionalLight) || hit.material == null)
    {
      return new Vector4(0, 0, 0, 0);
    }

    // Calculate dot product and clamp it to 0 if negative
    Vector4 diffuseResult = diffuseColor.Multiply(directionalLight.color).Multiply(Math.Max(directionalLight.direction.Dot(hit.normal.Multiply(-1)), 0));

    // Calculate reflection vector R = 2 * (L . N) * N - L THEN NORMALIZE
    Vector4 reflection = hit.normal.Multiply(2 * hit.normal.Dot(directionalLight.direction)).Subtract(directionalLight.direction).Normalize();

    // Calculate specular factor S = max(R . V, 0) ^ exponent
    float specularFactor = (float)Math.Pow(Math.Max(reflection.Dot(ray.direction.Normalize()), 0.0f), exponent);
    Vector4 specularResult = specularColor.Multiply(directionalLight.color).Multiply(specularFactor);

    Vector4 result = diffuseResult.Add(specularResult);

    return result;
  }
}
