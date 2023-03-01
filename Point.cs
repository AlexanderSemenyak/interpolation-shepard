namespace InterpolationShepard
{
    internal class Point
    {
        public Point(float v1, float v2, float v3, float v4)
        {
            X = v1;
            Y = v2;
            Z = v3;
            Value = v4;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float Value { get; set; }

        public override string ToString()
        {
            return $"Point({X}, {Y}, {Z}):{Value}";
        }
    }
}