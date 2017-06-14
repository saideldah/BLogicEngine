using System;
using Jint;
using System.Web.Script.Serialization;

namespace RuleEngine
{
    public class RuleManager
    {
        /// <summary>
        /// function to validate json object based on JavaScript Logical Expression
        /// </summary>
        /// <param name="jsLogicalExpression">JavaScript Logical Expression</param>
        /// <param name="jsonObjectToValidate">json Object To Validate</param>
        /// <returns>boolean result</returns>
        private static bool evaluateJavaScriptExpression(string jsLogicalExpression, string jsonObjectToValidate)
        {
            var JSEngine = new Engine(cfg => cfg.AllowClr());
            bool outPut = false;

            try
            {
                var executeResult = JSEngine.Execute(@" var object = " + jsonObjectToValidate + "; var output = " + jsLogicalExpression + ";").GetValue("output");
                outPut = Convert.ToBoolean(executeResult.ToString());
            }
            catch (Jint.Runtime.JavaScriptException Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return outPut;
        }

        /// <summary>
        /// function to validate object based on BLogic language Logical Expression 
        /// </summary>
        /// <param name="bLogicalExpression">BLogic Logical Expression</param>
        /// <param name="objectToValidate">C# object To Validate</param>
        /// <returns>boolean result</returns>
        public static bool Evaluate(string bLogicalExpression, object objectToValidate)
        {
            bool evaluationResult = false;
            //Transpile BLogical Expressions to javascript Logical Expressions
            string jsLogicExpression = Transpiler.BLogicToJavaScript(bLogicalExpression);

            //Conver objectToValidate to JSON string
            var jsonString = new JavaScriptSerializer().Serialize(objectToValidate);

            evaluationResult = evaluateJavaScriptExpression(jsLogicExpression, jsonString);
            return evaluationResult;
        }
    }
}
