// *********************************************************
// 
// Coho.UI LoadingRing.cs
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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coho.UI.Controls.LoadingRing;

public class LoadingRing : Control
{
    /// <summary>
    ///     Property for <see cref="Progress" />.
    /// </summary>
    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress),
        typeof(double), typeof(LoadingRing),
        new PropertyMetadata(50d, PropertyChangedCallback));

    /// <summary>
    ///     Property for <see cref="IsIndeterminate" />.
    /// </summary>
    public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
        nameof(IsIndeterminate),
        typeof(bool), typeof(LoadingRing),
        new PropertyMetadata(false));

    /// <summary>
    ///     Property for <see cref="EngAngle" />.
    /// </summary>
    public static readonly DependencyProperty EngAngleProperty = DependencyProperty.Register(nameof(EngAngle),
        typeof(double), typeof(LoadingRing),
        new PropertyMetadata(180.0d));

    /// <summary>
    ///     Property for <see cref="IndeterminateAngle" />.
    /// </summary>
    public static readonly DependencyProperty IndeterminateAngleProperty = DependencyProperty.Register(
        nameof(IndeterminateAngle),
        typeof(double), typeof(LoadingRing),
        new PropertyMetadata(180.0d));

    /// <summary>
    ///     Property for <see cref="CoverRingStroke" />.
    /// </summary>
    public static readonly DependencyProperty CoverRingStrokeProperty =
        DependencyProperty.RegisterAttached(
            nameof(CoverRingStroke),
            typeof(Brush),
            typeof(LoadingRing),
            new FrameworkPropertyMetadata(
                Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender |
                FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    ///     Property for <see cref="CoverRingVisibility" />.
    /// </summary>
    public static readonly DependencyProperty CoverRingVisibilityProperty = DependencyProperty.Register(
        nameof(CoverRingVisibility),
        typeof(Visibility), typeof(LoadingRing),
        new PropertyMetadata(Visibility.Visible));

    /// <summary>
    ///     Gets or sets the progress.
    /// </summary>
    public double Progress
    {
        get
        {
            return (double) GetValue(ProgressProperty);
        }
        set
        {
            SetValue(ProgressProperty, value);
        }
    }

    /// <summary>
    ///     Determines if <see cref="LoadingRing" /> shows actual values (<see langword="false" />)
    ///     or generic, continuous progress feedback (<see langword="true" />).
    /// </summary>
    public bool IsIndeterminate
    {
        get
        {
            return (bool) GetValue(IsIndeterminateProperty);
        }
        set
        {
            SetValue(IsIndeterminateProperty, value);
        }
    }

    public double EngAngle
    {
        get
        {
            return (double) GetValue(EngAngleProperty);
        }
        set
        {
            SetValue(EngAngleProperty, value);
        }
    }

    public double IndeterminateAngle
    {
        get
        {
            return (double) GetValue(IndeterminateAngleProperty);
        }
        internal set
        {
            SetValue(IndeterminateAngleProperty, value);
        }
    }

    /// <summary>
    ///     Background ring fill.
    /// </summary>
    public Brush CoverRingStroke
    {
        get
        {
            return (Brush) GetValue(CoverRingStrokeProperty);
        }
        internal set
        {
            SetValue(CoverRingStrokeProperty, value);
        }
    }

    /// <summary>
    ///     Background ring visibility.
    /// </summary>
    public Visibility CoverRingVisibility
    {
        get
        {
            return (Visibility) GetValue(CoverRingVisibilityProperty);
        }
        internal set
        {
            SetValue(CoverRingVisibilityProperty, value);
        }
    }

    protected void UpdateProgressAngle()
    {
        double percentage = Progress;

        if (percentage > 100)
        {
            percentage = 100;
        }

        if (percentage < 0)
        {
            percentage = 0;
        }

        // (360 / 100) * percentage
        double endAngle = 3.6d * percentage;

        if (endAngle >= 360)
        {
            endAngle = 359;
        }

        EngAngle = endAngle;
    }

    protected static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not LoadingRing control)
        {
            return;
        }

        control.UpdateProgressAngle();
    }
}