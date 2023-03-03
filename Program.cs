namespace InterpolationShepard;

internal static class Program
{
    private static void Main(string[] args)
    {
        var filePath = Path.Combine(@"C:\Git\Masters\nrg\InterpolationShepard/data/point-cloud-10k.raw");

        var shepardInterpolation = new ShepardInterpolation();
        shepardInterpolation.LoadData(filePath);
        shepardInterpolation.InitializeCubeVolume(32);
        shepardInterpolation.InterpolateToFile("output.ppm");
    }
}