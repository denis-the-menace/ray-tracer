public class Vector4
{
  private float[] values;

  public float X { get { return values[0]; } set { values[0] = value; } }
  public float Y { get { return values[1]; } set { values[1] = value; } }
  public float Z { get { return values[2]; } set { values[2] = value; } }
  public float W { get { return values[3]; } set { values[3] = value; } }

  public Vector4()
  {
    values = new float[4];
  }

  public Vector4(float x, float y, float z)
  {
    values = new float[] { x, y, z, 0 };
  }

  public Vector4(float[] nums)
  {
    values = new float[4];
    values[0] = nums[0];
    values[1] = nums[1];
    values[2] = nums[2];
    values[3] = 0;
  }

  public Vector4(float x, float y, float z, float w)
  {
    values = new float[] { x, y, z, w };
  }

  public Vector4(Vector4 v)
  {
    values = new float[] { v.X, v.Y, v.Z, v.W };
  }

  public float Magnitude()
  {
    return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
  }

  public float Dot(Vector4 v)
  {
    return X * v.X + Y * v.Y + Z * v.Z + W * v.W;
  }

  public Vector4 Cross(Vector4 v)
  {
    return new Vector4(
        Y * v.Z - Z * v.Y,
        Z * v.X - X * v.Z,
        X * v.Y - Y * v.X,
        0
    );
  }

  public Vector4 Normalize()
  {
    float magnitude = Magnitude();

    if (magnitude != 0)
    {
      return new Vector4(X / magnitude, Y / magnitude, Z / magnitude, W / magnitude);
    }
    else
    {
      return new Vector4(0, 0, 0, 0);
    }
  }

  public Vector4 Clamp(Vector4 min, Vector4 max)
  {
    return new Vector4(
        Math.Clamp(X, min.X, max.X),
        Math.Clamp(Y, min.Y, max.Y),
        Math.Clamp(Z, min.Z, max.Z),
        Math.Clamp(W, min.W, max.W)
    );
  }

  public Vector4 Clamp(float min, float max)
  {
    return new Vector4(
        Math.Clamp(X, min, max),
        Math.Clamp(Y, min, max),
        Math.Clamp(Z, min, max),
        Math.Clamp(W, min, max)
    );
  }

  public Vector4 Subtract(Vector4 v)
  {
    return new Vector4(X - v.X, Y - v.Y, Z - v.Z, W - v.W);
  }

  public Vector4 Subtract(float scalar)
  {
    return new Vector4(X - scalar, Y - scalar, Z - scalar, W - scalar);
  }

  public Vector4 Add(Vector4 v)
  {
    return new Vector4(X + v.X, Y + v.Y, Z + v.Z, W + v.W);
  }

  public Vector4 Add(float scalar)
  {
    return new Vector4(X + scalar, Y + scalar, Z + scalar, W + scalar);
  }

  public Vector4 Multiply(float scalar)
  {
    return new Vector4(X * scalar, Y * scalar, Z * scalar, W * scalar);
  }

  public Vector4 Multiply(Vector4 v)
  {
    return new Vector4(X * v.X, Y * v.Y, Z * v.Z, W * v.W);
  }

  public Vector4 Divide(float divisor)
  {
    if (divisor != 0)
    {
      return new Vector4(X / divisor, Y / divisor, Z / divisor, W / divisor);
    }
    else
    {
      return new Vector4(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);
    }
  }

  public override string ToString()
  {
    return $"({X}, {Y}, {Z}, {W})";
  }
}
