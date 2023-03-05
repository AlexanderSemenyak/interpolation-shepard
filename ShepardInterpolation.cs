namespace InterpolationShepard;

public class ShepardInterpolation
{
    public enum Interpolation
    {
        Basic,
        Modified
    }

    private const double P = 10.0;

    private static readonly Point MaximumPoint = new(1.0f, 1.0f, 1.0f);
    private static readonly Point MinimumPoint = new(0.0f, 0.0f, 0.0f);
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
                octree.Initialize();

                foreach (var volumePoint in _volume)
                    if (Octree.ViableNodes != null)
                        foreach (var viableNode in Octree.ViableNodes)
                            if (viableNode.Contains(volumePoint))
                                foreach (var point in viableNode.GetPoints())
                                {
                                    // shepard method
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

    private float InterpolateModified(Point volumePoint)
    {
        throw new NotImplementedException();
    }

    private float InterpolateBasic(Point volumePoint)
    {
        var denominatorAccumulated = 0.0f;
        var numeratorAccumulated = 0.0f;
        if (volumePoint.X > MaximumPoint.X || volumePoint.Y > MaximumPoint.Y || volumePoint.Z > MaximumPoint.Z ||
            volumePoint.X < MinimumPoint.X || volumePoint.Y < MinimumPoint.Y || volumePoint.Z < MinimumPoint.Z)
            return 0.0f;
        foreach (var point in _points)
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
            var point = new Point(x, y, z);
            _volume.Add(point);
        }

        Console.WriteLine($"LOG: Initialized volume with resolution of {xRes}:{yRes}:{zRes}");
    }
}