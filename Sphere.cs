public class Sphere : Object3D
{
  public float radius { get; private set; }
  public Vector4 center { get; private set; }

  public Sphere(Vector4 center, float radius, float[] color)
      : base(color[0], color[1], color[2])
  {
    this.center = new Vector4(center);
    this.radius = radius;
  }

  public override bool Intersect(Ray ray, Hit hit, float tMin)
  {
    Vector4 L = ray.origin.Subtract(center);

    float a = ray.direction.Dot(ray.direction);
    float b = 2.0f * L.Dot(ray.direction);
    // float b = 2.0f * ray.direction.Dot(L);
    float c = L.Dot(L) - (radius * radius);

    float discriminant = b * b - 4.0f * a * c;

    if (discriminant > 0)
    {
      float t1 = (-b - (float)Math.Sqrt(discriminant)) / (2.0f * a);
      float t2 = (-b + (float)Math.Sqrt(discriminant)) / (2.0f * a);

      if (t1 > t2)
      {
        float temp = t1;
        t1 = t2;
        t2 = temp;
      }

      if (t1 < 0)
      {
        t1 = t2; // if t0 is negative, let's use t1 instead
        if (t1 < 0) return false; // both t0 and t1 are negative
      }

      Vector4 hitPoint = ray.origin.Add(ray.direction.Multiply(t1));

      // Calculate the normal at the hit point
      Vector4 hitNormal = hitPoint.Subtract(center).Normalize();


      hit.t = t1;
      hit.color = color;
      hit.normal = hitNormal;
      // Console.WriteLine(hit.normal.ToString());

      return true;
    }
    return false;
  }

  public override string ToString()
  {
    return $"Sphere: Center({center}), Radius({radius}), Color({color[0]}, {color[1]}, {color[2]})";
  }
}
