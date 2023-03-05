namespace InterpolationShepard;

internal class Point
{
    public readonly float Value;

    public readonly float X;

    public readonly float Y;

    public readonly float Z;

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

    public override string ToString()
    {
        return $"Point({X}, {Y}, {Z}):{Value}";
    }

    public float DistanceTo(Point point)
    {
        var valueX = X - point.X;
        var valueY = Y - point.Y;
        var valueZ = Z - point.Z;
        return (float)Math.Sqrt(valueX * valueX + valueY * valueY + valueZ * valueZ);
    }
}