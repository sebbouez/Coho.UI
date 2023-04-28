// *********************************************************
// 
// Coho.UI
// RibbonButtonTooltip.cs
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
using System.Windows.Controls.Primitives;
using Coho.UI.Tools;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonButtonTooltip : ContentControl
{
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.RegisterAttached(nameof(Description), typeof(string), typeof(RibbonButtonTooltip), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(RibbonButtonTooltip), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty GestureProperty =
        DependencyProperty.RegisterAttached(nameof(Gesture), typeof(string), typeof(RibbonButtonTooltip), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public RibbonButtonTooltip()
    {
        Loaded += AdvancedTooltip_Loaded;
    }

    public string Description
    {
        get
        {
            return (string) GetValue(DescriptionProperty);
        }
        set
        {
            SetValue(DescriptionProperty, value);
        }
    }

    public string Title
    {
        get
        {
            return (string) GetValue(TitleProperty);
        }
        set
        {
            SetValue(TitleProperty, value);
        }
    }

    public string Gesture
    {
        get
        {
            return (string) GetValue(GestureProperty);
        }
        set
        {
            SetValue(GestureProperty, value);
        }
    }

    private void AdvancedTooltip_Loaded(object sender, RoutedEventArgs e)
    {
        ToolTip? toolTip = WpfTools.GetClosestParent<ToolTip>(this);
        if (toolTip == null)
        {
            return;
        }

        toolTip.VerticalOffset = 12;
        toolTip.Placement = PlacementMode.Bottom;
    }
}