using System;
using System.Collections.Generic;

/// <summary>
/// Class to compare expression evaluation results
/// </summary>
public class CompareExpressions : IComparer<double>
{
    // Epsilon for floating-point comparison
    private const double Epsilon = 1e-6;

    /// <summary>
    /// Compares two expression evaluation results
    /// </summary>
    /// <param name="x">First evaluation result</param>
    /// <param name="y">Second evaluation result</param>
    /// <returns>0 if equal, -1 if x < y, 1 if x > y</returns>
    public int Compare( double x, double y )
    {
        // Handle special cases like NaN
        if (double.IsNaN(x) && double.IsNaN(y))
            return 0;
        if (double.IsNaN(x))
            return -1;
        if (double.IsNaN(y))
            return 1;

        // Handle infinity
        if (double.IsPositiveInfinity(x) && double.IsPositiveInfinity(y))
            return 0;
        if (double.IsNegativeInfinity(x) && double.IsNegativeInfinity(y))
            return 0;
        if (double.IsPositiveInfinity(x))
            return 1;
        if (double.IsNegativeInfinity(x))
            return -1;
        if (double.IsPositiveInfinity(y))
            return -1;
        if (double.IsNegativeInfinity(y))
            return 1;

        // For floating-point comparisons, use a small epsilon to account for precision errors
        if (Math.Abs(x - y) < Epsilon)
            return 0;  // Values are equal within the epsilon range

        return x < y ? -1 : 1;
    }
}