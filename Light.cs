public class Light
{
  public Vector4 direction { get; private set; }
  public float[] color { get; private set; }

  public Light(Vector4 direction, float[] color)
  {
    this.direction = direction;
    this.color = color;
  }
}
