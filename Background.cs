public class Background
{

  public Vector4 color { get; set; }
  public Vector4 ambient { get; set; }

  public Background(Vector4 color, Vector4 ambient)
  {
    this.color = color;
    this.ambient = ambient;
  }
};
