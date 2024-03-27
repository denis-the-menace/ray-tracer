public abstract class Object3D
{
  public float[] color {get; private set;} = new float[3];

  public Object3D(float red, float green, float blue)
  {
    color = new float[] { red, green, blue };
  }

  public abstract bool Intersect(Ray ray, Hit hit, float tMin);
}
