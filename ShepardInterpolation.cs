namespace InterpolationShepard;

internal class ShepardInterpolation
{
    private static double _p = 10.0f;
    private static float _r = 0.5f;

    private static Point _maximumPoint = new(1.0f, 1.0f, 1.0f);
    private static Point _minimumPoint = new(0.0f, 0.0f, 0.0f);
    private readonly List<Point> _points = new();
    private readonly List<float> _valuesForOutput = new();

    public ShepardInterpolation(double parameterP, float parameterRadius, float xMin, float yMin, float zMin,
        float xMax, float yMax, float zMax)
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

    public void InterpolateToFile(int xRes, int yRes, int zRes, string type, string outputFile, string outputFileVisualization)
    {
        if (File.Exists(outputFileVisualization))
            outputFileVisualization = outputFileVisualization.Replace(".", "Replaceable.");

        var downTime = DateTime.Now;
        switch (type)
        {
            case "basic":
                {
                    for (var k = 0.0f; k < zRes; k++)
                        for (var j = 0.0f; j < yRes; j++)
                            for (var i = 0.0f; i < xRes; i++)
                            {
                                var x = i / xRes;
                                var y = j / yRes;
                                var z = k / zRes;
                                var volumePoint = new Point(x, y, z);
                                var shInterpolationBasic = GetBasicInterpolation(volumePoint);

                                _valuesForOutput.Add(shInterpolationBasic);
                            }

                    break;
                }
            case "modified":
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

                                // TODO: temp solution
                                if (volumePoint.X > _maximumPoint.X || volumePoint.Y > _maximumPoint.Y || volumePoint.Z > _maximumPoint.Z ||
                                    volumePoint.X < _minimumPoint.X || volumePoint.Y < _minimumPoint.Y || volumePoint.Z < _minimumPoint.Z)
                                {
                                    _valuesForOutput.Add(0.0f);
                                }
                                else
                                {
                                    var shInterpolationModified = GetModifiedInterpolation(volumePoint);
                                    _valuesForOutput.Add(shInterpolationModified);
                                }
                            }

                    break;
                }
            default:
                throw new NotImplementedException("ERROR: Not implemented type of interpolation");
        }

        var elapsedTime = DateTime.Now - downTime;
        Console.WriteLine(
            $"LOG: Interpolated and wrote data to output file with elapsed time of {elapsedTime.ToString()}");

        Console.Write("LOG: Writing to visualization file file...");
        var dataOutVisualization = new BinaryWriter(new FileStream(outputFileVisualization, FileMode.Create));
        var dataOutput = new BinaryWriter(new FileStream(outputFile, FileMode.Create));
        foreach (var value in _valuesForOutput)
        {
            var ppmValue = (uint)(value * 255.0f);
            dataOutVisualization.Write((byte)ppmValue);
            dataOutput.Write(value);
        }
        dataOutVisualization.Close();
        dataOutput.Close();
        Console.Write("DONE, exiting\n");
    }

    private static float GetModifiedInterpolation(Point volumePoint)
    {
        var denominator = 0.0f;
        var numerator = 0.0f;

        if (Octree.ViableNodes == null) return 0.0f;

        foreach (var viableNode in Octree.ViableNodes) {
            if (viableNode.Contains(volumePoint, _r))
            {
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
            }
        }

        return numerator / denominator;
    }

    private float GetBasicInterpolation(Point volumePoint)
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

        return numerator / denominator;
    }
}