public class RayTracer
{
  public static Vector4 Trace(SceneObjects scene, Ray ray, float tMin, int depth, float weight, float indexOfRefraction, Hit hit)
  {
    // Base case: if the ray weight is too low or maximum recursion depth is reached
    if (depth <= 0 || weight < 0.01f)
    {
      return new Vector4(0, 0, 0, 0); // No contribution if we've reached the depth limit or the weight is negligible
    }

    Vector4 color = scene.background.ambient.Multiply(hit.material.diffuseColor);
    Vector4 hitPoint = ray.origin.Add(ray.direction.Multiply(hit.t));

    foreach (DirectionalLight light in scene.lights)
    {
      Ray ray2 = new Ray(hitPoint, light.direction.Multiply(-1));
      Hit hit2 = new Hit(float.PositiveInfinity, null);

      if (!scene.group.Intersect(ray2, hit2, tMin))
      {
        color = color.Add(hit.material.Shade(ray, hit, light));
      }
    }

    if (hit.material.reflectiveColor != null && hit.material.reflectiveColor.Magnitude() > 0.0f)
    {
      Vector4 mirrorDir = MirrorDirection(hit.normal, ray.direction);
      Ray reflectedRay = new Ray(hitPoint, mirrorDir);
      Hit reflectedHit = new Hit(float.PositiveInfinity, null);

      if (scene.group.Intersect(reflectedRay, reflectedHit, tMin))
      {
        Vector4 reflectedColor = Trace(scene, reflectedRay, tMin, depth - 1, hit.material.reflectiveColor.Multiply(weight).Magnitude(), indexOfRefraction, reflectedHit);
        color = color.Add(reflectedColor.Multiply(hit.material.reflectiveColor));
      }
    }

    if (hit.material.transparentColor != null && hit.material.transparentColor.Magnitude() > 0.0f)
    {
      Vector4 transmittedDir = TransmittedDirection(hit.normal, ray.direction, 1.0f, indexOfRefraction);
      if (transmittedDir.Magnitude() > 0.0f)
      {
        Ray transmittedRay = new Ray(hitPoint.Subtract(hit.normal.Multiply(tMin)), transmittedDir);
        Hit transmittedHit = new Hit(float.PositiveInfinity, null);

        if (scene.group.Intersect(transmittedRay, transmittedHit, tMin))
        {
          Vector4 transmittedColor = Trace(scene, transmittedRay, tMin, depth - 1, hit.material.transparentColor.Multiply(weight).Magnitude(), hit.material.indexOfRefraction, transmittedHit);
          color = color.Add(transmittedColor.Multiply(hit.material.transparentColor));
        }
      }
    }

    return color;
  }

  public static Vector4 TransmittedDirection(Vector4 normal, Vector4 incoming, float index_i, float index_t)
  {
    float nr = index_i / index_t;
    Vector4 T = normal.Multiply((float)Math.Sqrt(1 - nr * nr * (1 - incoming.Dot(normal) * incoming.Dot(normal)))).Subtract(incoming.Multiply(nr)).Subtract(nr * incoming.Dot(normal));
    return T.Normalize();
  }

  public static Vector4 MirrorDirection(Vector4 normal, Vector4 incoming)
  {
    return incoming.Subtract(normal.Multiply(2 * incoming.Dot(normal))).Normalize();
  }
}
