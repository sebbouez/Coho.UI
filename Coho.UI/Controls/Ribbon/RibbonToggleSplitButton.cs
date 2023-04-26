// *********************************************************
// 
// Coho.UI
// RibbonToggleSplitButton.cs
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

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Coho.UI.Controls.Common;
using Coho.UI.Tools;
using static Coho.UI.Controls.Ribbon.RibbonEnums;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonToggleSplitButton : ToggleButton, IRibbonCommandWithChildren, IRibbonCommand
{
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.RegisterAttached("Description", typeof(string), typeof(RibbonToggleSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DisplayProperty =
        DependencyProperty.RegisterAttached("Display", typeof(RibbonButtonDisplay), typeof(RibbonToggleSplitButton), new FrameworkPropertyMetadata(RibbonButtonDisplay.IconAndText, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DropDownContentProperty =
        DependencyProperty.Register("DropDownContent", typeof(object), typeof(RibbonToggleSplitButton), null);

    public static readonly DependencyProperty GestureProperty =
        DependencyProperty.RegisterAttached("Gesture", typeof(string), typeof(RibbonToggleSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached("Icon", typeof(Brush), typeof(RibbonToggleSplitButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsInQATProperty =
        DependencyProperty.RegisterAttached("IsInQAT", typeof(bool), typeof(RibbonToggleSplitButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty LockEnabledStateProperty =
        DependencyProperty.RegisterAttached("LockEnabledState", typeof(bool), typeof(RibbonToggleSplitButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached("Text", typeof(string), typeof(RibbonToggleSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    private DropDownPopup? _dropDownPopup;
    private Grid? _grid;
    private RoutedEventHandler? _onClick;
    private ToggleButton? _toggleButton;

    public RibbonToggleSplitButton()
    {
        SetValue(ToolTipService.ShowOnDisabledProperty, true);

        Click += RibbonToggleSplitButton_Click;
        Loaded += RibbonToggleSplitButton_Loaded;
    }

    public object DropDownContent
    {
        get
        {
            return GetValue(DropDownContentProperty);
        }
        set
        {
            SetValue(DropDownContentProperty, value);
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

    public RibbonButtonDisplay Display
    {
        get
        {
            return (RibbonButtonDisplay) GetValue(DisplayProperty);
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

    public void RaiseClick()
    {
        IsChecked = !IsChecked;
        RaiseEvent(new RoutedEventArgs(ClickEvent));
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

    public object GetDropDownContent()
    {
        return DropDownContent;
    }

    List<IRibbonCommand> IRibbonCommandWithChildren.GetSubCommands()
    {
        List<IRibbonCommand> result = new();
        if (DropDownContent is StackPanel st)
        {
            result.AddRange(RibbonButtonWithContentHelper.GetChildrenItems(this, st));
        }

        return result;
    }

    public void SetDropDownContent(UIElement content)
    {
        DropDownContent = content;
    }

    private void _dropDownPopup_PopupVisibilityChanged(object? sender, bool e)
    {
        if (e)
        {
            if (DropDownContent is StackPanel st)
            {
                st.Children.OfType<UIElement>().FirstOrDefault()?.Focus();
            }
        }
        else
        {
            _toggleButton!.IsChecked = false;
        }
    }

    private void RibbonDropDownButton_Checked(object sender, RoutedEventArgs e)
    {
        int hash = -1;

        if (!string.IsNullOrEmpty(Name))
        {
            hash = Name.GetStaticHashCode();
        }
        else if (((IRibbonCommand) this).OriginalCommand != null)
        {
            hash = ((IRibbonCommand) this).OriginalCommand.Name.GetStaticHashCode();
        }

        DropDownPopup? p = RibbonBar.GetRibbonCommandPopup2(hash);
        if (p != null)
        {
            ((Grid) p.Parent).Children.Remove(p);
            _grid!.Children.Add(p);

            p.PopupVisibilityChanged -= _dropDownPopup_PopupVisibilityChanged;
            p.PopupVisibilityChanged += _dropDownPopup_PopupVisibilityChanged;

            p.OpenPopup(this);
        }
    }

    private void RibbonSplitButton_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            CloseDropDown();
            _toggleButton!.Focus();
            e.Handled = true;
        }
    }

    private void RibbonToggleSplitButton_Click(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is ToggleButton btn && btn.Name == "toggleButton")
        {
            e.Handled = true;
        }
        else
        {
            _onClick?.Invoke(this, e);
        }
    }

    private void RibbonToggleSplitButton_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyTemplate();
        ContextMenu = InternalRibbonSettings.CurrentRibbon!.GetItemContextMenu(this);

        _dropDownPopup = (DropDownPopup?) Template.FindName("DropDownPopupPart", this);
        _grid = (Grid) Template.FindName("gridMain", this);
        _toggleButton = (ToggleButton?) Template.FindName("toggleButton", this);

        if (_dropDownPopup != null)
        {
            _dropDownPopup.PopupVisibilityChanged += _dropDownPopup_PopupVisibilityChanged;
        }

        if (!string.IsNullOrEmpty(Name))
        {
            RibbonBar.RegisterRibbonCommandPopup(Name.GetStaticHashCode(), _dropDownPopup!);
        }

        if (_toggleButton != null)
        {
            _toggleButton.Checked -= RibbonDropDownButton_Checked;
            _toggleButton.Checked += RibbonDropDownButton_Checked;
        }

        PreviewKeyDown -= RibbonSplitButton_PreviewKeyDown;
        PreviewKeyDown += RibbonSplitButton_PreviewKeyDown;
    }

    public void CloseDropDown()
    {
        _dropDownPopup!.ClosePopup();
    }
}