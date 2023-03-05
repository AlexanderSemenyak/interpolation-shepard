namespace InterpolationShepard;

internal class ShepardInterpolation
{
    public enum Interpolation
    {
        Basic,
        Modified
    }

    private static double _p = 10.0f;
    private static float _r = 0.5f;

    private static Point _maximumPoint = new(1.0f, 1.0f, 1.0f);
    private static Point _minimumPoint = new(0.0f, 0.0f, 0.0f);
    private readonly List<Point> _points = new();
    private readonly List<uint> _writeValues = new();

    public ShepardInterpolation(double parameterP, float parameterRadius, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
        _p = parameterP;
        _r = parameterRadius;
        _minimumPoint = new Point(xMin, yMin, zMin);
        _maximumPoint = new Point(xMax, yMax, zMax);
        Console.WriteLine("LOG: Arguments set");
    }

    public void LoadData(string filePath)
    {
        var binReader = new BinaryReader(File.Open(filePath, FileMode.Open));
        var dataPoints = binReader.ReadInt32();

        for (var i = 0; i < dataPoints; i++)
            _points.Add(new Point(binReader.ReadSingle(), binReader.ReadSingle(), binReader.ReadSingle(),
                binReader.ReadSingle()));

        Console.WriteLine($"LOG: Loaded data with path: {filePath}");
    }

    public void InterpolateToFile(int xRes, int yRes, int zRes, Interpolation type, string outputFile)
    {
        if (File.Exists(outputFile))
            outputFile = outputFile.Replace(".", "Replaceable.");

        var downTime = DateTime.Now;
        switch (type)
        {
            case Interpolation.Basic:
            {
                for (var k = 0.0f; k < zRes; k++)
                for (var j = 0.0f; j < yRes; j++)
                for (var i = 0.0f; i < xRes; i++)
                {
                    var x = i / xRes;
                    var y = j / yRes;
                    var z = k / zRes;
                    var volumePoint = new Point(x, y, z);
                    var shepardInterpolation = InterpolateBasic(volumePoint);

                    // Console.WriteLine($"LOG: Finished {(double)i / volumePoint.Length * 100} % of the volume");

                    var ppmValue = (uint)(shepardInterpolation * 255.0f);
                    _writeValues.Add(ppmValue);
                }

                break;
            }
            case Interpolation.Modified:
            {
                var octree = new Octree(_points, _minimumPoint, _maximumPoint);
                octree.Initialize(8);

                for (var k = 0.0f; k < zRes; k++)
                for (var j = 0.0f; j < yRes; j++)
                for (var i = 0.0f; i < xRes; i++)
                {
                    var x = i / xRes;
                    var y = j / yRes;
                    var z = k / zRes;
                    var volumePoint = new Point(x, y, z);

                    var denominator = 0.0f;
                    var numerator = 0.0f;

                    if (Octree.ViableNodes == null) continue;

                    foreach (var viableNode in Octree.ViableNodes)
                        // if (viableNode.Contains(volume[i], R))
                    foreach (var point in viableNode.GetPoints())
                    {
                        var distance = volumePoint.DistanceTo(point);

                        if (distance <= 0.001) // 0.0 never hits
                        {
                            numerator = point.Value;
                            denominator = 1;
                            Console.WriteLine($"LOG: Break for point {point} and volume point {volumePoint}");
                            break;
                        }

                        if (!(distance < _r)) continue;
                        var value = (_r - distance) / (_r * distance);
                        var weight = value * value;

                        denominator += weight;
                        numerator += weight * point.Value;
                    }

                    var shInterpolationModified = numerator / denominator;
                    var ppmValue = (uint)(shInterpolationModified * 255.0f);

                    _writeValues.Add(ppmValue);
                    // TODO: temp solution
                    // if (volumePoint.X > MaximumPoint.X || volumePoint.Y > MaximumPoint.Y ||
                    //     volumePoint.Z > MaximumPoint.Z || volumePoint.X < MinimumPoint.X ||
                    //     volumePoint.Y < MinimumPoint.Y || volumePoint.Z < MinimumPoint.Z)
                    //     ppmValue = 0;
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        var elapsedTime = DateTime.Now - downTime;
        Console.WriteLine(
            $"LOG: Interpolated and wrote data to output file with elapsed time of {elapsedTime.ToString()}");

        Console.Write("LOG: Writing to file...");
        var dataOut = new BinaryWriter(new FileStream(outputFile, FileMode.Create));
        foreach (var writeValue in _writeValues) dataOut.Write((byte)writeValue);
        dataOut.Close();
        Console.Write("DONE, exiting");
    }

    private float InterpolateBasic(Point volumePoint)
    {
        var denominator = 0.0f;
        var numerator = 0.0f;
        if (volumePoint.X > _maximumPoint.X || volumePoint.Y > _maximumPoint.Y || volumePoint.Z > _maximumPoint.Z ||
            volumePoint.X < _minimumPoint.X || volumePoint.Y < _minimumPoint.Y || volumePoint.Z < _minimumPoint.Z)
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

            var weight = (float)Math.Pow(distance, _p);
            numerator += point.Value / weight;
            denominator += 1 / weight;
        }

        var shepardInterpolation = numerator / denominator;
        return shepardInterpolation;
    }

    public Point[] InitializeVolume(int xRes, int yRes, int zRes)
    {
        var volume = new Point[xRes * yRes * zRes];
        var index = 0;
        for (var i = 0.0f; i < zRes; i++)
        for (var j = 0.0f; j < yRes; j++)
        for (var k = 0.0f; k < xRes; k++)
        {
            var x = k / xRes;
            var y = j / yRes;
            var z = i / zRes;
            var point = new Point(x, y, z);
            volume[index] = point;
            index++;
        }

        Console.WriteLine($"LOG: Initialized volume with resolution of {xRes}:{yRes}:{zRes}");

        return volume;
    }
}