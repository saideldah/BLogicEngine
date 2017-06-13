namespace RuleEngine
{
    abstract public class Transpiler
    {
        public static string BLogicToJavaScript(string logicExpression)
        {
            string bLogic = logicExpression;
            bLogic = bLogic.Replace("=", "==");
            bLogic = bLogic.Replace(">=", ">");
            bLogic = bLogic.Replace("<=", "<");
            bLogic = bLogic.Replace("!=", "!");
            bLogic = bLogic.Replace("and", "&&");
            bLogic = bLogic.Replace("or", "||");
            bLogic = bLogic.Replace("Count()", "length");

            return bLogic;
        }
    }
}
