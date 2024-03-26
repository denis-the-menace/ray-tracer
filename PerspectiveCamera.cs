public class PerspectiveCamera : Camera
{
  private Vector4 center;
  private Vector4 direction;
  private Vector4 up;
  private float angle;

  public PerspectiveCamera(Vector4 center, Vector4 direction, Vector4 up, float angle)
  {
    this.center = center;
    this.direction = direction.Normalize();
    this.up = up.Normalize();
    this.angle = angle;
  }

  public override Ray GenerateRay(float x, float y)
  {
    Vector4 horizontal = direction.Cross(up);

    float k = 1 / MathF.Tan(angle / 2);
    Vector4 a = direction.Multiply(k);
    Vector4 b = horizontal.Multiply(x - 0.5f).Multiply((float)2);
    Vector4 c = up.Multiply(y - 0.5f).Multiply((float)2);

    Vector4 rayDirection = b.Add(c).Add(a);
    // Vector4 rayOrigin = center.Add(horizontal.Multiply(size * (x - 0.5f))).Add(up.Multiply(size * (y - 0.5f)));

    // Calculate the camera's right vector
    // Vector4 right = Vector4.Cross(direction, up).Normalized();

    // Calculate the half-width and half-height of the view plane
    // float halfHeight = MathF.Tan(angle / 2);
    // float halfWidth = aspectRatio * halfHeight;

    // Calculate the direction of the ray
    // Vector4 rayDirection = (direction + x * halfWidth * right + y * halfHeight * up).Normalized();

    return new Ray(center, rayDirection);
  }
}
