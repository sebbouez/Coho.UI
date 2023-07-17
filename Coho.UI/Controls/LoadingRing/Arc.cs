// *********************************************************
// 
// Coho.UI Arc.cs
// Copyright (c) Sébastien Bouez. All rights reserved.
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// *********************************************************

/*
Original source taken from https://github.com/lepoco/wpfui

MIT License

Copyright (c) 2021-2023 Leszek Pomianowski and WPF UI Contributors. https://dev.lepo.co/

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Coho.UI.Controls.LoadingRing;

public class Arc : Shape
{
    /// <summary>
    ///     Property for <see cref="StartAngle" />.
    /// </summary>
    public static readonly DependencyProperty StartAngleProperty =
        DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(Arc),
            new PropertyMetadata(0.0d, PropertyChangedCallback));

    /// <summary>
    ///     Property for <see cref="EndAngle" />.
    /// </summary>
    public static readonly DependencyProperty EndAngleProperty =
        DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(Arc),
            new PropertyMetadata(0.0d, PropertyChangedCallback));

    /// <summary>
    ///     Overrides default properties.
    /// </summary>
    static Arc()
    {
        StrokeStartLineCapProperty.OverrideMetadata(
            typeof(Arc),
            new FrameworkPropertyMetadata(PenLineCap.Round)
        );

        StrokeEndLineCapProperty.OverrideMetadata(
            typeof(Arc),
            new FrameworkPropertyMetadata(PenLineCap.Round)
        );
    }

    /// <summary>
    ///     Gets or sets the initial angle from which the arc will be drawn.
    /// </summary>
    public double StartAngle
    {
        get
        {
            return (double) GetValue(StartAngleProperty);
        }
        set
        {
            SetValue(StartAngleProperty, value);
        }
    }

    /// <summary>
    ///     Gets or sets the final angle from which the arc will be drawn.
    /// </summary>
    public double EndAngle
    {
        get
        {
            return (double) GetValue(EndAngleProperty);
        }
        set
        {
            SetValue(EndAngleProperty, value);
        }
    }

    /// <summary>
    ///     If IsLargeArc is <see langword="true" />, then one of the two larger arc sweeps is chosen; otherwise, if is
    ///     <see langword="false" />, one of the smaller arc sweeps is chosen.
    /// </summary>
    public bool IsLargeArc
    {
        get;
        internal set;
    }

    /// <inheritdoc />
    protected override Geometry DefiningGeometry
    {
        get
        {
            return GetDefiningGeometry();
        }
    }

    /// <summary>
    ///     Get the geometry that defines this shape.
    ///     <para>
    ///         <see href="https://stackoverflow.com/a/36756365/13224348">Based on Mark Feldman implementation.</see>
    ///     </para>
    /// </summary>
    protected Geometry GetDefiningGeometry()
    {
        StreamGeometry geometryStream = new StreamGeometry();
        Size arcSize = new Size(
            Math.Max(0, (RenderSize.Width - StrokeThickness) / 2),
            Math.Max(0, (RenderSize.Height - StrokeThickness) / 2)
        );

        using (StreamGeometryContext context = geometryStream.Open())
        {
            context.BeginFigure(
                PointAtAngle(Math.Min(StartAngle, EndAngle)),
                false,
                false
            );

            context.ArcTo(
                PointAtAngle(Math.Max(StartAngle, EndAngle)),
                arcSize,
                0,
                IsLargeArc,
                SweepDirection.Counterclockwise,
                true,
                false
            );
        }

        geometryStream.Transform = new TranslateTransform(StrokeThickness / 2, StrokeThickness / 2);

        return geometryStream;
    }

    /// <summary>
    ///     Draws a point on the coordinates of the given angle.
    ///     <para>
    ///         <see href="https://stackoverflow.com/a/36756365/13224348">Based on Mark Feldman implementation.</see>
    ///     </para>
    /// </summary>
    /// <param name="angle">The angle at which to create the point.</param>
    protected Point PointAtAngle(double angle)
    {
        double radAngle = angle * (Math.PI / 180);
        double xRadius = (RenderSize.Width - StrokeThickness) / 2;
        double yRadius = (RenderSize.Height - StrokeThickness) / 2;

        return new Point(
            xRadius + xRadius * Math.Cos(radAngle),
            yRadius - yRadius * Math.Sin(radAngle)
        );
    }

    /// <summary>
    ///     Event triggered when one of the key parameters is changed. Forces the geometry to be redrawn.
    /// </summary>
    protected static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Arc control)
        {
            return;
        }

        control.IsLargeArc = Math.Abs(control.EndAngle - control.StartAngle) > 180;

        // Force complete new layout pass
        control.InvalidateVisual();
    }
}