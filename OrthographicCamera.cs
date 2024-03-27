public class OrthographicCamera : Camera
{
  public Vector4 center;
  public Vector4 direction;
  public Vector4 up;
  public float size;

  public OrthographicCamera(Vector4 center, Vector4 direction, Vector4 up, float size)
  {
    this.center = center;
    this.direction = direction.Normalize();
    this.up = up.Normalize();
    this.size = size;
  }

  public override Ray GenerateRay(float x, float y)
  {
    Vector4 horizontal = direction.Cross(up);

    // Vector4 rayOrigin = center.Add(horizontal.Multiply(size * (x - 0.5f))).Add(up.Multiply(size * (y - 0.5f)));
    Vector4 rayOrigin = center.Add(horizontal.Multiply(size * (x - 0.5f))).Subtract(up.Multiply(size * (y - 0.5f)));

    return new Ray(rayOrigin, direction);
  }

  public override string ToString()
  {
    return $"Orthographic Camera: Center({center}), Direction({direction}), Up({up}), Size({size})";
  }
}
