namespace InterpolationShepard;

internal static class Program
{
    private static readonly List<Point> _points = new();
    private static readonly List<Point> _volume = new();
    private const double P = 10.0;

    private static void Main(string[] args)
    {
        // this can stay this way
        // Console.WriteLine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));
        var filePath = Path.Combine(@"/home/wrath/git/interpolation-shepard/data/point-cloud-10k.raw");

        // Initialization of normalized data points and normalized volume
        LoadData(filePath);
        InitializeCubeVolume(16);

        BinaryWriter dataOut = new BinaryWriter(new FileStream("output.ppm", FileMode.Create)); 

        foreach (var volume_point in _volume)
        {
            var shepard_interpolant = 0.0;

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

            if (numerator_acummulated > denominator_acummulated) {
                Console.WriteLine("Error");
                shepard_interpolant = 0.0;
            } else {
                shepard_interpolant = numerator_acummulated / denominator_acummulated;
            }
            
            if (shepard_interpolant > 1.0) {
                Console.WriteLine("Woop error");
                throw new NotSupportedException();
            } else {
                // Console.WriteLine("Interpolation for " + volume_point + " from every points is " +
                                //   shepard_interpolant);
            }

            uint ppmValue = (uint)(shepard_interpolant * 255);
            dataOut.Write((byte) ppmValue);
        }
        dataOut.Close();
    }

    private static void InitializeCubeVolume(int resolution)
    {
        var length = Math.Sqrt(Math.Pow(resolution, 2) + Math.Pow(resolution, 2) + Math.Pow(resolution, 2));
        for (var i = 0; i < resolution; i++)
        for (var j = 0; j < resolution; j++)
        for (var k = 0; k < resolution; k++)
        {
            // normalized coordinates
            // var length = Math.Sqrt(Math.Pow(k, 2) + Math.Pow(j, 2) + Math.Pow(i, 2));
            var x = (float)(k / length);
            var y = (float)(j / length);
            var z = (float)(i / length);
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