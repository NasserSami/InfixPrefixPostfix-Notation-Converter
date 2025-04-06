using System;
using System.IO;

/// <summary>
/// Extension methods for StreamWriter to create XML files
/// </summary>
public static class XMLExtension
{
    /// <summary>
    /// Writes the XML document declaration
    /// </summary>
    /// <param name="writer">The StreamWriter instance</param>
    public static void WriteStartDocument( this StreamWriter writer )
    {
        writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
    }

    /// <summary>
    /// Writes the opening root element tag
    /// </summary>
    /// <param name="writer">The StreamWriter instance</param>
    public static void WriteStartRootElement( this StreamWriter writer )
    {
        writer.WriteLine("<root>");
    }

    /// <summary>
    /// Writes the closing root element tag
    /// </summary>
    /// <param name="writer">The StreamWriter instance</param>
    public static void WriteEndRootElement( this StreamWriter writer )
    {
        writer.WriteLine("</root>");
    }

    /// <summary>
    /// Writes the opening element tag
    /// </summary>
    /// <param name="writer">The StreamWriter instance</param>
    public static void WriteStartElement( this StreamWriter writer )
    {
        writer.WriteLine("  <elements>");
    }

    /// <summary>
    /// Writes the closing element tag
    /// </summary>
    /// <param name="writer">The StreamWriter instance</param>
    public static void WriteEndElement( this StreamWriter writer )
    {
        writer.WriteLine("  </elements>");
    }

    /// <summary>
    /// Writes an attribute (tag with value)
    /// </summary>
    /// <param name="writer">The StreamWriter instance</param>
    /// <param name="name">The attribute name</param>
    /// <param name="value">The attribute value</param>
    public static void WriteAttribute( this StreamWriter writer, string name, string value )
    {
        writer.WriteLine($"    <{name}>{value}</{name}>");
    }
}