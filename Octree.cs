namespace InterpolationShepard;

internal class Octree
{
    public const int MaxPointsOctant = 200;

    public static List<OctreeNode>? ViableNodes;
    private readonly OctreeNode _parentNode;

    public Octree(List<Point> points, Point minimumPoint, Point maximumPoint)
    {
        _parentNode = new OctreeNode(points, minimumPoint, maximumPoint);
        ViableNodes = new List<OctreeNode>();
    }

    public void Initialize()
    {
        _parentNode.Initialize();
    }
}

internal class OctreeNode
{
    private readonly List<OctreeNode> _children;
    private readonly Point _maximumPoint;
    private readonly Point _minimumPoint;
    private readonly List<Point> _points;

    public OctreeNode(List<Point> points, Point minimumPoint, Point maximumPoint)
    {
        _points = points;
        _minimumPoint = minimumPoint;
        _maximumPoint = maximumPoint;
        _children = new List<OctreeNode>(8);
    }

    public bool Contains(Point volumePoint)
    {
        return _minimumPoint.X < volumePoint.X && _minimumPoint.Y < volumePoint.Y && _minimumPoint.Z < volumePoint.Z &&
               _maximumPoint.X > volumePoint.X && _maximumPoint.Y > volumePoint.Y && _maximumPoint.Z > volumePoint.Z;
    }

    public void Initialize()
    {
        List<Point> bottomLeftBack = new(),
            bottomLeftFront = new(),
            topLeftBack = new(),
            topLeftFront = new(),
            bottomRightBack = new(),
            bottomRightFront = new(),
            topRightBack = new(),
            topRightFront = new();

        var averagePoint = new Point((_maximumPoint.X + _minimumPoint.X) / 2, (_maximumPoint.Y + _minimumPoint.Y) / 2,
            (_maximumPoint.Z + _minimumPoint.Z) / 2);

        foreach (var point in _points)
            if (point.X < averagePoint.X)
                if (point.Y < averagePoint.Y)
                    if (point.Z < averagePoint.Z)
                        bottomLeftBack.Add(point);
                    else
                        bottomLeftFront.Add(point);
                else if (point.Z < averagePoint.Z)
                    topLeftBack.Add(point);
                else
                    topLeftFront.Add(point);
            else if (point.Y < averagePoint.Y)
                if (point.Z < averagePoint.Z)
                    bottomRightBack.Add(point);
                else
                    bottomRightFront.Add(point);
            else if (point.Z < averagePoint.Z)
                topRightBack.Add(point);
            else
                topRightFront.Add(point);

        _children.Add(new OctreeNode(bottomLeftBack, _minimumPoint, averagePoint));
        _children.Add(new OctreeNode(bottomLeftFront, new Point(_minimumPoint.X, _minimumPoint.Y, averagePoint.Z),
            new Point(averagePoint.X, averagePoint.Y, _maximumPoint.Z)));

        _children.Add(new OctreeNode(topLeftBack, new Point(_minimumPoint.X, averagePoint.Y, _minimumPoint.Z),
            new Point(averagePoint.X, _maximumPoint.Y, averagePoint.Z)));
        _children.Add(new OctreeNode(topLeftFront, new Point(_minimumPoint.X, averagePoint.Y, averagePoint.Z),
            new Point(averagePoint.X, _maximumPoint.Y, _maximumPoint.Z)));

        _children.Add(new OctreeNode(bottomRightBack, new Point(averagePoint.X, _minimumPoint.Y, _minimumPoint.Z),
            new Point(_maximumPoint.X, averagePoint.Y, averagePoint.Z)));
        _children.Add(new OctreeNode(bottomRightFront, new Point(averagePoint.X, _minimumPoint.Y, averagePoint.Z),
            new Point(_maximumPoint.X, averagePoint.Y, _maximumPoint.Z)));

        _children.Add(new OctreeNode(topRightBack, new Point(averagePoint.X, averagePoint.Y, _minimumPoint.Z),
            new Point(_maximumPoint.X, _maximumPoint.Y, averagePoint.Z)));
        _children.Add(new OctreeNode(topRightFront, averagePoint,
            _maximumPoint));

        foreach (var child in _children)
            if (child._points.Count > Octree.MaxPointsOctant)
                child.Initialize();
            else
                Octree.ViableNodes?.Add(child);
    }

    public List<Point> GetPoints()
    {
        return _points;
    }
}