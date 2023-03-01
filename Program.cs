namespace InterpolationShepard;

internal static class Program
{
    private static readonly List<Point> _points = new();
    private static readonly List<Point> _volume = new();
    private const double P = 2.0;

    private static void Main(string[] args)
    {
        // this can stay this way
        // Console.WriteLine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));
        var filePath = Path.Combine(@"C:\Git\Masters\nrg\InterpolationShepard\data\point-cloud-1k.raw");

        // Initialization of data points and volume
        LoadData(filePath);
        InitializeCubeVolume(32);

        foreach (var volume_point in _volume)
        {
            var denominator_acummulated = 0.0;
            var numerator_acummulated = 0.0;

            foreach (var point in _points)
            {
                var distance = Math.Sqrt(Math.Pow(point.X - volume_point.X, 2) + Math.Pow(point.Y - volume_point.Y, 2) +
                                         Math.Pow(point.Z - volume_point.Z, 2));
                if (distance <= 0.01) // 0.0 never hits
                {
                    numerator_acummulated = point.Value;
                    denominator_acummulated = 1;
                    Console.WriteLine("Break: for point " + point + " and volume point " + volume_point);
                    break;
                }
                var weight = Math.Pow(distance, P);
                numerator_acummulated += point.Value / weight;
                denominator_acummulated += 1 / weight;
            }
            var shepard_interpolant = numerator_acummulated / denominator_acummulated;
            Console.WriteLine("Interpolation for " + volume_point + " from every points is " +
                              shepard_interpolant);
        }
    }

    private static void InitializeCubeVolume(int resolution)
    {
        for (var i = 0; i < resolution; i++)
        for (var j = 0; j < resolution; j++)
        for (var k = 0; k < resolution; k++)
        {
            // normalized coordinates
            var length = Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));
            var x = (float)(i / length);
            var y = (float)(j / length);
            var z = (float)(k / length);
            _volume.Add(new Point(x, y, z, 0));
        }
    }

    private static void LoadData(string filePath)
    {
        var binReader = new BinaryReader(File.Open(filePath, FileMode.Open));

        var dataPoints = binReader.ReadInt32();

        for (var i = 0; i < dataPoints; i++)
            _points.Add(new Point(binReader.ReadSingle(), binReader.ReadSingle(), binReader.ReadSingle(),
                binReader.ReadSingle()));
    }
}