public class Background
{

  public float[] color { get; set; }
  public float[] ambient { get; set; }

  public Background(float[] color, float[] ambient)
  {
    this.color = color;
    this.ambient = ambient;
  }
};
