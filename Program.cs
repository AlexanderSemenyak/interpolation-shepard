namespace InterpolationShepard;

internal static class Program
{
    private static void Main(string[] args)
    {
        string? projectDirectory;
        if (IsLinux)
            projectDirectory = Environment.CurrentDirectory;
        else
            projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

        var filePath = Path.Combine(projectDirectory + "/data/point-cloud-10k.raw");

        var shepardInterpolation = new ShepardInterpolation();
        shepardInterpolation.LoadData(filePath);
        shepardInterpolation.InitializeCubeVolume(16, 8, 16);
        shepardInterpolation.InterpolateToFile("output.ppm");
    }

    public static bool IsLinux
    {
        get
        {
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }
    }
}