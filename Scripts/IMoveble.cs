using System.Collections;
using System.Collections.Generic;

interface IMoveble 
{
    float MoveSpeed { get; set; }
    void Move();

}

public enum Direction
{
    NE_TOP_RIGHT,
    E_RIGHT,
    SE_RIGHT_DOWN,
    SW_LEFT_DOWN,
    W_LEFT,
    NW_TOP_LEFT

}

public static class DirectionExtensions
{
    public static Direction Opposite(this Direction direction)
    {
        return (int) direction < 3 ? (direction + 3) : (direction - 3);
    }

    public static Direction Next(this Direction direction)
    {
        return (int)direction < 5 ? (direction + 1) : (0);
    }

    public static bool IsOpposite(this Direction direction1, Direction direction2)
    {
        if (direction1 == direction2 + 3 || direction1 == direction2 - 3)
        {
            return true;
        }
        else return false;
    }
}