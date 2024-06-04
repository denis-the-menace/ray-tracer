public abstract class Light
{
  public Vector4 color { get; private set; }

  public Light(Vector4 color)
  {
    this.color = color;
  }
}
