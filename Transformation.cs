public class Transformation : Object3D
{
  public Matrix4 m;
  public Object3D obj;

  public Transformation(Matrix4 m, Object3D obj) : base(0, 0, 0)
  {
    this.m = m;
    this.obj = obj;
  }

  public override bool Intersect(Ray ray, Hit hit, float tMin)
  {
    Matrix4 inverse = m.Inverse();
    Vector4 transformedOrigin = inverse.Multiply(ray.origin);
    Vector4 transformedDirection = inverse.Multiply(ray.direction);
    Ray transformedRay = new Ray(transformedOrigin, transformedDirection);

    return obj.Intersect(transformedRay, hit, tMin);
  }

  public override string ToString()
  {
    return $"Transformation:\n{m.ToString()}{obj.ToString()}";
  }
}
