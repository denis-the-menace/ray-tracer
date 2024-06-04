public abstract class Material
{
    public Vector4 diffuseColor;
    public Vector4 reflectiveColor;
    public Vector4 transparentColor;
    public float indexOfRefraction;

    public Material(Vector4 diffuseColor, Vector4 reflectiveColor, Vector4 transparentColor, float indexOfRefraction)
    {
        this.diffuseColor = diffuseColor;
        this.reflectiveColor = reflectiveColor;
        this.transparentColor = transparentColor;
        this.indexOfRefraction = indexOfRefraction;
    }

    public Material(Vector4 diffuseColor)
    {
        this.diffuseColor = diffuseColor;
    }

    public abstract Vector4 Shade(Ray ray, Hit hit, Light light);

    public override string ToString()
    {
        return $"Material: DiffuseColor({diffuseColor}), ReflectiveColor({reflectiveColor}), TransparentColor({transparentColor}), IndexOfRefraction({indexOfRefraction})";
    }
}
