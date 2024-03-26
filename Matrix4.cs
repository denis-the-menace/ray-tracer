public class Matrix4
{
  private double[,] values;

  public Matrix4()
  {
    values = new double[4, 4];
  }

  public Matrix4(double[,] m)
  {
    if (m.GetLength(0) == 4 && m.GetLength(1) == 4)
    {
      values = m;
    }
    else
    {
      throw new ArgumentException("Input matrix must be of size 4x4.");
    }
  }

  public static Matrix4 Translate(double tx, double ty, double tz)
  {
    double[,] translationMatrix = {
            { 1, 0, 0, tx },
            { 0, 1, 0, ty },
            { 0, 0, 1, tz },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(translationMatrix);
  }

  public static Matrix4 Scale(double sx, double sy, double sz)
  {
    double[,] scaleMatrix = {
            { sx, 0, 0, 0 },
            { 0, sy, 0, 0 },
            { 0, 0, sz, 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(scaleMatrix);
  }

  public static Matrix4 RotateX(double angle)
  {
    double cosTheta = Math.Cos(angle);
    double sinTheta = Math.Sin(angle);

    double[,] rotationMatrix = {
            { 1, 0, 0, 0 },
            { 0, cosTheta, -sinTheta, 0 },
            { 0, sinTheta, cosTheta, 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(rotationMatrix);
  }

  public static Matrix4 RotateY(double angle)
  {
    double cosTheta = Math.Cos(angle);
    double sinTheta = Math.Sin(angle);

    double[,] rotationMatrix = {
            { cosTheta, 0, sinTheta, 0 },
            { 0, 1, 0, 0 },
            { -sinTheta, 0, cosTheta, 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(rotationMatrix);
  }

  public static Matrix4 RotateZ(double angle)
  {
    double cosTheta = Math.Cos(angle);
    double sinTheta = Math.Sin(angle);

    double[,] rotationMatrix = {
            { cosTheta, -sinTheta, 0, 0 },
            { sinTheta, cosTheta, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(rotationMatrix);
  }

  public static Matrix4 Rotate(double x, double y, double z, double angle)
  {
    double axisMagnitude = Math.Sqrt(x * x + y * y + z * z);
    double cosTheta = Math.Cos(angle);
    double sinTheta = Math.Sin(angle);
    double oneMinusCosTheta = 1 - cosTheta;

    double[,] rotationMatrix = {
            { (x * x * oneMinusCosTheta + cosTheta), (x * y * oneMinusCosTheta - z * sinTheta), (x * z * oneMinusCosTheta + y * sinTheta), 0 },
            { (y * x * oneMinusCosTheta + z * sinTheta), (y * y * oneMinusCosTheta + cosTheta), (y * z * oneMinusCosTheta - x * sinTheta), 0 },
            { (z * x * oneMinusCosTheta - y * sinTheta), (z * y * oneMinusCosTheta + x * sinTheta), (z * z * oneMinusCosTheta + cosTheta), 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(rotationMatrix);
  }

  public Vector4 Mult(Vector4 v)
  {
    double[] result = new double[4];

    for (int i = 0; i < 4; i++)
    {
      result[i] += values[i, 0] * v.X;
      result[i] += values[i, 1] * v.Y;
      result[i] += values[i, 2] * v.Z;
      result[i] += values[i, 3] * v.W;

      /* for (int j = 0; j < 4; j++) */
      /* { */
      /*   result[i] += values[i, j] * v[j]; */
      /* } */
    }

    return new Vector4((float)result[0], (float)result[1], (float)result[2], (float)result[3]);
  }

  public double Determinant()
  {
    return values[0, 3] * values[1, 2] * values[2, 1] * values[3, 0] - values[0, 2] * values[1, 3] * values[2, 1] * values[3, 0] - values[0, 3] * values[1, 1] * values[2, 2] * values[3, 0] + values[0, 1] * values[1, 3] * values[2, 2] * values[3, 0] + values[0, 2] * values[1, 1] * values[2, 3] * values[3, 0] - values[0, 1] * values[1, 2] * values[2, 3] * values[3, 0] - values[0, 3] * values[1, 2] * values[2, 0] * values[3, 1] + values[0, 2] * values[1, 3] * values[2, 0] * values[3, 1] + values[0, 3] * values[1, 0] * values[2, 2] * values[3, 1] - values[0, 0] * values[1, 3] * values[2, 2] * values[3, 1] - values[0, 2] * values[1, 0] * values[2, 3] * values[3, 1] + values[0, 0] * values[1, 2] * values[2, 3] * values[3, 1] + values[0, 3] * values[1, 1] * values[2, 0] * values[3, 2] - values[0, 1] * values[1, 3] * values[2, 0] * values[3, 2] - values[0, 3] * values[1, 0] * values[2, 1] * values[3, 2] + values[0, 0] * values[1, 3] * values[2, 1] * values[3, 2] + values[0, 1] * values[1, 0] * values[2, 3] * values[3, 2] - values[0, 0] * values[1, 1] * values[2, 3] * values[3, 2] - values[0, 2] * values[1, 1] * values[2, 0] * values[3, 3] + values[0, 1] * values[1, 2] * values[2, 0] * values[3, 3] + values[0, 2] * values[1, 0] * values[2, 1] * values[3, 3] - values[0, 0] * values[1, 2] * values[2, 1] * values[3, 3] - values[0, 1] * values[1, 0] * values[2, 2] * values[3, 3] + values[0, 0] * values[1, 1] * values[2, 2] * values[3, 3];
  }

  public override string ToString()
  {
    string result = "";

    for (int i = 0; i < 4; i++)
    {
      for (int j = 0; j < 4; j++)
      {
        result += $"{values[i, j],10:F3} ";
      }
      result += "\n";
    }

    return result;
  }
}
