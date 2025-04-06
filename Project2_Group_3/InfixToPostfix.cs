using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Class to convert infix expressions to postfix notation
/// </summary>
public class InfixToPostfix
{
    /// <summary>
    /// Converts an infix expression to postfix notation using the Shunting Yard algorithm
    /// </summary>
    /// <param name="infix">The infix expression</param>
    /// <returns>The postfix expression</returns>
    public string Convert( string infix )
    {
        // Tokenize the infix expression
        List<string> tokens = TokenizeExpression(infix);

        // Convert to postfix
        List<string> postfixTokens = new List<string>();
        Stack<string> stack = new Stack<string>();

        foreach (string token in tokens)
        {
            // If token is an operand
            if (IsOperand(token))
            {
                postfixTokens.Add(token);
            }
            // If token is an opening bracket
            else if (token == "(")
            {
                stack.Push(token);
            }
            // If token is a closing bracket
            else if (token == ")")
            {
                while (stack.Count > 0 && stack.Peek() != "(")
                {
                    postfixTokens.Add(stack.Pop());
                }

                // Pop the opening bracket
                if (stack.Count > 0 && stack.Peek() == "(")
                {
                    stack.Pop();
                }
            }
            // If token is an operator
            else if (IsOperator(token))
            {
                while (stack.Count > 0 && stack.Peek() != "(" &&
                       Precedence(token) <= Precedence(stack.Peek()))
                {
                    postfixTokens.Add(stack.Pop());
                }

                stack.Push(token);
            }
        }

        // Pop remaining operators from the stack
        while (stack.Count > 0)
        {
            postfixTokens.Add(stack.Pop());
        }

        // Join the tokens
        return string.Join("", postfixTokens);
    }

    /// <summary>
    /// Tokenizes an infix expression into operands and operators
    /// </summary>
    private List<string> TokenizeExpression( string expression )
    {
        List<string> tokens = new List<string>();
        string pattern = @"(\d+(\.\d+)?)|[\+\-\*\/\^\(\)]";

        MatchCollection matches = Regex.Matches(expression, pattern);
        foreach (Match match in matches)
        {
            tokens.Add(match.Value);
        }

        return tokens;
    }

    /// <summary>
    /// Checks if a token is an operand (number)
    /// </summary>
    private bool IsOperand( string token )
    {
        return double.TryParse(token, out _);
    }

    /// <summary>
    /// Checks if a token is an operator
    /// </summary>
    private bool IsOperator( string token )
    {
        return token == "+" || token == "-" || token == "*" || token == "/" || token == "^";
    }

    /// <summary>
    /// Gets the precedence value of an operator
    /// </summary>
    private int Precedence( string op )
    {
        switch (op)
        {
            case "+":
            case "-":
                return 1;
            case "*":
            case "/":
                return 2;
            case "^":
                return 3;
            default:
                return 0;
        }
    }
}