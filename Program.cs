namespace InterpolationShepard;

internal static class Program
{
    private static bool IsLinux
    {
        get
        {
            var p = (int)Environment.OSVersion.Platform;
            return p == 4 || p == 6 || p == 128;
        }
    }

    private static void Main(string[] args)
    {
        var projectDirectory = IsLinux ? Environment.CurrentDirectory : Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

        var filePath = Path.Combine(projectDirectory + "/data/point-cloud-10k.raw");

        var shepardInterpolation = new ShepardInterpolation();
        shepardInterpolation.LoadData(filePath);
        shepardInterpolation.InitializeVolume(16, 16, 8);
        shepardInterpolation.InterpolateToFile("output.ppm");
    }
}