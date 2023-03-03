namespace InterpolationShepard;

public class ShepardInterpolation
{
    private const double P = 10.0;
    private static readonly List<Point> Points = new();
    private static readonly List<Point> Volume = new();

    public void LoadData(string filePath)
    {
        var binReader = new BinaryReader(File.Open(filePath, FileMode.Open));

        var dataPoints = binReader.ReadInt32();

        for (var i = 0; i < dataPoints; i++)
            Points.Add(new Point(binReader.ReadSingle(), binReader.ReadSingle(), binReader.ReadSingle(),
                binReader.ReadSingle()));
    }

    public void InterpolateToFile(string outputFile)
    {
        var dataOut = new BinaryWriter(new FileStream(outputFile, FileMode.Create));
        foreach (var volumePoint in Volume)
        {
            var denominatorAccumulated = 0.0;
            var numeratorAccumulated = 0.0;

            foreach (var point in Points)
            {
                var distance = Math.Sqrt(Math.Pow(point.X - volumePoint.X, 2) + Math.Pow(point.Y - volumePoint.Y, 2) +
                                         Math.Pow(point.Z - volumePoint.Z, 2));
                if (distance <= 0.001) // 0.0 never hits
                {
                    numeratorAccumulated = point.Value;
                    denominatorAccumulated = 1;
                    Console.WriteLine("Break: for point " + point + " and volume point " + volumePoint);
                    break;
                }

                var weight = Math.Pow(distance, P);
                numeratorAccumulated += point.Value / weight;
                denominatorAccumulated += 1 / weight;
            }

            var shepardInterpolation = numeratorAccumulated / denominatorAccumulated;

            // Console.WriteLine("Interpolation for " + volume_point + " from every points is " +
            //   shepard_interpolant);
            var ppmValue = (uint)(shepardInterpolation * 255);
            dataOut.Write((byte)ppmValue);
        }

        dataOut.Close();
    }

    public void InitializeCubeVolume(int resolution)
    {
        for (var i = 0; i < resolution; i++)
        for (var j = 0; j < resolution; j++)
        for (var k = 0; k < resolution; k++)
        {
            var x = k / (float)resolution;
            var y = j / (float)resolution;
            var z = i / (float)resolution;
            var point = new Point(x, y, z, 0);
            Volume.Add(point);
        }
    }
}