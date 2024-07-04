public class Hit
{
  public float t { get; set; } // Distance of the closest intersection
  public Material material { get; set; }
  public Vector4 normal { get; set; }

  public Hit(float t, Material material)
  {
    this.t = t;
    this.material = material;
    this.normal = new Vector4(0, 0, 0, 0);
  }

  public override string ToString()
  {
    return $"Hit: t={t}, normal={normal} material={material}";
  }
}
