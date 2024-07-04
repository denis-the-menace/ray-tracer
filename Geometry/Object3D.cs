public abstract class Object3D
{
  public Material material;

  public Object3D(Material material)
  {
    this.material = material;
  }

  public abstract bool Intersect(Ray ray, Hit hit, float tMin);
}
