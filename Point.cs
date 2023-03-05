namespace InterpolationShepard;

internal class Point
{
    public Point(float v1, float v2, float v3, float? v4)
    {
        X = v1;
        Y = v2;
        Z = v3;
        if (v4 != null) Value = (float)v4;
    }

    public Point(float v1, float v2, float v3)
    {
        X = v1;
        Y = v2;
        Z = v3;
    }

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public float Value { get; set; }

    public override string ToString()
    {
        return $"Point({X}, {Y}, {Z}):{Value}";
    }

    public bool IsValid()
    {
        return !(X > 1.0f) && !(X < 0.0f) &&
               !(Y > 1.0f) && !(Y < 0.0f) &&
               !(Z > 1.0f) && !(Z < 0.0f);
    }
}