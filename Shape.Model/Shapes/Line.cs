﻿using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Sim.Core;
using Vector.Lib;

namespace Shape.Model;

public class Line
    : Shape
    , ILine
    , IXmlSerializable
{
    private DrawingVisual? drawingVisual;

    private DrawingContext? drawingContext;

    public Vector2 SecondPoint { get; set; }

    public Point SecondPoint2D => new Point(SecondPoint.X, SecondPoint.Y);

    public Line() : base() => Initialize();

    private void Initialize() => drawingVisual = new DrawingVisual();

    public Line(Point massCenter) : base(massCenter) => Initialize();

    public override Visual GetVisual()
    {
        UpdateVisual();
        ArgumentNullException.ThrowIfNull(drawingVisual);
        return drawingVisual;
    }

    public override void UpdateVisual()
    {
        drawingContext = drawingVisual?.RenderOpen();
        DrawShape();
        drawingContext?.Close();
    }

    private void DrawShape()
    {
        ArgumentNullException.ThrowIfNull(MassCenterPoint);
        drawingContext?.DrawLine(GetColorPen(), MassCenterPoint.Value, SecondPoint2D);
    }
    
    public override string ToString() => base.ToString() +
        $"{nameof(SecondPoint)}={SecondPoint}{Environment.NewLine}";

    public override int GetHashCode() => ToString().GetHashCode();

    public override bool Equals(object? theObject) => ToString().Equals(theObject?.ToString());

    public override XmlSchema GetSchema() => new XmlSchema();

    public override void ReadXml(XmlReader reader)
    {
        base.ReadXml(reader);
        if (reader.IsStartElement(nameof(SecondPoint)))
        {
            var valueX = double.Parse(reader.ReadElementString(), CultureInfo.InvariantCulture);
            var valueY = double.Parse(reader.ReadElementString(), CultureInfo.InvariantCulture);
            SecondPoint = new Vector2(valueX, valueY);
            reader.Read();
        }
    }

    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);
        writer.WriteStartElement(nameof(SecondPoint));
        SecondPoint.WriteXml(writer);
        writer.WriteEndElement();
    }
}