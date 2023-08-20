// *********************************************************
// 
// Coho.UI
// RibbonToggleButton.cs
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
using System.Windows.Media;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonToggleButton : ToggleButton, IRibbonCommand
{
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.RegisterAttached(nameof(Description), typeof(string), typeof(RibbonToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DisplayProperty =
        DependencyProperty.RegisterAttached(nameof(Display), typeof(RibbonEnums.RibbonButtonDisplay), typeof(RibbonToggleButton), new FrameworkPropertyMetadata(RibbonEnums.RibbonButtonDisplay.IconAndText, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty GestureProperty =
        DependencyProperty.RegisterAttached(nameof(Gesture), typeof(string), typeof(RibbonToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(nameof(Icon), typeof(Brush), typeof(RibbonToggleButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsInQATProperty =
        DependencyProperty.RegisterAttached(nameof(IsInQAT), typeof(bool), typeof(RibbonToggleButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty LockEnabledStateProperty =
        DependencyProperty.RegisterAttached(nameof(LockEnabledState), typeof(bool), typeof(RibbonToggleButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(nameof(Text), typeof(string), typeof(RibbonToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    private RoutedEventHandler? _onClick;

    public RibbonToggleButton()
    {
        SetValue(ToolTipService.ShowOnDisabledProperty, true);
        Loaded += RibbonToggleButton_Loaded;
        Click += RibbonToggleButton_Click;
    }

    event RoutedEventHandler IRibbonCommand.OnClick
    {
        add
        {
            _onClick += value;
        }
        remove
        {
            _onClick -= value;
        }
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

    public RibbonEnums.RibbonButtonDisplay Display
    {
        get
        {
            return (RibbonEnums.RibbonButtonDisplay) GetValue(DisplayProperty);
        }
        set
        {
            SetValue(DisplayProperty, value);
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

    public Brush? Icon
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

    public bool IsInQAT
    {
        get
        {
            return (bool) GetValue(IsInQATProperty);
        }
        set
        {
            SetValue(IsInQATProperty, value);
        }
    }

    public bool LockEnabledState
    {
        get
        {
            return (bool) GetValue(LockEnabledStateProperty);
        }
        set
        {
            SetValue(LockEnabledStateProperty, value);
        }
    }

    IRibbonCommand? IRibbonCommand.OriginalCommand
    {
        get;
        set;
    }

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

    void IRibbonCommand.RaiseClick()
    {
        IsChecked = !IsChecked;
        RaiseEvent(new RoutedEventArgs(ClickEvent));
    }

    private void RibbonToggleButton_Loaded(object sender, RoutedEventArgs e)
    {
        ContextMenu = InternalFrameworkSettings.CurrentMainBarControl!.GetItemContextMenu(this);
    }

    private void RibbonToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _onClick?.Invoke(this, e);
    }
}