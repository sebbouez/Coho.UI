// *********************************************************
// 
// Coho.UI
// RibbonBar.cs
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Coho.UI.CommandManaging;
using Coho.UI.Controls.Common;
using Coho.UI.Controls.Menus;
using Coho.UI.Controls.Omnibar;
using Coho.UI.Interfaces;
using Coho.UI.Tools;
using Button = System.Windows.Controls.Button;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Coho.UI.Controls.Ribbon;

/// <summary>
/// Represents a RibbonBar control that mimics the Microsoft Sun Valley design.
/// </summary>
[ContentProperty("Items")]
public sealed class RibbonBar : ContentControl, IApplicationMainBarControl
{
    public static readonly DependencyProperty EnableAnimationsProperty =
        DependencyProperty.RegisterAttached(nameof(EnableAnimations), typeof(bool), typeof(RibbonBar), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ExtraButtonsProperty =
        DependencyProperty.RegisterAttached(nameof(ExtraButtons), typeof(List<UIElement>), typeof(RibbonBar), new FrameworkPropertyMetadata(new List<UIElement>(), FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty FileButtonTextProperty =
        DependencyProperty.RegisterAttached(nameof(FileButtonText), typeof(string), typeof(RibbonBar), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.RegisterAttached(nameof(Items), typeof(ObservableCollection<RibbonTabItem>), typeof(RibbonBar), new FrameworkPropertyMetadata(new ObservableCollection<RibbonTabItem>(), FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ShowQATProperty =
        DependencyProperty.RegisterAttached(nameof(ShowQAT), typeof(bool), typeof(RibbonBar), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ShowQATLabelsProperty =
        DependencyProperty.RegisterAttached(nameof(ShowQATLabels), typeof(bool), typeof(RibbonBar),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
    
    private readonly DropDownPopup _ribbonOptionsDropDown = new();

    private StackPanel? _extraButtonsStackPanel;
    private bool _focusFirstChild;
    private RibbonTabPanelToolbar? _headersPanelToolbar;
    private bool _isAnimating;
    private bool _isTabIndicatorExpanded;
    private Grid? _mainGrid;
    private ContextMenu? _qatButtonsContextMenu;
    private RibbonQuickAccessToolbar? _qatToolbar;
    private ToggleButton? _ribbonOptionsButton;
    private RibbonTabItemInnerToolbar? _ribbonTabToolbar;
    private ContextMenu? _standardButtonsContextMenu;
    private Border? _tabIndicator;
    private Border? _tabIndicatorParent;

    public RibbonBar()
    {
        Focusable = false;

        Loaded += RibbonBar_Loaded;
        PreviewMouseMove += RibbonBar_PreviewMouseMove;
        MouseLeave += RibbonBar_MouseLeave;
        SizeChanged += RibbonBar_SizeChanged;
    }

    internal static Dictionary<int, object> RibbonPopupsCache
    {
        get;
        set;
    } = new();

    public bool EnableAnimations
    {
        get
        {
            return (bool) GetValue(EnableAnimationsProperty);
        }
        set
        {
            SetValue(EnableAnimationsProperty, value);
        }
    }

    public List<UIElement> ExtraButtons
    {
        get
        {
            return (List<UIElement>) GetValue(ExtraButtonsProperty);
        }
        set
        {
            SetValue(ExtraButtonsProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets the text of the File button
    /// </summary>
    public string FileButtonText
    {
        get
        {
            return (string) GetValue(FileButtonTextProperty);
        }
        set
        {
            SetValue(FileButtonTextProperty, value);
        }
    }

    public ObservableCollection<RibbonTabItem> Items
    {
        get
        {
            return (ObservableCollection<RibbonTabItem>) GetValue(ItemsProperty);
        }
        set
        {
            SetValue(ItemsProperty, value);
        }
    }

    /// <summary>
    ///     Obtient le dernier onglet sélectionné
    /// </summary>
    public RibbonTabItem? LastTab
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the list of commands that are available in the Quick Access Toolbar area.
    /// </summary>
    /// <remarks>
    /// You should set all the commands in the <see cref="Window.Loaded"/> event.
    /// You should save all the commands in the <see cref="Window.Closing"/> event.
    /// </remarks>
    public List<string> QatCommands
    {
        get;
    } = new();

    /// <summary>
    /// Gets the current activated <see cref="RibbonTabItem"/>.
    /// </summary>
    public RibbonTabItem? SelectedItem
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets or sets the visibility state of the Quick Access Toolbar.
    /// </summary>
    public bool ShowQAT
    {
        get
        {
            return (bool) GetValue(ShowQATProperty);
        }
        set
        {
            SetValue(ShowQATProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets the visibility state of the commands labels in the Quick Access Toolbar.
    /// </summary>
    /// <remarks>
    /// This property should only be used in the Xaml. To change the visibility of the commands labels by code, you should call the 
    /// </remarks>
    public bool ShowQATLabels
    {
        get
        {
            return (bool) GetValue(ShowQATLabelsProperty);
        }
        set
        {
            SetValue(ShowQATLabelsProperty, value);
        }
    }

    /// <summary>
    /// Occurs when the user clicks the FileButton.
    /// </summary>
    public event RoutedEventHandler? FileButtonClicked;

    private void RibbonOptionsButton_Click(object sender, RoutedEventArgs e)
    {
        bool ischecked = ((ToggleButton) sender).IsChecked!.Value;

        if (ischecked)
        {
            _ribbonOptionsDropDown.OpenPopup((UIElement) sender);
        }
        else
        {
            _ribbonOptionsDropDown.ClosePopup();
        }
    }

    private void RibbonOptionsDropDown_PopupVisibilityChanged(object? sender, bool e)
    {
        _ribbonOptionsButton!.IsChecked = e;
    }

    private void AddToQatMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (((ContextMenu) (sender as MenuItem)!.Parent).PlacementTarget is IRibbonCommand cmd)
        {
            AddCommandToQat(cmd);
            ShowQAT = true;
        }
    }

    /// <summary>
    ///     Permet d'animer la largeur de la barre de selection du tab actif
    /// </summary>
    /// <param name="shouldExpand"></param>
    private void AnimIndicatorExpand(bool shouldExpand)
    {
        Thickness thk = shouldExpand ? new Thickness(2, 0, 4, 2) : new Thickness(11, 0, 12, 2);

        if (EnableAnimations)
        {
            ThicknessAnimation animMargin = new()
            {
                Duration = TimeSpan.FromMilliseconds(300),
                To = thk,
                EasingFunction = new CubicEase {EasingMode = EasingMode.EaseInOut}
            };

            Storyboard sb = new();
            sb.SpeedRatio = 1.5;

            Storyboard.SetTargetProperty(animMargin, new PropertyPath(MarginProperty));
            Storyboard.SetTarget(animMargin, _tabIndicator);
            sb.Children.Add(animMargin);

            sb.Completed += delegate
            {
                _isAnimating = false;
                _isTabIndicatorExpanded = shouldExpand;
            };

            _isAnimating = true;
            sb.Begin();
        }
        else
        {
            _tabIndicator!.Margin = thk;
            _isTabIndicatorExpanded = shouldExpand;
        }
    }

    private void AnimIndicatorPosition(bool isMouseEvent = false)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        if (SelectedItem == null)
        {
            return;
        }

        RibbonTabItem tab = SelectedItem;
        bool isTabOverflown = tab.IsOverflown;

        tab.UpdateLayout();

        UIElement? container = (UIElement?) VisualTreeHelper.GetParent(tab);
        if (container != null)
        {
            container.UpdateLayout();
        }
        else
        {
            isTabOverflown = true;
        }

        Point relativeLocation = tab.TranslatePoint(new Point(0, 0), container);

        Thickness thk = new(relativeLocation.X, 5, 0, 0);
        double width = tab.ActualWidth;

        _tabIndicatorParent!.Visibility = isTabOverflown ? Visibility.Collapsed : Visibility.Visible;

        if (EnableAnimations && !isTabOverflown)
        {
            ThicknessAnimation animLeft = new()
            {
                From = _tabIndicatorParent.Margin,
                To = thk,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase {EasingMode = EasingMode.EaseInOut}
            };

            DoubleAnimation animWidth = new()
            {
                From = _tabIndicatorParent.Width,
                To = width,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            ThicknessAnimation animMargin = new()
            {
                Duration = TimeSpan.FromMilliseconds(300),
                To = new Thickness(2, 0, 4, 2),
                EasingFunction = new CubicEase {EasingMode = EasingMode.EaseInOut}
            };

            Storyboard sb = new();
            sb.SpeedRatio = 1.5;

            Storyboard.SetTargetProperty(animLeft, new PropertyPath(MarginProperty));
            Storyboard.SetTarget(animLeft, _tabIndicatorParent);
            sb.Children.Add(animLeft);

            if (isMouseEvent)
            {
                Storyboard.SetTargetProperty(animMargin, new PropertyPath(MarginProperty));
                Storyboard.SetTarget(animMargin, _tabIndicator);
                sb.Children.Add(animMargin);
            }

            Storyboard.SetTargetProperty(animWidth, new PropertyPath(WidthProperty));
            Storyboard.SetTarget(animWidth, _tabIndicatorParent);
            sb.Children.Add(animWidth);

            sb.Begin();
        }
        else
        {
            _tabIndicatorParent.Margin = thk;
            _tabIndicatorParent.Width = width;
        }
    }

    private void BuildOptionsMenu()
    {
        StackPanel st = new();
        MenuItem mi1 = new()
        {
            Header = RibbonText.ToggleQAT
        };
        mi1.Click += Mi1_Click;
        st.Children.Add(mi1);

        MenuItem mi2 = new()
        {
            Header = RibbonText.ToggleQATLabels
        };
        mi2.Click += Mi2_Click;
        st.Children.Add(mi2);

        _ribbonOptionsDropDown.Content = st;
        _ribbonOptionsDropDown.PopupVisibilityChanged += RibbonOptionsDropDown_PopupVisibilityChanged;
        _mainGrid!.Children.Add(_ribbonOptionsDropDown);
    }

    private void BuildQatItemsContextMenu()
    {
        _qatButtonsContextMenu = new ContextMenu();
        MenuItem removeFromQatMenuItem = new() {Header = RibbonText.RemoveFromQAT};
        removeFromQatMenuItem.Click += RemoveFromQatMenuItem_Click;

        _qatButtonsContextMenu.Items.Add(removeFromQatMenuItem);
    }

    private void BuildStandardItemsContextMenu()
    {
        _standardButtonsContextMenu = new AcrylicContextMenu();
        MenuItem addToQatMenuItem = new() {Header = RibbonText.AddToQAT};
        addToQatMenuItem.Click += AddToQatMenuItem_Click;

        _standardButtonsContextMenu.Items.Add(addToQatMenuItem);
    }

    private void FileButton_Click(object sender, RoutedEventArgs e)
    {
        FileButtonClicked?.Invoke(sender, e);
    }

    private void Mi1_Click(object sender, RoutedEventArgs e)
    {
        ShowQAT = !ShowQAT;
        _ribbonOptionsDropDown.ClosePopup();
    }

    private void Mi2_Click(object sender, RoutedEventArgs e)
    {
        _qatToolbar!.ToggleLabels();
        _ribbonOptionsDropDown.ClosePopup();
    }

    private void RemoveFromQatMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (((ContextMenu) (sender as MenuItem)!.Parent).PlacementTarget is IRibbonCommand cmd)
        {
            RemoveCommandFromQat(cmd);
        }
    }

    private void ReorderTabsShortcuts()
    {
        int i = 1;
        foreach (RibbonTabItem item in Items.OfType<RibbonTabItem>())
        {
            if (item.Visibility == Visibility.Visible)
            {
                item.Gesture = i.ToString(CultureInfo.InvariantCulture);
                i++;
            }
        }
    }

    private void RibbonBar_Loaded(object sender, RoutedEventArgs e)
    {
        InternalFrameworkSettings.CurrentMainBarControl = this;

        BuildStandardItemsContextMenu();
        BuildQatItemsContextMenu();

        _extraButtonsStackPanel = (StackPanel?) Template.FindName("ExtraButtonsStackPanel", this);

        if (_extraButtonsStackPanel != null)
        {
            foreach (UIElement element in ExtraButtons)
            {
                _extraButtonsStackPanel.Children.Add(element);
            }
        }

        _ = ApplyTemplate();

        Button fileButton = (Button) Template.FindName("fileButton", this);
        fileButton.Click -= FileButton_Click;
        fileButton.Click += FileButton_Click;

        _mainGrid = (Grid) Template.FindName("MainGrid", this);
        _headersPanelToolbar = (RibbonTabPanelToolbar) Template.FindName("HeaderPanel", this);
        _ribbonTabToolbar = (RibbonTabItemInnerToolbar) Template.FindName("ribbonTabToolbar", this);

        _tabIndicator = (Border) Template.FindName("tabIndicator", this);
        _tabIndicatorParent = (Border) Template.FindName("tabIndicatorHolder", this);

        _ribbonOptionsButton = (ToggleButton) Template.FindName("RibbonOptionsToggleButton", this);
        _ribbonOptionsButton.Click += RibbonOptionsButton_Click;

        Border qatHolder = (Border) Template.FindName("QatToolbarHolder", this);
        _qatToolbar = (RibbonQuickAccessToolbar) qatHolder.Child;
        _qatToolbar.AttachParentRibbon(this);

        foreach (RibbonTabItem item in Items)
        {
            item.ParentRibbon = this;
            _headersPanelToolbar.Items.Add(item);
        }

        SelectFirstTab();

        BuildOptionsMenu();
        AnimIndicatorPosition();

        CommandManager.RebuildCommandsCache(this);
        OmnibarSearchService.RegisterOmnibarSearchService(new RibbonOmnibarSearchService());
    }

    private void RibbonBar_MouseLeave(object sender, MouseEventArgs e)
    {
        AnimIndicatorExpand(false);
    }

    private void RibbonBar_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_isAnimating)
        {
            return;
        }

        bool shouldExpand = e.OriginalSource is FrameworkElement el && el.Name.Equals("tabIndicatorHolder", StringComparison.InvariantCultureIgnoreCase);
        if (shouldExpand && _isTabIndicatorExpanded)
        {
            return;
        }

        if (!shouldExpand && !_isTabIndicatorExpanded)
        {
            return;
        }

        AnimIndicatorExpand(shouldExpand);
    }

    private void RibbonBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        AnimIndicatorPosition();
    }

    private void VisuallyUnSelectAllTabs()
    {
        foreach (RibbonTabItem item in Items)
        {
            item.IsSelected = false;
        }
    }

    internal static Popup? GetRibbonCommandPopup(int key)
    {
        if (RibbonPopupsCache.TryGetValue(key, out object? value))
        {
            return (Popup) value;
        }

        return null;
    }

    internal static DropDownPopup? GetRibbonCommandPopup2(int key)
    {
        if (RibbonPopupsCache.TryGetValue(key, out object? value))
        {
            return (DropDownPopup) value;
        }

        return null;
    }

    internal static void RegisterRibbonCommandPopup(int key, Popup popup)
    {
        RibbonPopupsCache.TryAdd(key, popup);
    }

    internal static void RegisterRibbonCommandPopup(int key, DropDownPopup popup)
    {
        RibbonPopupsCache.TryAdd(key, popup);
    }

    internal void AddCommandToQat(IRibbonCommand cmd)
    {
        _qatToolbar!.AddCmd(cmd);
    }

    ContextMenu IApplicationMainBarControl.GetItemContextMenu(IRibbonCommand cmd)
    {
        if (cmd.IsInQAT)
        {
            return _qatButtonsContextMenu!;
        }

        return _standardButtonsContextMenu!;
    }

    bool IApplicationMainBarControl.HandleKeyboardNavigation(Keys key)
    {
        if (key.ToString().Equals("NumPad0", StringComparison.InvariantCultureIgnoreCase))
        {
            FileButtonClicked?.Invoke(this, null);
            return true;
        }

        foreach (RibbonTabItem item in Items.OfType<RibbonTabItem>())
        {
            if (item.Gesture == key.ToString().Replace("NumPad", "", StringComparison.InvariantCultureIgnoreCase) && item.Visibility == Visibility.Visible)
            {
                SelectTab(item);
                _focusFirstChild = true;
                item.FocusFirstItem();
                return true;
            }
        }

        return false;
    }

    internal void RefreshIndicatorPosition()
    {
    }

    internal void RemoveCommandFromQat(IRibbonCommand cmd)
    {
        _qatToolbar!.RemoveCmd(cmd);
    }

    internal void SelectTab(RibbonTabItem? tabItem, bool isMouseEvent = false)
    {
        if (tabItem == null)
        {
            tabItem = Items.First();
        }

        bool mustAnimate = !tabItem.IsSelected;

        VisuallyUnSelectAllTabs();

        LastTab = SelectedItem;
        SelectedItem = tabItem;
        tabItem.IsSelected = true;
        AnimIndicatorPosition(isMouseEvent);

        _ribbonTabToolbar!.Items.Clear();

        foreach (UIElement item in tabItem.Items)
        {
            _ribbonTabToolbar.Items.Add(item);

            // si on devait mettre le focus sur le premier élément
            // par exemple avec la navigation clavier
            if (_focusFirstChild)
            {
                _ribbonTabToolbar.Focus();
                _focusFirstChild = false;
            }
        }

        if (InternalFrameworkSettings.KeyboardNavigationUIIndicator != null
            && InternalFrameworkSettings.KeyboardNavigationUIIndicator.ShowTips)
        {
            InternalFrameworkSettings.KeyboardNavigationUIIndicator.ShowTips = false;
        }

        if (mustAnimate)
        {
            _ribbonTabToolbar.Animate();
        }
    }

    /// <summary>
    /// Hides all the contextual <see cref="RibbonTabItem"/> and activates the first <see cref="RibbonTabItem"/>.
    /// </summary>
    public void HideAllContextualTabs()
    {
        foreach (RibbonTabItem item in Items)
        {
            if (item.IsContextual)
            {
                item.Visibility = Visibility.Collapsed;
                if (item.IsSelected)
                {
                    SelectFirstTab();
                }
            }
        }

        ReorderTabsShortcuts();
    }

    /// <summary>
    /// Hides the given <see cref="RibbonTabItem"/> and activates the last selected <see cref="RibbonTabItem"/>.
    /// </summary>
    /// <param name="ribbonTab">The <see cref="RibbonTabItem"/> to hide.</param>
    public void HideContextualTab(RibbonTabItem ribbonTab)
    {
        bool mustChange = ribbonTab.IsSelected;
        ribbonTab.Visibility = Visibility.Collapsed;
        if (mustChange)
        {
            RestoreLastTab();
        }

        AnimIndicatorPosition();
        ReorderTabsShortcuts();
    }

    public void RestoreLastTab()
    {
        if (LastTab?.Visibility == Visibility.Visible)
        {
            SelectTab(LastTab);
        }
        else
        {
            SelectFirstTab();
        }
    }

    /// <summary>
    /// Returns the identifier of the provided <paramref name="cmd"/>. It is used to memorize the QAT commands for example. 
    /// </summary>
    /// <param name="cmd">A <see cref="FrameworkElement"/> that belongs to the ribbon children.</param>
    /// <returns>A <see cref="string"/> that is produced using the <see cref="FrameworkElement.Name"/> property.</returns>
    /// <exception cref="NullReferenceException">Occurs when the provided <paramref name="cmd"/> has no <see cref="FrameworkElement.Name"/> property.</exception>
    public string GetCommandIdentifier(FrameworkElement cmd)
    {
        if (string.IsNullOrEmpty(cmd.Name))
        {
            throw new NullReferenceException("The provided component has no 'Name' property.");
        }

        return cmd.Name.GetStaticHashCode().ToString();
    }

    /// <summary>
    /// Activates the first <see cref="RibbonTabItem"/> of <see cref="Items"/> and hides all contextual tabs.
    /// </summary>
    public void SelectFirstTab()
    {
        HideAllContextualTabs();
        SelectTab(Items.First());
    }

    /// <summary>
    /// Shows and activates a contextual tab.
    /// </summary>
    /// <param name="ribbonTab">The <see cref="RibbonTabItem"/> to activate.</param>
    public void ShowContextualTab(RibbonTabItem ribbonTab)
    {
        if (ribbonTab.Visibility != Visibility.Visible)
        {
            ribbonTab.Visibility = Visibility.Visible;
            SelectTab(ribbonTab);
        }

        ReorderTabsShortcuts();
    }
}