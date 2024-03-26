public class Hit
{
  public float t { get; set; } // Distance of the closest intersection
  public float[] color { get; set; }
  public Vector4 normal { get; set; }

  public Hit(float t, float[] color)
  {
    this.t = t;
    this.color = color;
    this.normal = new Vector4(0, 0, 0, 0);
  }
}
