using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Coho.UI.Controls.Common;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonTabItemInnerToolbar : ToolBar
{
    private DropDownPopup? _dropDownPopup;
    private bool _isEnabled = true;
    private ToolBarOverflowPanel? _overflowPanel;
    private ToggleButton? _toggleButton;
    private ToolBarPanel? _toolbarPanel;

    public RibbonTabItemInnerToolbar()
    {
        Style = (Style) FindResource("AnimatedToolbarStyle");
        Loaded += AnimatedToolbar_Loaded;
        SizeChanged += AnimatedToolbar_SizeChanged;
    }

    public new bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            _isEnabled = value;
            foreach (UIElement tile in Items.OfType<UIElement>())
            {
                if (tile is IRibbonCommand btn && !btn.LockEnabledState)
                {
                    tile.IsEnabled = value;
                }
                else if (tile is IRibbonCommandWithChildren)
                {
                    tile.IsEnabled = value;
                }
            }
        }
    }

    internal void Animate()
    {
        if (InternalRibbonSettings.CurrentRibbon!.EnableAnimations)
        {
            Storyboard sb = new();
            DoubleAnimation opacityAnimation = new()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            ThicknessAnimation thicknessAnimation = new()
            {
                From = new Thickness(-12, 0, 0, 0),
                To = new Thickness(0),
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase {EasingMode = EasingMode.EaseOut}
            };

            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(MarginProperty));
            Storyboard.SetTarget(opacityAnimation, this);
            Storyboard.SetTarget(thicknessAnimation, this);
            sb.Children.Add(opacityAnimation);
            sb.Children.Add(thicknessAnimation);
            sb.Begin();
        }
    }

    private void AnimatedToolbar_Loaded(object sender, RoutedEventArgs e)
    {
        _ = ApplyTemplate();
        _toolbarPanel = (ToolBarPanel) Template.FindName("PART_ToolBarPanel", this);
        _overflowPanel = (ToolBarOverflowPanel) Template.FindName("PART_ToolBarOverflowPanel", this);

        SetValue(KeyboardNavigation.TabNavigationProperty, KeyboardNavigationMode.Continue);
        ClipToBounds = true;

        _toggleButton = (ToggleButton?) Template.FindName("OverFlowButton", this);
        _dropDownPopup = (DropDownPopup?) Template.FindName("DropDownPopupPart", this);

        if (_dropDownPopup != null)
        {
            _dropDownPopup.PopupVisibilityChanged += DropDownPopup_PopupVisibilityChanged;
        }

        if (_toggleButton != null)
        {
            _toggleButton.Checked -= RibbonDropDownButton_Checked;
            _toggleButton.Checked += RibbonDropDownButton_Checked;
            _toggleButton.Unchecked -= ToggleButton_Unchecked;
            _toggleButton.Unchecked += ToggleButton_Unchecked;
        }

        Animate();
        RefreshOverflowStatus();
    }

    private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
    {
        _dropDownPopup!.ClosePopup();
    }

    private void RibbonDropDownButton_Checked(object sender, RoutedEventArgs e)
    {
        _dropDownPopup!.OpenPopup(_toggleButton!);
    }

    private void DropDownPopup_PopupVisibilityChanged(object? sender, bool e)
    {
        if (e)
        {
            _toggleButton!.IsChecked = true;
            _overflowPanel!.Children.OfType<UIElement>().FirstOrDefault()?.Focus();
            FocusManager.SetFocusedElement(_overflowPanel!.Children.OfType<UIElement>().FirstOrDefault(), null);
            _overflowPanel.Focus();
            _dropDownPopup!.Focus();
        }
        else
        {
            _toggleButton!.IsChecked = false;
        }
    }

    private void AnimatedToolbar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        foreach (UIElement tile in Items.OfType<UIElement>())
        {
            if (tile is RibbonDropDownButton btn)
            {
                btn.IsOverflown = false;
                btn.Width = double.NaN;
            }
            else if (tile is RibbonButton b2)
            {
                b2.IsOverflown = false;
                b2.Width = double.NaN;
            }
            // todo
            //else if (tile is StylesGallery b3)
            //{
            //    b3.IsOverflown = false;
            //}
        }

        RefreshOverflowStatus();
    }

    /// <summary>
    ///     Hack pour recalculer la taille de la toolbar et afficher/masquer le bouton overflow
    /// </summary>
    private void RefreshOverflowStatus()
    {
        if (_toolbarPanel != null && ActualWidth > 0)
        {
            _toolbarPanel.MaxWidth = ActualWidth - 70 > 0 ? ActualWidth - 70 : ActualWidth;
        }
    }
}