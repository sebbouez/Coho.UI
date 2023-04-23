/*
 * PageFabric.
 * Copyright Sébastien Bouez. All Rights Reserved.
*/

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Coho.UI.Controls.Common;
using Coho.UI.Tools;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonDropDownButton : ContentControl, IRibbonCommandWithChildren, IRibbonCommand
{
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.RegisterAttached("Description", typeof(string), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DisplayProperty =
        DependencyProperty.RegisterAttached("Display", typeof(RibbonEnums.RibbonButtonDisplay), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(RibbonEnums.RibbonButtonDisplay.IconAndText, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty GestureProperty =
        DependencyProperty.RegisterAttached("Gesture", typeof(string), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached("Icon", typeof(Brush), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsContextualProperty =
        DependencyProperty.RegisterAttached("IsContextual", typeof(bool), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsInQATProperty =
        DependencyProperty.RegisterAttached("IsInQAT", typeof(bool), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsOverflownProperty =
        DependencyProperty.RegisterAttached("IsOverflown", typeof(bool), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty LockEnabledStateProperty =
        DependencyProperty.RegisterAttached("LockEnabledState", typeof(bool), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached("Text", typeof(string), typeof(RibbonDropDownButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    private DropDownPopup? _dropDownPopup;
    private Grid? _grid;
    private ToggleButton? _toggleButton;
    private RoutedEventHandler? _onClick;

    public RibbonDropDownButton()
    {
        SetValue(ToolTipService.ShowOnDisabledProperty, true);
        Loaded += RibbonDropDownButton_Loaded;
    }

    public bool IsContextual
    {
        get
        {
            return (bool) GetValue(IsContextualProperty);
        }
        set
        {
            SetValue(IsContextualProperty, value);
        }
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

    public IRibbonCommand? OriginalCommand
    {
        get;
        set;
    }

    public void RaiseClick()
    {
        RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
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

    public void SetDropDownContent(UIElement content)
    {
        Content = content;
    }

    public object GetDropDownContent()
    {
        return Content;
    }

    public List<IRibbonCommand> GetSubCommands()
    {
        List<IRibbonCommand> result = new();
        result.AddRange(RibbonButtonWithContentHelper.GetChildrenItems(this, (StackPanel) Content));
        return result;
    }

    internal event RoutedEventHandler? Click;
    internal event RoutedEventHandler? PopupOpened;

    private void RibbonDropDownButton_Checked(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
        _onClick?.Invoke(this, e);

        int hash = -1;

        if (!string.IsNullOrEmpty(Name))
        {
            hash = Name.GetStaticHashCode();
        }
        else if (OriginalCommand != null)
        {
            hash = OriginalCommand.Name.GetStaticHashCode();
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

    private void RibbonDropDownButton_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyTemplate();

        ContextMenu = InternalRibbonSettings.CurrentRibbon!.GetItemContextMenu(this);

        _dropDownPopup = (DropDownPopup?) Template.FindName("DropDownPopupPart", this);

        _toggleButton = (ToggleButton?) Template.FindName("toggleButton", this);
        _grid = (Grid) Template.FindName("gridMain", this);


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

        PreviewKeyDown -= RibbonDropDownButton_PreviewKeyDown;
        PreviewKeyDown += RibbonDropDownButton_PreviewKeyDown;
    }

    private void _dropDownPopup_PopupVisibilityChanged(object? sender, bool e)
    {
        if (e)
        {
            PopupOpened?.Invoke(this, new RoutedEventArgs());
            if (Content is StackPanel st)
            {
                st.Children.OfType<UIElement>().FirstOrDefault()?.Focus();
            }
        }
        else
        {
            _toggleButton!.IsChecked = false;
        }
    }

    private void RibbonDropDownButton_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            CloseDropDown();
            _toggleButton!.Focus();
            e.Handled = true;
        }
    }

    public void CloseDropDown()
    {
        _dropDownPopup!.ClosePopup();
    }
}