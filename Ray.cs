public class Ray
{
  public Vector4 origin { get; set; }
  public Vector4 direction { get; set; }

  public Ray(Vector4 origin, Vector4 direction)
  {
    this.origin = new Vector4(origin);
    this.direction = new Vector4(direction).Normalize();
  }
}
