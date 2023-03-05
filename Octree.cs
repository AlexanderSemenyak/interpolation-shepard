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

    public void Initialize(int maxDepth)
    {
        _parentNode.Initialize(maxDepth);
    }
}

internal class OctreeNode
{
    private readonly List<OctreeNode> _children;
    public readonly Point _maximumPoint;
    public readonly Point _minimumPoint;
    private readonly List<Point> _points;

    public OctreeNode(List<Point> points, Point minimumPoint, Point maximumPoint)
    {
        _points = points;
        _minimumPoint = minimumPoint;
        _maximumPoint = maximumPoint;
        _children = new List<OctreeNode>(8);
    }

    public bool Contains(Point point, float radius)
    {
        var expandedMin = new Point(_minimumPoint.X - radius, _minimumPoint.Y - radius, _minimumPoint.Z - radius);
        var expandedMax = new Point(_maximumPoint.X + radius, _maximumPoint.Y + radius, _maximumPoint.Z + radius);

        return point.X >= expandedMin.X && point.X <= expandedMax.X
                                        && point.Y >= expandedMin.Y && point.Y <= expandedMax.Y
                                        && point.Z >= expandedMin.Z && point.Z <= expandedMax.Z;
    }

    public void Initialize(int maxDepth)
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
                child.Initialize(maxDepth - 1);
            else
                Octree.ViableNodes?.Add(child);
    }

    public List<Point> GetPoints()
    {
        return _points;
    }
}