namespace InterpolationShepard;

internal class Octree
{
    private readonly OctreeNode _parentNode;
    private List<OctreeNode> _viableNodes;
    private const int MaxPointsPerOctant = 200;
    
    public Octree(List<Point> points, Point minimumPoint, Point maximumPoint)
    {
        _parentNode = new OctreeNode(points, minimumPoint, maximumPoint);
    }

    public void Initialize()
    {
        _parentNode.Initialize();
    }

    public List<OctreeNode> GetViableNodes()
    {
        return _viableNodes;
    }
}

internal class OctreeNode
{
    private readonly List<OctreeNode> _children;
    private readonly Point _maximumPoint;
    private readonly Point _minimumPoint;
    private readonly List<Point> _points;
    private const int MaxPointsOctant = 200;

    public OctreeNode(List<Point> points, Point minimumPoint, Point maximumPoint)
    {
        _points = points;
        _minimumPoint = minimumPoint;
        _maximumPoint = maximumPoint;
        _children = new List<OctreeNode>(8);
    }

    public bool Contains(Point volumePoint)
    {
        throw new NotImplementedException();
    }

    public void Initialize()
    {
        if (!_maximumPoint.IsValid() && !_minimumPoint.IsValid())
            return;

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
        {
            if (child._points.Count > MaxPointsOctant)
                child.Initialize();
        }
    }
}