/*
 * PageFabric.
 * Copyright Sébastien Bouez. All Rights Reserved.
*/

using System;
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

public sealed class RibbonSplitButton : Button, IRibbonCommandWithChildren, IRibbonCommand
{
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.RegisterAttached("Description", typeof(string), typeof(RibbonSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DisplayProperty =
        DependencyProperty.RegisterAttached("Display", typeof(RibbonEnums.RibbonButtonDisplay), typeof(RibbonSplitButton), new FrameworkPropertyMetadata(RibbonEnums.RibbonButtonDisplay.IconAndText, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty DropDownContentProperty =
        DependencyProperty.Register("DropDownContent", typeof(object), typeof(RibbonSplitButton), null);

    public static readonly DependencyProperty GestureProperty =
        DependencyProperty.RegisterAttached("Gesture", typeof(string), typeof(RibbonSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached("Icon", typeof(Brush), typeof(RibbonSplitButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsInQATProperty =
        DependencyProperty.RegisterAttached("IsInQAT", typeof(bool), typeof(RibbonSplitButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty LockEnabledStateProperty =
        DependencyProperty.RegisterAttached("LockEnabledState", typeof(bool), typeof(RibbonSplitButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached("Text", typeof(string), typeof(RibbonSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    private DropDownPopup? _dropDownPopup;
    private Grid? _grid;
    private ToggleButton? _toggleButton;
    private RoutedEventHandler? _onClick;

    public RibbonSplitButton()
    {
        SetValue(ToolTipService.ShowOnDisabledProperty, true);
        Loaded += RibbonSplitButton_Loaded;
        Click += RibbonToggleSplitButton_Click;
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

    event RoutedEventHandler? IRibbonCommand.OnClick
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

    public List<IRibbonCommand> GetSubCommands()
    {
        List<IRibbonCommand> result = new();
        result.AddRange(RibbonButtonWithContentHelper.GetChildrenItems(this, DropDownContent as StackPanel));
        return result;
    }

    public void SetDropDownContent(UIElement content)
    {
        DropDownContent = content;
    }

    private void DropDownPopup_PopupVisibilityChanged(object? sender, bool e)
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
        else if (OriginalCommand != null)
        {
            hash = OriginalCommand.Name.GetStaticHashCode();
        }

        DropDownPopup? p = RibbonBar.GetRibbonCommandPopup2(hash);
        if (p != null)
        {
            ((Grid)p.Parent).Children.Remove(p);
            _grid!.Children.Add(p);

            p.PopupVisibilityChanged -= DropDownPopup_PopupVisibilityChanged;
            p.PopupVisibilityChanged += DropDownPopup_PopupVisibilityChanged;

            p.OpenPopup(this);
        }
    }

    private void RibbonSplitButton_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyTemplate();
        ContextMenu = InternalRibbonSettings.CurrentRibbon!.GetItemContextMenu(this);

        _dropDownPopup = (DropDownPopup?) Template.FindName("DropDownPopupPart", this);
        _toggleButton = (ToggleButton?) Template.FindName("toggleButton", this);
        _grid = (Grid) Template.FindName("gridMain", this);

        if (_dropDownPopup != null)
        {
            _dropDownPopup.PopupVisibilityChanged += DropDownPopup_PopupVisibilityChanged;
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

    private void RibbonSplitButton_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (e.OriginalSource is Border bdr && string.Equals(bdr.Name, "InnerBdrButton",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                RaiseClick();
            }
        }

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

    public void CloseDropDown()
    {
        _dropDownPopup!.ClosePopup();
    }
}