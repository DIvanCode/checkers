namespace Checkers.Libraries;

public class Player
{
    public string? Nickname;
    public int? AliveCheckers;

    public void Init(string nickname, int aliveCheckers)
    {
        Nickname = nickname;
        AliveCheckers = aliveCheckers;
    }
}