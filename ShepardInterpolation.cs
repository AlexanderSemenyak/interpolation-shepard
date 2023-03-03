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

        Console.WriteLine("LOG: Loaded data");
    }

    public void InterpolateToFile(string outputFile)
    {
        if (File.Exists(outputFile))
            outputFile = outputFile.Replace(".", "Replaceable.");

        var downTime = DateTime.Now;
        var dataOut = new BinaryWriter(new FileStream(outputFile, FileMode.Create));
        foreach (var volumePoint in Volume)
        {
            var shepardInterpolation = Interpolate(volumePoint);
            var ppmValue = (uint)(shepardInterpolation * 255);
            dataOut.Write((byte)ppmValue);
        }

        dataOut.Close();
        var elapsedTime = DateTime.Now - downTime;
        Console.WriteLine(
            $"LOG: Interpolated and wrote data to output file with elapsed time of {elapsedTime.ToString()}");
    }

    private static float Interpolate(Point volumePoint)
    {
        var denominatorAccumulated = 0.0f;
        var numeratorAccumulated = 0.0f;

        foreach (var point in Points)
        {
            var distance = Math.Sqrt(Math.Pow(point.X - volumePoint.X, 2) + Math.Pow(point.Y - volumePoint.Y, 2) +
                                     Math.Pow(point.Z - volumePoint.Z, 2));
            if (distance <= 0.001) // 0.0 never hits
            {
                numeratorAccumulated = point.Value;
                denominatorAccumulated = 1;
                Console.WriteLine($"LOG: Break for point {point} and volume point {volumePoint}");
                break;
            }

            var weight = (float)Math.Pow(distance, P);
            numeratorAccumulated += point.Value / weight;
            denominatorAccumulated += 1 / weight;
        }

        var shepardInterpolation = numeratorAccumulated / denominatorAccumulated;
        return shepardInterpolation;
    }

    public void InitializeVolume(int xRes, int yRes, int zRes)
    {
        for (var i = 0.0f; i < zRes; i++)
        for (var j = 0.0f; j < yRes; j++)
        for (var k = 0.0f; k < xRes; k++)
        {
            var x = k / xRes;
            var y = j / yRes;
            var z = i / zRes;
            var point = new Point(x, y, z, 0);
            Volume.Add(point);
        }

        Console.WriteLine($"LOG: Initialized volume with resolution of {xRes}:{yRes}:{zRes}");
    }
}