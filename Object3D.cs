public abstract class Object3D
{
  protected float[] color;

  public Object3D(float red, float green, float blue)
  {
    color = new float[] { red, green, blue };
  }

  public abstract bool Intersect(Ray ray, Hit hit, float tMin);
}
