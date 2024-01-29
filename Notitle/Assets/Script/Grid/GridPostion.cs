


using System;

public struct GridPostion : IEquatable<GridPostion>
{
    public int x;
    public int z;

    public GridPostion(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPostion postion &&
               x == postion.x &&
               z == postion.z;
    }

    public bool Equals(GridPostion other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override string ToString()
    {
        return $"x: {x}; z: {z}";
    }

    public static bool operator == (GridPostion a, GridPostion b)
    {
        return a.x == b.x && a.z == b.z ;
    }

    public static bool operator !=(GridPostion a, GridPostion b) 
    { 
        return !(a == b); 
    }

    public static GridPostion operator +(GridPostion a, GridPostion b)
    {
        return new GridPostion(a.x + b.x, a.z + b.z);
    }

    public static GridPostion operator -(GridPostion a, GridPostion b)
    {
        return new GridPostion(a.x - b.x, a.z - b.z);
    }
}