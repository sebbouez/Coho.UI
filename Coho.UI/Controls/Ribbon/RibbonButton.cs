// *********************************************************
// 
// Coho.UI
// RibbonButton.cs
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

public sealed class RibbonButton : Button, IRibbonCommand
{
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.RegisterAttached(nameof(Description), typeof(string), typeof(RibbonButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DisplayProperty =
        DependencyProperty.RegisterAttached(nameof(Display), typeof(RibbonEnums.RibbonButtonDisplay), typeof(RibbonButton), new FrameworkPropertyMetadata(RibbonEnums.RibbonButtonDisplay.IconAndText, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty GestureProperty =
        DependencyProperty.RegisterAttached(nameof(Gesture), typeof(string), typeof(RibbonButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(nameof(Icon), typeof(Brush), typeof(RibbonButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsInQATProperty =
        DependencyProperty.RegisterAttached(nameof(IsInQAT), typeof(bool), typeof(RibbonButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsOverflownProperty =
        DependencyProperty.RegisterAttached(nameof(IsOverflown), typeof(bool), typeof(RibbonButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty LockEnabledStateProperty =
        DependencyProperty.RegisterAttached(nameof(LockEnabledState), typeof(bool), typeof(RibbonButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(nameof(Text), typeof(string), typeof(RibbonButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    private RoutedEventHandler? _onClick;

    public RibbonButton()
    {
        SetValue(ToolTipService.ShowOnDisabledProperty, true);
        Loaded += RibbonButton_Loaded;
        Click += RibbonButton_Click;
    }

    public bool IsOverflown
    {
        get
        {
            return (bool) GetValue(IsOverflownProperty);
        }
        set
        {
            SetValue(IsOverflownProperty, value);
        }
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

    /// <summary>
    ///     Gets or sets the text displayed in the button
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

    void IRibbonCommand.RaiseClick()
    {
        RaiseEvent(new RoutedEventArgs(ClickEvent));
    }

    private void RibbonButton_Loaded(object sender, RoutedEventArgs e)
    {
        ContextMenu = InternalRibbonSettings.CurrentRibbon!.GetItemContextMenu(this);
    }

    private void RibbonButton_Click(object sender, RoutedEventArgs e)
    {
        _onClick?.Invoke(this, e);
        e.Handled = true;
    }
}