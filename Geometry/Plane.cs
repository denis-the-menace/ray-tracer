public class Plane : Object3D
{
  private Vector4 normal;
  private float d; // offset from origin

  public Plane(Vector4 normal, float d, Material material) : base(material)
  {
    this.normal = normal.Normalize();
    this.d = d;
  }

  public override bool Intersect(Ray ray, Hit hit, float tMin)
  {
    float denominator = ray.direction.Dot(normal);

    // Check if the ray is parallel or almost parallel to the plane (for performance)
    if (MathF.Abs(denominator) < float.Epsilon)
      return false;

    Vector4 p0 = new Vector4(normal.Multiply(d));
    float t = (p0.Subtract(ray.origin)).Dot(normal) / denominator;

    // Check if intersection is within allowed range and closer than previous hits
    if (t < tMin || t >= hit.t)
      return false;

    // Update hit information
    hit.t = t;
    hit.material = this.material;
    hit.normal = normal;

    return true;
  }

  public override string ToString()
  {
    return $"Plane: Normal({normal}), Offset({d}), Material({material})";
  }

}
