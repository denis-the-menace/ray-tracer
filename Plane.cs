public class Plane : Object3D
{
  private Vector4 normal;
  private float d; // offset from origin

  public Plane(Vector4 normal, float d, float[] color) : base(color[0], color[1], color[2])
  {
    this.normal = normal.Normalize();
    this.d = d;
  }

  public override bool Intersect(Ray ray, Hit hit, float tMin)
  {
    float denominator = ray.direction.Dot(normal);

    // Check if the ray is parallel or almost parallel to the plane (for performance)
    if (MathF.Abs(denominator) < 1e-6)
      return false;

    Vector4 p0 = new Vector4(normal.Multiply(d));
    float t = (p0.Subtract(ray.origin)).Dot(normal) / denominator;

    // Check if intersection is within allowed range and closer than previous hits
    if (t < 0)
      return false;

    // Update hit information
    hit.t = t;
    hit.color = color;
    hit.normal = normal;

    return true;
  }

  public override string ToString()
  {
    return $"Plane: Normal({normal}), Offset({d}), Color({color[0]}, {color[1]}, {color[2]})";
  }

}
