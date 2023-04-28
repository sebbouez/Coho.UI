// *********************************************************
// 
// Coho.UI
// RibbonExtraButton.cs
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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coho.UI.Controls.Ribbon;

public class RibbonExtraButton : Button
{
    public static readonly DependencyProperty IsPrimaryProperty =
        DependencyProperty.RegisterAttached(nameof(IsPrimary), typeof(bool), typeof(RibbonExtraButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(nameof(Text), typeof(string), typeof(RibbonExtraButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(nameof(Icon), typeof(Brush), typeof(RibbonExtraButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    ///     Gets or sets the text of the button
    /// </summary>
    public string Text
    {
        get
        {
            return (string) GetValue(TextProperty);
        }
        set
        {
            SetValue(TextProperty, value);
        }
    }

    /// <summary>
    ///     Gets or sets the icon of the button
    /// </summary>
    public Brush Icon
    {
        get
        {
            return (Brush) GetValue(IconProperty);
        }
        set
        {
            SetValue(IconProperty, value);
        }
    }

    /// <summary>
    ///     Gets or sets if the button should be displayed as primary choice
    /// </summary>
    public bool IsPrimary
    {
        get
        {
            return (bool) GetValue(IsPrimaryProperty);
        }
        set
        {
            SetValue(IsPrimaryProperty, value);
        }
    }
}