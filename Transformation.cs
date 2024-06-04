public class Transformation : Object3D
{
  public Matrix4 m;
  public Object3D obj;

  public Transformation(Matrix4 m, Object3D obj) : base(null)
  {
    this.m = m;
    this.obj = obj;
  }

  public override bool Intersect(Ray ray, Hit hit, float tMin)
  {
    Matrix4 inverse = m.Inverse();
    Vector4 transformedOrigin = inverse.Multiply(ray.origin);
    Vector4 transformedDirection = inverse.Multiply(ray.direction);
    // RAY NORMALIZE EDILMEYECEK
    Ray transformedRay = new Ray(transformedOrigin, transformedDirection);
    ray = transformedRay;

    // return obj.Intersect(transformedRay, hit, tMin);
    if (obj.Intersect(ray, hit, tMin))
    {
      Vector4 hitPoint = ray.origin.Add(ray.direction.Multiply(hit.t));
      hit.t = ray.origin.Subtract(hitPoint).Magnitude();
      hit.normal = inverse.Multiply(hit.normal);
      return true;
    }

    return false;
  }

  public override string ToString()
  {
    return $"Transformation:\n{m.ToString()}{obj.ToString()}";
  }
}
