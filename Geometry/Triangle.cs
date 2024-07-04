public class Triangle : Object3D
{
  private Vector4 v1;
  private Vector4 v2;
  private Vector4 v3;

  public Triangle(Vector4 v1, Vector4 v2, Vector4 v3, Material material) : base(material)
  {
    this.v1 = v1;
    this.v2 = v2;
    this.v3 = v3;
  }

  public override bool Intersect(Ray ray, Hit hit, float tMin)
  {
    // Compute the plane's normal
    Vector4 v1v2 = v2.Subtract(v1);
    Vector4 v1v3 = v3.Subtract(v1);

    // No need to normalize
    Vector4 N = v1v2.Cross(v1v3);
    float area2 = N.Magnitude(); // area of the triangle

    // Step 1: finding P

    // check if the ray and plane are parallel.
    float NdotRayDirection = N.Dot(ray.direction);
    if (MathF.Abs(NdotRayDirection) < float.Epsilon) // almost 0
      return false; // they are parallel so they don't intersect

    // Compute d parameter using equation 2
    float d = -N.Dot(v1);

    // Compute t (equation 3)
    float t = -(N.Dot(ray.origin) + d) / NdotRayDirection;
    // check if the triangle is behind the ray
    if (t < 0) return false; // the triangle is behind

    // Compute the intersection point using equation 1
    Vector4 P = ray.origin.Add(ray.direction.Multiply(t));

    // Step 2: inside-outside test
    Vector4 C; // vector perpendicular to triangle's plane

    // Edge 1
    Vector4 edge1 = v2.Subtract(v1);
    Vector4 vp1 = P.Subtract(v1);
    C = edge1.Cross(vp1);
    if (N.Dot(C) < 0) return false; // P is on the right side

    // Edge 2
    Vector4 edge2 = v3.Subtract(v2);
    Vector4 vp2 = P.Subtract(v2);
    C = edge2.Cross(vp2);
    float u = C.Magnitude() / area2;
    if (N.Dot(C) < 0) return false; // P is on the right side

    // Edge 3
    Vector4 edge3 = v1.Subtract(v3);
    Vector4 vp3 = P.Subtract(v3);
    C = edge3.Cross(vp3);
    float v = C.Magnitude() / area2;
    if (N.Dot(C) < 0) return false; // P is on the right side;

    hit.t = t;
    hit.material= this.material;
    hit.normal = N.Normalize();

    return true;
  }

  public override string ToString()
  {
    return $"Triangle: V1({v1}), V2({v2}), V3({v3}), Material({material})";
  }

}
