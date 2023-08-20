// *********************************************************
// 
// Coho.UI RibbonSwitchButton.cs
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

public class RibbonSwitchButton : Button, IRibbonCommand
{
    private RoutedEventHandler? _onClick;

    private void RibbonButton_Click(object sender, RoutedEventArgs e)
    {
        _onClick?.Invoke(this, e);
        //e.Handled = true;
    }

    private void RibbonButton_Loaded(object sender, RoutedEventArgs e)
    {
        ContextMenu = InternalFrameworkSettings.CurrentMainBarControl?.GetItemContextMenu(this);
    }

    public static readonly DependencyProperty DescriptionProperty =
                    DependencyProperty.RegisterAttached(nameof(Description), typeof(string), typeof(RibbonSwitchButton),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DisplayProperty =
        DependencyProperty.RegisterAttached(nameof(Display), typeof(RibbonEnums.RibbonButtonDisplay),
            typeof(RibbonSwitchButton),
            new FrameworkPropertyMetadata(RibbonEnums.RibbonButtonDisplay.IconAndText,
                FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty GestureProperty =
        DependencyProperty.RegisterAttached(nameof(Gesture), typeof(string), typeof(RibbonSwitchButton),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(nameof(Icon), typeof(Brush), typeof(RibbonSwitchButton),
            new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsInQATProperty =
            DependencyProperty.RegisterAttached(nameof(IsInQAT), typeof(bool), typeof(RibbonSwitchButton),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsOverflownProperty =
            DependencyProperty.RegisterAttached(nameof(IsOverflown), typeof(bool), typeof(RibbonSwitchButton),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsSwitchedProperty =
            DependencyProperty.RegisterAttached(nameof(IsSwitched), typeof(bool), typeof(RibbonSwitchButton),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    // public static readonly DependencyProperty ItemDisplayModeProperty =
    //         DependencyProperty.RegisterAttached(nameof(ItemDisplayMode), typeof(RibbonItemDisplayMode), typeof(RibbonSwitchButton),
    //             new FrameworkPropertyMetadata(RibbonItemDisplayMode.Simplified, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty LockEnabledStateProperty =
            DependencyProperty.RegisterAttached(nameof(LockEnabledState), typeof(bool), typeof(RibbonSwitchButton),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty SwitchedIconProperty =
                                DependencyProperty.RegisterAttached(nameof(SwitchedIcon), typeof(Brush), typeof(RibbonSwitchButton),
                new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(nameof(Text), typeof(string), typeof(RibbonSwitchButton),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public RibbonSwitchButton()
    {
        SetValue(ToolTipService.ShowOnDisabledProperty, true);
        Loaded += RibbonButton_Loaded;
        Click += RibbonButton_Click;
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
            return (string)GetValue(DescriptionProperty);
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
            return (RibbonEnums.RibbonButtonDisplay)GetValue(DisplayProperty);
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
            return (string)GetValue(GestureProperty);
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
            return (Brush)GetValue(IconProperty);
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
            return (bool)GetValue(IsInQATProperty);
        }
        set
        {
            SetValue(IsInQATProperty, value);
        }
    }

    public bool IsOverflown
    {
        get
        {
            return (bool)GetValue(IsOverflownProperty);
        }
        set
        {
            SetValue(IsOverflownProperty, value);
        }
    }

    public bool IsSwitched
    {
        get
        {
            return (bool)GetValue(IsSwitchedProperty);
        }
        set
        {
            SetValue(IsSwitchedProperty, value);
        }
    }

    // public RibbonItemDisplayMode ItemDisplayMode
    // {
    //     get
    //     {
    //         return (RibbonItemDisplayMode)GetValue(ItemDisplayModeProperty);
    //     }
    //     set
    //     {
    //         SetValue(ItemDisplayModeProperty, value);
    //     }
    // }

    public bool LockEnabledState
    {
        get
        {
            return (bool)GetValue(LockEnabledStateProperty);
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

    public Brush SwitchedIcon
    {
        get
        {
            return (Brush)GetValue(SwitchedIconProperty);
        }
        set
        {
            SetValue(SwitchedIconProperty, value);
        }
    }

    public string Text
    {
        get
        {
            return (string)GetValue(TextProperty);
        }
        set
        {
            SetValue(TextProperty, value);
        }
    }

    public void RaiseClick()
    {
        RaiseEvent(new RoutedEventArgs(ClickEvent));
    }
}