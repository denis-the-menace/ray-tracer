using System.Text;

public class Group : Object3D
{
  private List<Object3D> objects = new List<Object3D>();

  public Group(params Object3D[] objects) : base(null)
  {
    foreach (Object3D obj in objects)
    {
      AddObject(obj);
    }
  }

  public void AddObject(Object3D obj)
  {
    if (obj == null)
    {
      throw new ArgumentNullException(nameof(obj), "Object cannot be null.");
    }

    objects.Add(obj);
  }

  public override bool Intersect(Ray ray, Hit hit, float tMin)
  {
    bool hitOccurred = false;

    foreach (Object3D obj in objects)
    {
      // Create a temporary Hit to store the intersection information for each object
      Hit tempHit = new Hit(hit.t, hit.material);

      // Check for intersection with the current object in the group
      if (obj.Intersect(ray, tempHit, tMin))
      {
        // If an intersection occurs, update the main Hit if it is closer
        hitOccurred = true;
        if (tempHit.t < hit.t)
        {
          hit.t = tempHit.t;
          hit.material = tempHit.material;
          hit.normal = tempHit.normal;
        }
      }
    }

    return hitOccurred;
  }

  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("Group:");
    foreach (Object3D obj in objects)
    {
      sb.AppendLine(obj.ToString());
    }
    return sb.ToString();
  }
}
