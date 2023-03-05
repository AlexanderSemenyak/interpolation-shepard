namespace InterpolationShepard;

public class ShepardInterpolation
{
    public enum Interpolation
    {
        Basic,
        Modified
    }

    private const double P = 10.0f;
    private const float R = 1.0f;

    private static readonly Point MaximumPoint = new(1.0f, 1.0f, 1.0f);
    private static readonly Point MinimumPoint = new(0.4f, 0.0f, 0.0f);
    private readonly List<Point> _points = new();
    private readonly List<Point> _volume = new();

    public void LoadData(string filePath)
    {
        var binReader = new BinaryReader(File.Open(filePath, FileMode.Open));

        var dataPoints = binReader.ReadInt32();

        for (var i = 0; i < dataPoints; i++)
            _points.Add(new Point(binReader.ReadSingle(), binReader.ReadSingle(), binReader.ReadSingle(),
                binReader.ReadSingle()));

        Console.WriteLine("LOG: Loaded data");
    }

    public void InterpolateToFile(Interpolation type, string outputFile)
    {
        if (File.Exists(outputFile))
            outputFile = outputFile.Replace(".", "Replaceable.");

        var downTime = DateTime.Now;
        var dataOut = new BinaryWriter(new FileStream(outputFile, FileMode.Create));
        switch (type)
        {
            case Interpolation.Basic:
            {
                foreach (var volumePoint in _volume)
                {
                    var shepardInterpolation = InterpolateBasic(volumePoint);

                    var ppmValue = (uint)(shepardInterpolation * 255.0f);
                    dataOut.Write((byte)ppmValue);
                }

                break;
            }
            case Interpolation.Modified:
            {
                var octree = new Octree(_points, MinimumPoint, MaximumPoint);
                octree.Initialize(8);

                foreach (var volumePoint in _volume)
                {
                    var denominator = 0.0f;
                    var numerator = 0.0f;

                    if (Octree.ViableNodes != null)
                        foreach (var viableNode in Octree.ViableNodes)
                        {
                            if (viableNode.Contains(volumePoint, R)) {
                                foreach (var point in viableNode.GetPoints())
                                {
                                    var distance = volumePoint.DistanceTo(point);
                                    denominator +=
                                        (float)Math.Pow(Math.Max(0, R - distance) / (R * distance), 2);
                                    numerator +=
                                        (float)Math.Pow(Math.Max(0, R - distance) / (R * distance), 2) * point.Value;
                                }
                            }
                        }

                    var shInterpolationModified = numerator / denominator;
                    var ppmValue = (uint)(shInterpolationModified * 255.0f);
                    
                    // TODO: temp solution
                    if (volumePoint.X > MaximumPoint.X || volumePoint.Y > MaximumPoint.Y ||
                        volumePoint.Z > MaximumPoint.Z ||
                        volumePoint.X < MinimumPoint.X || volumePoint.Y < MinimumPoint.Y ||
                        volumePoint.Z < MinimumPoint.Z)
                        ppmValue = 0;
                    
                    dataOut.Write((byte)ppmValue);
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        dataOut.Close();
        var elapsedTime = DateTime.Now - downTime;
        Console.WriteLine(
            $"LOG: Interpolated and wrote data to output file with elapsed time of {elapsedTime.ToString()}");
    }

    private float InterpolateBasic(Point volumePoint)
    {
        var denominator = 0.0f;
        var numerator = 0.0f;
        if (volumePoint.X > MaximumPoint.X || volumePoint.Y > MaximumPoint.Y || volumePoint.Z > MaximumPoint.Z ||
            volumePoint.X < MinimumPoint.X || volumePoint.Y < MinimumPoint.Y || volumePoint.Z < MinimumPoint.Z)
            return 0.0f;
        foreach (var point in _points)
        {
            var distance = volumePoint.DistanceTo(point);
            if (distance <= 0.001) // 0.0 never hits
            {
                numerator = point.Value;
                denominator = 1;
                Console.WriteLine($"LOG: Break for point {point} and volume point {volumePoint}");
                break;
            }

            var weight = (float)Math.Pow(distance, P);
            numerator += point.Value / weight;
            denominator += 1 / weight;
        }

        var shepardInterpolation = numerator / denominator;
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
            var point = new Point(x, y, z);
            _volume.Add(point);
        }

        Console.WriteLine($"LOG: Initialized volume with resolution of {xRes}:{yRes}:{zRes}");
    }
}