using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Class to convert infix expressions to prefix notation
/// </summary>
public class InfixToPrefix
{
    /// <summary>
    /// Converts an infix expression to prefix notation
    /// </summary>
    /// <param name="infix">The infix expression</param>
    /// <returns>The prefix expression</returns>
    public string Convert( string infix )
    {
        // Tokenize the infix expression
        List<string> infixTokens = TokenizeExpression(infix);

        // Reverse the tokens
        infixTokens.Reverse();

        // Replace opening brackets with closing brackets and vice versa
        for (int i = 0; i < infixTokens.Count; i++)
        {
            if (infixTokens[i] == "(")
                infixTokens[i] = ")";
            else if (infixTokens[i] == ")")
                infixTokens[i] = "(";
        }

        // Convert to postfix
        List<string> postfixTokens = ConvertToPostfix(infixTokens);

        // Reverse to get prefix
        postfixTokens.Reverse();

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
    /// Helper method to convert infix tokens to postfix tokens using the Shunting Yard algorithm
    /// </summary>
    private List<string> ConvertToPostfix( List<string> infixTokens )
    {
        List<string> postfixTokens = new List<string>();
        Stack<string> stack = new Stack<string>();

        foreach (string token in infixTokens)
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

        return postfixTokens;
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