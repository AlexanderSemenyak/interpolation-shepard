namespace InterpolationShepard;

internal static class Program
{
    private static void Main(string[] args)
    {
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        var filePath = Path.Combine(projectDirectory + "/data/point-cloud-10k.raw");

        var shepardInterpolation = new ShepardInterpolation();
        shepardInterpolation.LoadData(filePath);
        shepardInterpolation.InitializeCubeVolume(8);
        shepardInterpolation.InterpolateToFile("output.ppm");
    }
}