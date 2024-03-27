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

    // fov'u genistletmek icin
    float adjustedAngle = angle * 0.2f;

    float k = 1 / MathF.Tan(adjustedAngle / 2);
    Vector4 a = direction.Multiply(k).Multiply(-1);
    Vector4 b = horizontal.Multiply(x - 0.5f).Multiply((float)2);
    Vector4 c = up.Multiply(y - 0.5f).Multiply((float)2);

    // discriminant hep negatif cikiyor diye -1 ile carptim ama neden bilmiyorum?
    Vector4 rayDirection = a.Add(b).Subtract(c);

    return new Ray(center, rayDirection);
  }
}
