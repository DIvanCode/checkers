#nullable enable

using System.Windows.Forms;

namespace Checkers.Libraries;

public class Cell : Button 
{
    public enum CellColor
    {
        White,
        Black
    }
        
    public int Index, Row, Col;
    public CellColor Color;
    public int X, Y;

    public int CellCheckerIndex = -1;
}