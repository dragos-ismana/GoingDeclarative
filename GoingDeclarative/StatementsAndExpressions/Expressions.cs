
using System.Linq;

namespace GoingDeclarative.StatementsAndExpressions;

public class Expressions
{
    public static int ExpressionIf(int x, int y, bool flag)
    {
        return flag ? x : y;
    }

    public static int ExpressionSwitch(int x, int y, string str)
    {
        return str switch
        {
            "x" => x,
            "y" => y,
            _ => x
        };
    }

    public static int ExpressionForeach()
    {
        var numbers = Enumerable.Range(0, 10);

        int sum = 0;

        return numbers.Aggregate(sum, (state, current) => state + current);
    }
}



