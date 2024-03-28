public class Matrix4
{
  private float[,] values;

  public Matrix4()
  {
    float[,] m = {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };
    values = m;
  }

  public Matrix4(float[,] m)
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

  public Matrix4 Multiply(Matrix4 other)
  {
    float[,] result = new float[4, 4];

    for (int i = 0; i < 4; i++)
    {
      for (int j = 0; j < 4; j++)
      {
        result[i, j] = 0;
        for (int k = 0; k < 4; k++)
        {
          result[i, j] += values[i, k] * other.values[k, j];
        }
      }
    }

    return new Matrix4(result);
  }

  public Vector4 Multiply(Vector4 vector)
  {
    float[] result = new float[4];

    for (int i = 0; i < 4; i++)
    {
      result[i] = 0;
      result[i] += values[i, 0] * vector.X;
      result[i] += values[i, 1] * vector.Y;
      result[i] += values[i, 2] * vector.Z;
      result[i] += values[i, 3] * vector.W;
    }

    return new Vector4(result);
  }

  public static Matrix4 Translate(float tx, float ty, float tz)
  {
    float[,] translationMatrix = {
            { 1, 0, 0, tx },
            { 0, 1, 0, ty },
            { 0, 0, 1, tz },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(translationMatrix);
  }

  public static Matrix4 Scale(float sx, float sy, float sz)
  {
    float[,] scaleMatrix = {
            { sx, 0, 0, 0 },
            { 0, sy, 0, 0 },
            { 0, 0, sz, 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(scaleMatrix);
  }

  public static Matrix4 RotateX(float angle)
  {
    float cosTheta = MathF.Cos(angle);
    float sinTheta = MathF.Sin(angle);

    float[,] rotationMatrix = {
            { 1, 0, 0, 0 },
            { 0, cosTheta, -sinTheta, 0 },
            { 0, sinTheta, cosTheta, 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(rotationMatrix);
  }

  public static Matrix4 RotateY(float angle)
  {
    float cosTheta = MathF.Cos(angle);
    float sinTheta = MathF.Sin(angle);

    float[,] rotationMatrix = {
            { cosTheta, 0, sinTheta, 0 },
            { 0, 1, 0, 0 },
            { -sinTheta, 0, cosTheta, 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(rotationMatrix);
  }

  public static Matrix4 RotateZ(float angle)
  {
    float cosTheta = MathF.Cos(angle);
    float sinTheta = MathF.Sin(angle);

    float[,] rotationMatrix = {
            { cosTheta, -sinTheta, 0, 0 },
            { sinTheta, cosTheta, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(rotationMatrix);
  }

  public static Matrix4 Rotate(float x, float y, float z, float angle)
  {
    float axisMagnitude = MathF.Sqrt(x * x + y * y + z * z);
    float cosTheta = MathF.Cos(angle);
    float sinTheta = MathF.Sin(angle);
    float oneMinusCosTheta = 1 - cosTheta;

    float[,] rotationMatrix = {
            { (x * x * oneMinusCosTheta + cosTheta), (x * y * oneMinusCosTheta - z * sinTheta), (x * z * oneMinusCosTheta + y * sinTheta), 0 },
            { (y * x * oneMinusCosTheta + z * sinTheta), (y * y * oneMinusCosTheta + cosTheta), (y * z * oneMinusCosTheta - x * sinTheta), 0 },
            { (z * x * oneMinusCosTheta - y * sinTheta), (z * y * oneMinusCosTheta + x * sinTheta), (z * z * oneMinusCosTheta + cosTheta), 0 },
            { 0, 0, 0, 1 }
        };

    return new Matrix4(rotationMatrix);
  }

  public float Determinant()
  {
    return values[0, 3] * values[1, 2] * values[2, 1] * values[3, 0] - values[0, 2] * values[1, 3] * values[2, 1] * values[3, 0] - values[0, 3] * values[1, 1] * values[2, 2] * values[3, 0] + values[0, 1] * values[1, 3] * values[2, 2] * values[3, 0] + values[0, 2] * values[1, 1] * values[2, 3] * values[3, 0] - values[0, 1] * values[1, 2] * values[2, 3] * values[3, 0] - values[0, 3] * values[1, 2] * values[2, 0] * values[3, 1] + values[0, 2] * values[1, 3] * values[2, 0] * values[3, 1] + values[0, 3] * values[1, 0] * values[2, 2] * values[3, 1] - values[0, 0] * values[1, 3] * values[2, 2] * values[3, 1] - values[0, 2] * values[1, 0] * values[2, 3] * values[3, 1] + values[0, 0] * values[1, 2] * values[2, 3] * values[3, 1] + values[0, 3] * values[1, 1] * values[2, 0] * values[3, 2] - values[0, 1] * values[1, 3] * values[2, 0] * values[3, 2] - values[0, 3] * values[1, 0] * values[2, 1] * values[3, 2] + values[0, 0] * values[1, 3] * values[2, 1] * values[3, 2] + values[0, 1] * values[1, 0] * values[2, 3] * values[3, 2] - values[0, 0] * values[1, 1] * values[2, 3] * values[3, 2] - values[0, 2] * values[1, 1] * values[2, 0] * values[3, 3] + values[0, 1] * values[1, 2] * values[2, 0] * values[3, 3] + values[0, 2] * values[1, 0] * values[2, 1] * values[3, 3] - values[0, 0] * values[1, 2] * values[2, 1] * values[3, 3] - values[0, 1] * values[1, 0] * values[2, 2] * values[3, 3] + values[0, 0] * values[1, 1] * values[2, 2] * values[3, 3];
  }

  public Matrix4 Inverse()
  {
    float[,] invValues = new float[4, 4];

    float det = Determinant();
    if (MathF.Abs(det) < 1e-6)
    {
      throw new InvalidOperationException("Matrix is singular, cannot compute inverse.");
    }

    float invDet = 1.0f / det;

    // Calculate matrix of cofactors (adjugate)
    float[,] cofactors = new float[4, 4];
    for (int i = 0; i < 4; i++)
    {
      for (int j = 0; j < 4; j++)
      {
        cofactors[i, j] = Minor(i, j) * MathF.Pow(-1, i + j);
      }
    }

    // Transpose the matrix of cofactors
    float[,] adjugate = new float[4, 4];
    for (int i = 0; i < 4; i++)
    {
      for (int j = 0; j < 4; j++)
      {
        adjugate[j, i] = cofactors[i, j];
      }
    }

    // Calculate inverse by dividing adjugate by determinant
    for (int i = 0; i < 4; i++)
    {
      for (int j = 0; j < 4; j++)
      {
        invValues[i, j] = adjugate[i, j] * invDet;
      }
    }

    return new Matrix4(invValues);
  }

  private float Minor(int row, int col)
  {
    float[,] minorValues = new float[3, 3];
    int m = 0, n = 0;
    for (int i = 0; i < 4; i++)
    {
      if (i == row) continue;
      n = 0;
      for (int j = 0; j < 4; j++)
      {
        if (j == col) continue;
        minorValues[m, n] = values[i, j];
        n++;
      }
      m++;
    }

    float minorDet = minorValues[0, 0] * (minorValues[1, 1] * minorValues[2, 2] - minorValues[1, 2] * minorValues[2, 1]) -
                      minorValues[0, 1] * (minorValues[1, 0] * minorValues[2, 2] - minorValues[1, 2] * minorValues[2, 0]) +
                      minorValues[0, 2] * (minorValues[1, 0] * minorValues[2, 1] - minorValues[1, 1] * minorValues[2, 0]);

    return minorDet;
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
