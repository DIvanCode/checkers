namespace Checkers.Libraries;

public class Checker
{
    public enum CheckerOwner
    {
        Player1 = 0,
        Player2 = 1
    }

    public enum CheckerType
    {
        Checker,
        Crown
    }

    public CheckerOwner Owner;
    public CheckerType Type;

    public int Row, Col;
    public int CheckerCellIndex;

    public Checker(CheckerOwner owner)
    {
        Owner = owner;
        Type = CheckerType.Checker;
    }
}