namespace InterpolationShepard;

internal static class Program
{
    private static void Main(string[] args)
    {
        // this can stay this way
        // Console.WriteLine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));
        var filePath = Path.Combine(@"C:\Git\Masters\nrg\InterpolationShepard\data\point-cloud-1k.raw");

        var binReader = new BinaryReader(File.Open(filePath, FileMode.Open));

        var dataPoints = binReader.ReadInt32();

        for (int i = 0; i < dataPoints; i++)
        {
            var point = new Point(binReader.ReadSingle(), binReader.ReadSingle(), binReader.ReadSingle(), binReader.ReadSingle());
            Console.WriteLine(point.X + " " + point.Y + " " + point.Z + " " + point.Value);
        }
    }
}