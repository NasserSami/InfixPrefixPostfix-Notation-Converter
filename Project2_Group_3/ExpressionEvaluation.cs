using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// Class to evaluate prefix and postfix expressions using expression trees
/// </summary>
public class ExpressionEvaluation
{
    /// <summary>
    /// Evaluates a prefix expression using expression trees
    /// </summary>
    /// <param name="prefix">The prefix expression</param>
    /// <returns>The result of the evaluation</returns>
    public double EvaluatePrefix( string prefix )
    {
        try
        {
            // Create a stack to hold operands
            Stack<double> stack = new Stack<double>();

            // Create the expression tree
            Expression<Func<double>> expressionTree = BuildPrefixExpressionTree(prefix);

            // Compile and execute the expression tree
            Func<double> compiledExpression = expressionTree.Compile();
            return compiledExpression();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error evaluating prefix expression: {ex.Message}");
            return double.NaN;
        }
    }

    /// <summary>
    /// Evaluates a postfix expression using expression trees
    /// </summary>
    /// <param name="postfix">The postfix expression</param>
    /// <returns>The result of the evaluation</returns>
    public double EvaluatePostfix( string postfix )
    {
        try
        {
            // Create a stack to hold operands
            Stack<double> stack = new Stack<double>();

            // Create the expression tree
            Expression<Func<double>> expressionTree = BuildPostfixExpressionTree(postfix);

            // Compile and execute the expression tree
            Func<double> compiledExpression = expressionTree.Compile();
            return compiledExpression();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error evaluating postfix expression: {ex.Message}");
            return double.NaN;
        }
    }

    /// <summary>
    /// Builds an expression tree from a prefix expression
    /// </summary>
    private Expression<Func<double>> BuildPrefixExpressionTree( string prefix )
    {
        // Tokenize the expression
        string[] tokens = TokenizeExpression(prefix);
        Stack<Expression> expressionStack = new Stack<Expression>();

        // Process the prefix expression from right to left
        for (int i = tokens.Length - 1; i >= 0; i--)
        {
            string token = tokens[i];

            if (IsOperand(token))
            {
                // If token is an operand, push it to the stack
                double value = double.Parse(token);
                expressionStack.Push(Expression.Constant(value));
            }
            else if (IsOperator(token))
            {
                // If token is an operator, pop two operands and create a binary expression
                if (expressionStack.Count < 2)
                {
                    throw new InvalidOperationException($"Invalid prefix expression: '{prefix}'");
                }

                Expression left = expressionStack.Pop();
                Expression right = expressionStack.Pop();
                Expression operation = CreateBinaryExpression(token[0], left, right);
                expressionStack.Push(operation);
            }
        }

        // The final expression should be the only item in the stack
        if (expressionStack.Count != 1)
        {
            throw new InvalidOperationException($"Invalid prefix expression: '{prefix}'");
        }

        // Create a lambda expression from the final expression
        Expression body = expressionStack.Pop();
        return Expression.Lambda<Func<double>>(body);
    }

    /// <summary>
    /// Builds an expression tree from a postfix expression
    /// </summary>
    private Expression<Func<double>> BuildPostfixExpressionTree( string postfix )
    {
        // Tokenize the expression
        string[] tokens = TokenizeExpression(postfix);
        Stack<Expression> expressionStack = new Stack<Expression>();

        // Process the postfix expression from left to right
        foreach (string token in tokens)
        {
            if (IsOperand(token))
            {
                // If token is an operand, push it to the stack
                double value = double.Parse(token);
                expressionStack.Push(Expression.Constant(value));
            }
            else if (IsOperator(token))
            {
                // If token is an operator, pop two operands and create a binary expression
                if (expressionStack.Count < 2)
                {
                    throw new InvalidOperationException($"Invalid postfix expression: '{postfix}'");
                }

                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                Expression operation = CreateBinaryExpression(token[0], left, right);
                expressionStack.Push(operation);
            }
        }

        // The final expression should be the only item in the stack
        if (expressionStack.Count != 1)
        {
            throw new InvalidOperationException($"Invalid postfix expression: '{postfix}'");
        }

        // Create a lambda expression from the final expression
        Expression body = expressionStack.Pop();
        return Expression.Lambda<Func<double>>(body);
    }

    /// <summary>
    /// Tokenizes an expression into operators and operands
    /// </summary>
    private string[] TokenizeExpression( string expression )
    {
        // This pattern matches operators and numbers (including decimals)
        string pattern = @"[\+\-\*\/\^]|\d+(\.\d+)?";

        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(expression);

        List<string> tokens = new List<string>();
        foreach (Match match in matches)
        {
            tokens.Add(match.Value);
        }

        return tokens.ToArray();
    }

    /// <summary>
    /// Creates a binary expression for the given operator and operands
    /// </summary>
    private Expression CreateBinaryExpression( char op, Expression left, Expression right )
    {
        switch (op)
        {
            case '+':
                return Expression.Add(left, right);
            case '-':
                return Expression.Subtract(left, right);
            case '*':
                return Expression.Multiply(left, right);
            case '/':
                return Expression.Divide(left, right);
            case '^':
                // For exponentiation, use Math.Pow method
                MethodInfo powMethod = typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) });
                return Expression.Call(powMethod, Expression.Convert(left, typeof(double)), Expression.Convert(right, typeof(double)));
            default:
                throw new ArgumentException($"Unsupported operator: {op}");
        }
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
        return token.Length == 1 && (token[0] == '+' || token[0] == '-' || token[0] == '*' || token[0] == '/' || token[0] == '^');
    }
}