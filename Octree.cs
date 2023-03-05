namespace InterpolationShepard;

internal class Octree
{
    private OctreeNode _parentNode;

    public Octree(List<Point> points, Point minimumPoint, Point maximumPoint)
    {
        throw new NotImplementedException();
    }

    public OctreeNode GetParentNode()
    {
        return _parentNode;
    }
}

internal class OctreeNode
{
    private List<OctreeNode> _children;

    public bool Contains(Point volumePoint)
    {
        throw new NotImplementedException();
    }

    public List<Point> GetPoints()
    {
        throw new NotImplementedException();
    }

    public List<OctreeNode> GetChildren()
    {
        return _children;
    }
}