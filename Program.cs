namespace InterpolationShepard;

internal static class Program
{
    private static bool IsLinux
    {
        get
        {
            var p = (int)Environment.OSVersion.Platform;
            return p is 4 or 6 or 128;
        }
    }

    private static void Main(string[] args)
    {
        var projectDirectory = IsLinux
            ? Environment.CurrentDirectory
            : Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        var inputFilePath = Path.Combine(projectDirectory + "/data/point-cloud-10k.raw");

        // default values
        string outputFilePath = "output.ppm", type = "modified";
        var parameterP = 10.0;
        float parameterRadius = 0.5f,
            xMin = 0.0f, yMin = 0.0f, zMin = 0.0f,
            xMax = 1.0f, yMax = 1.0f, zMax = 1.0f;
        int xRes = 64, yRes = 64, zRes = 64;

        for (var i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case "--input":
                    inputFilePath = args[i + 1];
                    break;
                case "--output":
                    outputFilePath = args[i + 1];
                    break;
                case "--p":
                    parameterP = Convert.ToDouble(args[i + 1]);
                    break;
                case "--R":
                    parameterRadius = Convert.ToSingle(args[i + 1]);
                    break;
                case "--method":
                    type = args[i + 1];
                    break;
                case "--min-x":
                    xMin = Convert.ToSingle(args[i + 1]);
                    break;
                case "--min-y":
                    yMin = Convert.ToSingle(args[i + 1]);
                    break;
                case "--min-z":
                    zMin = Convert.ToSingle(args[i + 1]);
                    break;
                case "--max-x":
                    xMax = Convert.ToSingle(args[i + 1]);
                    break;
                case "--max-y":
                    yMax = Convert.ToSingle(args[i + 1]);
                    break;
                case "--max-z":
                    zMax = Convert.ToSingle(args[i + 1]);
                    break;
                case "--res-x":
                    xRes = Convert.ToInt32(args[i + 1]);
                    break;
                case "--res-y":
                    yRes = Convert.ToInt32(args[i + 1]);
                    break;
                case "--res-z":
                    zRes = Convert.ToInt32(args[i + 1]);
                    break;
            }

        Console.WriteLine($"LOG: Output file: {outputFilePath}");

        var shepardInterpolation = 
            new ShepardInterpolation(parameterP, parameterRadius, xMin, yMin, zMin, xMax, yMax, zMax);
        shepardInterpolation.LoadData(inputFilePath);
        shepardInterpolation.InterpolateToFile(xRes, yRes, zRes, type, outputFilePath);
    }
}