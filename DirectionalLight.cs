public class DirectionalLight : Light
{
  public Vector4 direction { get; private set; }

  public DirectionalLight(Vector4 color, Vector4 direction) : base(color)
  {
    this.direction = direction;
  }

  public override string ToString()
  {
    return $"DirectionalLight: color={color}, direction={direction}";
  }
}
